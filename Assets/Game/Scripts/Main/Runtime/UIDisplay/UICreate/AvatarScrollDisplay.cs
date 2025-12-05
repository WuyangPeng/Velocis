using System.Collections.Generic;
using Game.Scripts.Main.Runtime.DataTable;
using Game.Scripts.Main.Runtime.GameModule.User;
using Game.Scripts.Main.Runtime.UIItem.UICreate;
using Game.Scripts.Main.Runtime.UIObject.UICreate;
using GameFramework.ObjectPool;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.UIDisplay.UICreate
{
    public class AvatarScrollDisplay : ScrollDisplayBase
    {
        [SerializeField]
        private AvatarItem itemPrefab;

        [SerializeField]
        private int poolCapacity = 20;

        private IObjectPool<AvatarItemObject> pool;
        private readonly List<DRAvatar> avatarData = new();
        private int selectedIndex = -1;
        private readonly List<AvatarItemObject> activeAvatarItemObject = new();
        private const int PerRow = 4;
        private readonly List<GameObject> rowGameObjects = new();

        private void Start()
        {
            const string poolName = "AvatarItemPool";
            pool = GameEntry.ObjectPool.HasObjectPool<AvatarItemObject>(poolName) ?
                GameEntry.ObjectPool.GetObjectPool<AvatarItemObject>(poolName) :
                GameEntry.ObjectPool.CreateSingleSpawnObjectPool<AvatarItemObject>(poolName, poolCapacity, 30f, 16);

            Refresh();
        }

        private void SetAvatarData()
        {
            avatarData.Clear();
            selectedIndex = -1;

            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            var avatarId = userModule.GetAvatarId();
            var sexType = userModule.GetSexType();
            var avatars = GameEntry.DataTable.GetDataTable<DRAvatar>();
            foreach (var avatar in avatars)
            {
                if ((avatar.Sex & (int)sexType) == 0) continue;

                avatarData.Add(avatar);
                if (avatarId == avatar.Id)
                {
                    selectedIndex = avatarData.Count - 1;
                }
            }

            if (selectedIndex >= 0)
            {
                return;
            }

            selectedIndex = 0;
            userModule.SetAvatarId(avatarData[0].Id);
        }

        public void Refresh()
        {
            SetAvatarData();
            UnSpawnAvatar();
            SpawnAvatar();
        }

        private void SpawnAvatar()
        {
            var rowCount = Mathf.CeilToInt((float)avatarData.Count / PerRow);

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
            if (idx >= avatarData.Count)
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
            avatarItem.SetData(idx, avatarData[idx], OnItemClick);
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
            if (result != null) return result;

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
                var avatarItem = (AvatarItem)(activeAvatarItemObject[i].Target);
                avatarItem.SetSelected(i == selectedIndex);
            }

            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            userModule.SetAvatarId(avatarData[index].Id);
        }

    }
}