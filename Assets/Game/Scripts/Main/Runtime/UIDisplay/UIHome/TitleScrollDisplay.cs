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
    public class TitleScrollDisplay : ScrollDisplayBase
    {
        private const int PerRow = 4;

        [SerializeField] private TitleItem itemPrefab;
        [SerializeField] private int poolCapacity = 20;

        private readonly List<TitleItemObject> activeTitleItemObject = new();
        private readonly List<title_config> holdTitleConfig = new();
        private readonly List<title_config> notUnlockedTitleConfig = new();
        private readonly List<GameObject> rowGameObjects = new();

        private IObjectPool<TitleItemObject> pool;
        private int selectedIndex = -1;

        private void Start()
        {
            const string poolName = "TitleItemPool";
            pool = GameEntry.ObjectPool.HasObjectPool<TitleItemObject>(poolName) 
                ? GameEntry.ObjectPool.GetObjectPool<TitleItemObject>(poolName) 
                : GameEntry.ObjectPool.CreateSingleSpawnObjectPool<TitleItemObject>(poolName, poolCapacity, 30f, 16);

            Refresh();
        }

        private void SetTitleData()
        {
            holdTitleConfig.Clear();
            selectedIndex = -1;

            var titleModule = GameEntry.ModuleComponent.GetModule<TitleModule>();
            var selectedTitle = titleModule.GetSelectedTitle();
            var index = 0;
            
            foreach (var titleConfig in GameEntry.GameConfig.GetGameConfig().GetTables().TitleConfigContainer.DataList)
            {
                var item = titleModule.GetItem(titleConfig.ItemTemplateId);
                if (item != null)
                {
                    holdTitleConfig.Add(titleConfig);
                    if (selectedTitle != null && selectedTitle.Inventory.ItemId == item.Inventory.ItemId)
                    {
                        selectedIndex = index;
                    }
                    ++index;
                }
                else if (!titleConfig.Hidden)
                {
                    notUnlockedTitleConfig.Add(titleConfig);
                }
            }

            if (selectedIndex < 0)
            {
                selectedIndex = 0;
            }
        }

        public void Refresh()
        {
            SetTitleData();
            UnSpawnTitle();
            SpawnTitle();
        }

        private void SpawnTitle()
        {
            var rowCount = Mathf.CeilToInt((float)(holdTitleConfig.Count + notUnlockedTitleConfig.Count) / PerRow);

            for (var row = 0; row < rowCount; row++)
            {
                if (!SpawnTitle(row))
                {
                    return;
                }
            }
        }

        private bool SpawnTitle(int row)
        {
            var rowGameObject = GetRowGameObject(row);
            rowGameObjects.Add(rowGameObject);

            for (var column = 0; column < PerRow; column++)
            {
                if (!SpawnTitle(row, column, rowGameObject))
                {
                    return false;
                }
            }

            return true;
        }

        private bool SpawnTitle(int row, int column, GameObject rowGameObject)
        {
            var idx = row * PerRow + column;
            if (idx >= holdTitleConfig.Count + notUnlockedTitleConfig.Count)
            {
                return true;
            }

            var spawn = GetSpawn();
            if (spawn == null)
            {
                return false;
            }

            activeTitleItemObject.Add(spawn);

            var titleItem = (TitleItem)spawn.Target;
            titleItem.transform.SetParent(rowGameObject.transform, false);
            
            var isUnlocked = idx < holdTitleConfig.Count;
            
            if (isUnlocked)
            {
                titleItem.SetData(idx, holdTitleConfig[idx], OnItemClick);
                titleItem.SetGrayscale(false);
            }
            else
            {
                titleItem.SetData(idx, notUnlockedTitleConfig[idx - holdTitleConfig.Count], OnItemClick);
                titleItem.SetGrayscale(true);
            }
         
            titleItem.SetSelected(idx == selectedIndex);

            return true;
        }

        private void UnSpawnTitle()
        {
            foreach (var obj in activeTitleItemObject)
            {
                var item = (TitleItem)obj.Target;
                if (item != null && item.gameObject != null)
                {
                    item.transform.SetParent(null, false);
                }

                pool.Unspawn(obj);
            }

            activeTitleItemObject.Clear();

            foreach (var rowGameObject in rowGameObjects)
            {
                DestroyImmediate(rowGameObject);
            }

            rowGameObjects.Clear();
        }

        private TitleItemObject GetSpawn()
        {
            var result = pool.Spawn();
            if (result != null)
            {
                return result;
            }

            var itemGameObject = Instantiate(itemPrefab.gameObject, null);
            if (itemGameObject.TryGetComponent<TitleItem>(out var item))
            {
                var titleItemObject = TitleItemObject.Create(item);
                pool.Register(titleItemObject, true);
                pool.Unspawn(titleItemObject);
                result = pool.Spawn();

                return result;
            }

            Log.Error("预制体没挂 TitleItem");
            Destroy(itemGameObject);
            return null;
        }

        private void OnItemClick(int index)
        {
            selectedIndex = index;

            for (var i = 0; i < activeTitleItemObject.Count; i++)
            {
                var titleItem = (TitleItem)activeTitleItemObject[i].Target;
                titleItem.SetSelected(i == selectedIndex);
            }
        }
    }
}
