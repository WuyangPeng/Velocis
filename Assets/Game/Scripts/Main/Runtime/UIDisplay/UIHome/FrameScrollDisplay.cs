using System.Collections.Generic;
using Celeritas.Config.game;
using Game.Scripts.Main.Runtime.GameModule.Item;
using Game.Scripts.Main.Runtime.UIItem.UIHome;
using Game.Scripts.Main.Runtime.UIObject.UICreate;
using GameFramework.ObjectPool;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.UIDisplay.UIHome
{
    public class FrameScrollDisplay : ScrollDisplayBase
    {
        private const int PerRow = 4;

        [SerializeField] private FrameItem itemPrefab;
        [SerializeField] private int poolCapacity = 20;

        private readonly List<FrameItemObject> _activeFrameItemObject = new();
        private readonly List<frame_config> _holdFrameConfig = new();
        private readonly List<frame_config> _notUnlockedFrameConfig = new();
        private readonly List<GameObject> _rowGameObjects = new();

        private IObjectPool<FrameItemObject> _pool;
        private int _selectedIndex = -1;

        private void Start()
        {
            const string poolName = "FrameItemPool";
            _pool = GameEntry.ObjectPool.HasObjectPool<FrameItemObject>(poolName)
                ? GameEntry.ObjectPool.GetObjectPool<FrameItemObject>(poolName)
                : GameEntry.ObjectPool.CreateSingleSpawnObjectPool<FrameItemObject>(poolName, poolCapacity, 30f, 16);

            Refresh();
        }

        private void SetFrameData()
        {
            _holdFrameConfig.Clear();
            _selectedIndex = -1;

            var frameModule = GameEntry.ModuleComponent.GetModule<FrameModule>();
            var selectedFrame = frameModule.GetSelectedFrame();
            var index = 0;

            foreach (var frameConfig in GameEntry.GameConfig.GetGameConfig().GetTables().FrameConfigContainer.DataList)
            {
                var item = frameModule.GetItem(frameConfig.ItemTemplateId);
                if (item != null)
                {
                    _holdFrameConfig.Add(frameConfig);
                    if (selectedFrame != null && selectedFrame.Inventory.ItemId == item.Inventory.ItemId)
                    {
                        _selectedIndex = index;
                    }

                    ++index;
                }
                else if (!frameConfig.Hidden)
                {
                    _notUnlockedFrameConfig.Add(frameConfig);
                }
            }

            if (_selectedIndex < 0)
            {
                _selectedIndex = 0;
            }
        }

        public void Refresh()
        {
            SetFrameData();
            UnSpawnFrame();
            SpawnFrame();
        }

        private void SpawnFrame()
        {
            var rowCount = Mathf.CeilToInt((float)(_holdFrameConfig.Count + _notUnlockedFrameConfig.Count) / PerRow);

            for (var row = 0; row < rowCount; row++)
            {
                if (!SpawnFrame(row))
                {
                    return;
                }
            }
        }

        private bool SpawnFrame(int row)
        {
            var rowGameObject = GetRowGameObject(row);
            _rowGameObjects.Add(rowGameObject);

            for (var column = 0; column < PerRow; column++)
            {
                if (!SpawnFrame(row, column, rowGameObject))
                {
                    return false;
                }
            }

            return true;
        }

        private bool SpawnFrame(int row, int column, GameObject rowGameObject)
        {
            var idx = row * PerRow + column;
            if (idx >= _holdFrameConfig.Count + _notUnlockedFrameConfig.Count)
            {
                return true;
            }

            var spawn = GetSpawn();
            if (spawn == null)
            {
                return false;
            }

            _activeFrameItemObject.Add(spawn);

            var frameItem = (FrameItem)spawn.Target;
            frameItem.transform.SetParent(rowGameObject.transform, false);

            var isUnlocked = idx < _holdFrameConfig.Count;

            if (isUnlocked)
            {
                frameItem.SetData(idx, _holdFrameConfig[idx], OnItemClick);
                frameItem.SetGrayscale(false);
            }
            else
            {
                frameItem.SetData(idx, _notUnlockedFrameConfig[idx - _holdFrameConfig.Count], OnItemClick);
                frameItem.SetGrayscale(true);
            }

            frameItem.SetSelected(idx == _selectedIndex);

            return true;
        }

        private void UnSpawnFrame()
        {
            foreach (var obj in _activeFrameItemObject)
            {
                var item = (FrameItem)obj.Target;
                if (item != null && item.gameObject != null)
                {
                    item.transform.SetParent(null, false);
                }

                _pool.Unspawn(obj);
            }

            _activeFrameItemObject.Clear();

            foreach (var rowGameObject in _rowGameObjects)
            {
                DestroyImmediate(rowGameObject);
            }

            _rowGameObjects.Clear();
        }

        private FrameItemObject GetSpawn()
        {
            var result = _pool.Spawn();
            if (result != null)
            {
                return result;
            }

            var itemGameObject = Instantiate(itemPrefab.gameObject, null);
            if (itemGameObject.TryGetComponent<FrameItem>(out var item))
            {
                var frameItemObject = FrameItemObject.Create(item);
                _pool.Register(frameItemObject, true);
                _pool.Unspawn(frameItemObject);
                result = _pool.Spawn();

                return result;
            }

            Log.Error("预制体没挂 FrameItem");
            Destroy(itemGameObject);
            return null;
        }

        private void OnItemClick(int index)
        {
            _selectedIndex = index;

            for (var i = 0; i < _activeFrameItemObject.Count; i++)
            {
                var frameItem = (FrameItem)_activeFrameItemObject[i].Target;
                frameItem.SetSelected(i == _selectedIndex);
            }
        }
    }
}