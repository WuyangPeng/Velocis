using System.Collections.Generic;
using Game.Scripts.Main.Runtime.UIItem.UIHome;
using Game.Scripts.Main.Runtime.UIObject.UICreate;
using GameFramework.ObjectPool;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.UIDisplay.UIHome
{
    public class AvatarScrollDisplay : ScrollDisplayBase
    {
        private const int PerRow = 4;

        [SerializeField] private AvatarItem itemPrefab;

        [SerializeField] private int poolCapacity = 20;

        private readonly List<AvatarItemObject> _activeAvatarItemObject = new();

        // private readonly List<avatar_config> _holdAvatarConfig = new();
        // private readonly List<avatar_config> _notUnlockedAvatarConfig = new();
        private readonly List<GameObject> _rowGameObjects = new();

        private IObjectPool<AvatarItemObject> _pool;
        private int _selectedIndex = -1;

        private void Start()
        {
            const string poolName = "AvatarItemPool";
            _pool = GameEntry.ObjectPool.HasObjectPool<AvatarItemObject>(poolName) ? GameEntry.ObjectPool.GetObjectPool<AvatarItemObject>(poolName) : GameEntry.ObjectPool.CreateSingleSpawnObjectPool<AvatarItemObject>(poolName, poolCapacity, 30f, 16);

            Refresh();
        }

        private void SetAvatarData()
        {
            /*  _holdAvatarConfig.Clear();
              _selectedIndex = -1;

              var avatarModule = GameEntry.ModuleComponent.GetModule<AvatarModule>();
              var selectedAvatar = avatarModule.GetSelectedAvatar();
              var index = 0;
              foreach (var avatarConfig in GameEntry.GameConfig.GetGameConfig().GetTables().AvatarConfigContainer.DataList)
              {
                  var item = avatarModule.GetItem(avatarConfig.ItemTemplateId);
                  if (item != null)
                  {
                      _holdAvatarConfig.Add(avatarConfig);
                      if (selectedAvatar != null && selectedAvatar.Inventory.ItemId == item.Inventory.ItemId)
                      {
                          _selectedIndex = index;
                      }

                      ++index;
                  }
                  else if (!avatarConfig.Hidden)
                  {
                      _notUnlockedAvatarConfig.Add(avatarConfig);
                  }
              }

              if (_selectedIndex >= 0)
              {
                  return;
              }

              _selectedIndex = 0;*/
        }

        public void Refresh()
        {
            SetAvatarData();
            UnSpawnAvatar();
            SpawnAvatar();
        }

        private void SpawnAvatar()
        {
            /*  var rowCount = Mathf.CeilToInt((float)(_holdAvatarConfig.Count + _notUnlockedAvatarConfig.Count) / PerRow);

              for (var row = 0; row < rowCount; row++)
              {
                  if (!SpawnAvatar(row))
                  {
                      return;
                  }
              }*/
        }

        private bool SpawnAvatar(int row)
        {
            var rowGameObject = GetRowGameObject(row);

            _rowGameObjects.Add(rowGameObject);

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
            /* var idx = row * PerRow + column;
             if (idx >= _holdAvatarConfig.Count + _notUnlockedAvatarConfig.Count)
             {
                 return true;
             }

             var spawn = GetSpawn();
             if (spawn == null)
             {
                 return false;
             }

             _activeAvatarItemObject.Add(spawn);

             var avatarItem = (AvatarItem)spawn.Target;
             avatarItem.transform.SetParent(rowGameObject.transform, false);

             var isUnlocked = idx < _holdAvatarConfig.Count;

             if (isUnlocked)
             {
                 avatarItem.SetData(idx, _holdAvatarConfig[idx], OnItemClick);
                 avatarItem.SetGrayscale(false);
             }
             else
             {
                 avatarItem.SetData(idx, _notUnlockedAvatarConfig[idx - _holdAvatarConfig.Count], OnItemClick);
                 avatarItem.SetGrayscale(true);
             }

             avatarItem.SetSelected(idx == _selectedIndex);
 */
            return true;
        }

        private void UnSpawnAvatar()
        {
            foreach (var obj in _activeAvatarItemObject)
            {
                var item = (AvatarItem)obj.Target;
                if (item != null && item.gameObject != null)
                {
                    item.transform.SetParent(null, false);
                }

                _pool.Unspawn(obj);
            }

            _activeAvatarItemObject.Clear();

            foreach (var rowGameObject in _rowGameObjects)
            {
                DestroyImmediate(rowGameObject);
            }

            _rowGameObjects.Clear();
        }

        private AvatarItemObject GetSpawn()
        {
            var result = _pool.Spawn();
            if (result != null)
            {
                return result;
            }

            var itemGameObject = Instantiate(itemPrefab.gameObject, null);
            if (itemGameObject.TryGetComponent<AvatarItem>(out var item))
            {
                var avatarItemObject = AvatarItemObject.Create(item);
                _pool.Register(avatarItemObject, true);
                _pool.Unspawn(avatarItemObject);
                result = _pool.Spawn();

                return result;
            }

            Log.Error("预制体没挂 AvatarItem");
            Destroy(itemGameObject);
            return null;
        }

        private void OnItemClick(int index)
        {
            _selectedIndex = index;

            for (var i = 0; i < _activeAvatarItemObject.Count; i++)
            {
                var avatarItem = (AvatarItem)_activeAvatarItemObject[i].Target;
                avatarItem.SetSelected(i == _selectedIndex);
            }
        }
    }
}