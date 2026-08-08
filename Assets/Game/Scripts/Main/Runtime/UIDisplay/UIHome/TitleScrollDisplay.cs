using System.Collections.Generic;
using Game.Scripts.Main.Runtime.UIItem.UIHome;
using Game.Scripts.Main.Runtime.UIObject.UICreate;
using GameFramework.ObjectPool;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.UIDisplay.UIHome
{
    public class TitleScrollDisplay : ScrollDisplayBase
    {
        private const int PerRow = 4;

        [SerializeField] private TitleItem itemPrefab;
        [SerializeField] private int poolCapacity = 20;

        private readonly List<TitleItemObject> _activeTitleItemObject = new();

        //   private readonly List<title_config> _holdTitleConfig = new();
        //  private readonly List<title_config> _notUnlockedTitleConfig = new();
        private readonly List<GameObject> _rowGameObjects = new();

        private IObjectPool<TitleItemObject> _pool;
        private int _selectedIndex = -1;

        private void Start()
        {
            const string poolName = "TitleItemPool";
            _pool = GameEntry.ObjectPool.HasObjectPool<TitleItemObject>(poolName)
                ? GameEntry.ObjectPool.GetObjectPool<TitleItemObject>(poolName)
                : GameEntry.ObjectPool.CreateSingleSpawnObjectPool<TitleItemObject>(poolName, poolCapacity, 30f, 16);

            Refresh();
        }

        private void SetTitleData()
        {
            /*   _holdTitleConfig.Clear();
               _selectedIndex = -1;

               var titleModule = GameEntry.ModuleComponent.GetModule<TitleModule>();
               var selectedTitle = titleModule.GetSelectedTitle();
               var index = 0;

//                foreach (var titleConfig in GameEntry.GameConfig.GetGameConfig().GetTables().TitleConfigContainer.DataList)
//                {
//                    var item = titleModule.GetItem(titleConfig.ItemTemplateId);
//                    if (item != null)
//                    {
//                        _holdTitleConfig.Add(titleConfig);
//                        if (selectedTitle != null && selectedTitle.Inventory.ItemId == item.Inventory.ItemId)
//                        {
//                            _selectedIndex = index;
//                        }
//
//                        index++;
//                    }
//                }   else if (!titleConfig.Hidden)
                   {
                       _notUnlockedTitleConfig.Add(titleConfig);
                   }
               }

               if (_selectedIndex < 0)
               {
                   _selectedIndex = 0;
               }*/
        }

        public void Refresh()
        {
            SetTitleData();
            UnSpawnTitle();
            SpawnTitle();
        }

        private void SpawnTitle()
        {
            /*  var rowCount = Mathf.CeilToInt((float)(_holdTitleConfig.Count + _notUnlockedTitleConfig.Count) / PerRow);

              for (var row = 0; row < rowCount; row++)
              {
                  if (!SpawnTitle(row))
                  {
                      return;
                  }
              }*/
        }

        private bool SpawnTitle(int row)
        {
            var rowGameObject = GetRowGameObject(row);
            _rowGameObjects.Add(rowGameObject);

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
            /*    var idx = row * PerRow + column;
                if (idx >= _holdTitleConfig.Count + _notUnlockedTitleConfig.Count)
                {
                    return true;
                }

                var spawn = GetSpawn();
                if (spawn == null)
                {
                    return false;
                }

                _activeTitleItemObject.Add(spawn);

                var titleItem = (TitleItem)spawn.Target;
                titleItem.transform.SetParent(rowGameObject.transform, false);

                var isUnlocked = idx < _holdTitleConfig.Count;

                if (isUnlocked)
                {
                    titleItem.SetData(idx, _holdTitleConfig[idx], OnItemClick);
                    titleItem.SetGrayscale(false);
                }
                else
                {
                    titleItem.SetData(idx, _notUnlockedTitleConfig[idx - _holdTitleConfig.Count], OnItemClick);
                    titleItem.SetGrayscale(true);
                }

                titleItem.SetSelected(idx == _selectedIndex);*/

            return true;
        }

        private void UnSpawnTitle()
        {
            foreach (var obj in _activeTitleItemObject)
            {
                var item = (TitleItem)obj.Target;
                if (item != null && item.gameObject != null)
                {
                    item.transform.SetParent(null, false);
                }

                _pool.Unspawn(obj);
            }

            _activeTitleItemObject.Clear();

            foreach (var rowGameObject in _rowGameObjects)
            {
                DestroyImmediate(rowGameObject);
            }

            _rowGameObjects.Clear();
        }

        private TitleItemObject GetSpawn()
        {
            var result = _pool.Spawn();
            if (result != null)
            {
                return result;
            }

            var itemGameObject = Instantiate(itemPrefab.gameObject, null);
            if (itemGameObject.TryGetComponent<TitleItem>(out var item))
            {
                var titleItemObject = TitleItemObject.Create(item);
                _pool.Register(titleItemObject, true);
                _pool.Unspawn(titleItemObject);
                result = _pool.Spawn();

                return result;
            }

            Log.Error("预制体没挂 TitleItem");
            Destroy(itemGameObject);
            return null;
        }

        private void OnItemClick(int index)
        {
            _selectedIndex = index;

            for (var i = 0; i < _activeTitleItemObject.Count; i++)
            {
                var titleItem = (TitleItem)_activeTitleItemObject[i].Target;
                titleItem.SetSelected(i == _selectedIndex);
            }
        }
    }
}