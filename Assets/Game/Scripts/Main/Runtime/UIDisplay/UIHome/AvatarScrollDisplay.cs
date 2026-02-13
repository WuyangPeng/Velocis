using System.Collections.Generic;
using Celeritas.Config.game;
using Game.Scripts.Main.Runtime.GameModule.Item;
using Game.Scripts.Main.Runtime.UIItem.UICreate;
using Game.Scripts.Main.Runtime.UIObject.UICreate;
using GameFramework.ObjectPool;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.UIDisplay.UICreate
{
    public class AvatarScrollDisplay : ScrollDisplayBase
    {
        private const int PerRow = 4;

        [SerializeField] private AvatarItem itemPrefab;

        [SerializeField] private int poolCapacity = 20;

        private readonly List<AvatarItemObject> activeAvatarItemObject = new();
        private readonly List<avatar_config> holdAvatarConfig = new();
        private readonly List<avatar_config> notUnlockedAvatarConfig = new();
        private readonly List<GameObject> rowGameObjects = new();

        private IObjectPool<AvatarItemObject> pool;
        private int selectedIndex = -1;

        private void Start()
        {
            const string poolName = "AvatarItemPool";
            pool = GameEntry.ObjectPool.HasObjectPool<AvatarItemObject>(poolName) ? GameEntry.ObjectPool.GetObjectPool<AvatarItemObject>(poolName) : GameEntry.ObjectPool.CreateSingleSpawnObjectPool<AvatarItemObject>(poolName, poolCapacity, 30f, 16);

            Refresh();
        }

        private void SetAvatarData()
        {
            holdAvatarConfig.Clear();
            selectedIndex = -1;

            var avatarModule = GameEntry.ModuleComponent.GetModule<AvatarModule>();
            var selectedAvatar = avatarModule.GetSelectedAvatar();
            var index = 0;
            foreach (var avatarConfig in GameEntry.GameConfig.GetGameConfig().GetTables().AvatarConfigContainer.DataList)
            {
                var item = avatarModule.GetItem(avatarConfig.ItemTemplateId);
                if (item != null)
                {
                    holdAvatarConfig.Add(avatarConfig);
                    if (selectedAvatar != null && selectedAvatar.Inventory.ItemId == item.Inventory.ItemId)
                    {
                        selectedIndex = index;
                    }

                    ++index;
                }
                else if (!avatarConfig.Hidden)
                {
                    notUnlockedAvatarConfig.Add(avatarConfig);
                }
            }

            if (selectedIndex >= 0)
            {
                return;
            }

            selectedIndex = 0;
        }

        public void Refresh()
        {
            SetAvatarData();
            UnSpawnAvatar();
            SpawnAvatar();
        }

        private void SpawnAvatar()
        {
            var rowCount = Mathf.CeilToInt((float)(holdAvatarConfig.Count + notUnlockedAvatarConfig.Count) / PerRow);

            for (var row = 0; row < rowCount; row++)
            {
                if (!SpawnAvatar(row))
                {
                    return;
                }
            }
        }

        private bool SpawnAvatar(int row)
        {
            var rowGameObject = GetRowGameObject(row);

            rowGameObjects.Add(rowGameObject);

            for (var column = 0; column < PerRow; column++)
            {
                if (!SpawnAvatar(row, column, rowGameObject))
                {
                    return false;
                }
            }

            return true;
        }

        private bool SpawnAvatar(int row, int column, GameObject rowGameObject)
        {
            var idx = row * PerRow + column;
            if (idx >= holdAvatarConfig.Count + notUnlockedAvatarConfig.Count)
            {
                return true;
            }

            var spawn = GetSpawn();
            if (spawn == null)
            {
                return false;
            }

            activeAvatarItemObject.Add(spawn);

            var avatarItem = (AvatarItem)spawn.Target;
            avatarItem.transform.SetParent(rowGameObject.transform, false);
            
            var isUnlocked = idx < holdAvatarConfig.Count;
            
            if (isUnlocked)
            {
                avatarItem.SetData(idx, holdAvatarConfig[idx], OnItemClick);
                avatarItem.SetGrayscale(false);
            }
            else
            {
                avatarItem.SetData(idx, notUnlockedAvatarConfig[idx - holdAvatarConfig.Count], OnItemClick);
                avatarItem.SetGrayscale(true);
            }
         
            avatarItem.SetSelected(idx == selectedIndex);

            return true;
        }

        private void UnSpawnAvatar()
        {
            foreach (var obj in activeAvatarItemObject)
            {
                var item = (AvatarItem)obj.Target;
                if (item != null && item.gameObject != null)
                {
                    item.transform.SetParent(null, false);
                }

                pool.Unspawn(obj);
            }

            activeAvatarItemObject.Clear();

            foreach (var rowGameObject in rowGameObjects)
            {
                DestroyImmediate(rowGameObject);
            }

            rowGameObjects.Clear();
        }

        private AvatarItemObject GetSpawn()
        {
            var result = pool.Spawn();
            if (result != null)
            {
                return result;
            }

            var itemGameObject = Instantiate(itemPrefab.gameObject, null);
            if (itemGameObject.TryGetComponent<AvatarItem>(out var item))
            {
                var avatarItemObject = AvatarItemObject.Create(item);
                pool.Register(avatarItemObject, true);
                pool.Unspawn(avatarItemObject);
                result = pool.Spawn();

                return result;
            }

            Log.Error("预制体没挂 AvatarItem");
            Destroy(itemGameObject);
            return null;
        }

        private void OnItemClick(int index)
        {
            selectedIndex = index;

            for (var i = 0; i < activeAvatarItemObject.Count; i++)
            {
                var avatarItem = (AvatarItem)activeAvatarItemObject[i].Target;
                avatarItem.SetSelected(i == selectedIndex);
            }
        }
    }
}