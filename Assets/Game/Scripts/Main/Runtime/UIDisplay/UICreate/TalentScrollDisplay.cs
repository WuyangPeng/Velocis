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
    public class TalentScrollDisplay : ScrollDisplayBase
    {
        [SerializeField]
        private TalentItem itemPrefab;

        [SerializeField]
        private int poolCapacity = 20;

        [SerializeField]
        private Text talentDescription;

        private IObjectPool<TalentItemObject> pool;
        private readonly List<DRTalent> talentData = new();
        private readonly List<int> selectedIndex = new();
        private readonly List<TalentItemObject> activeTalentItemObject = new();
        private const int PerRow = 2;
        private readonly List<GameObject> rowGameObjects = new();

        private void Start()
        {
            const string poolName = "TalentItemPool";
            pool = GameEntry.ObjectPool.HasObjectPool<TalentItemObject>(poolName) ?
                GameEntry.ObjectPool.GetObjectPool<TalentItemObject>(poolName) :
                GameEntry.ObjectPool.CreateSingleSpawnObjectPool<TalentItemObject>(poolName, poolCapacity, 30f, 16);

            Refresh();
        }

        public void Refresh()
        {
            SetAvatarData();
            UnSpawnAvatar();
            SpawnAvatar();
        }

        private void SetAvatarData()
        {
            talentData.Clear();
            selectedIndex.Clear();

            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            var accountModule = GameEntry.ModuleComponent.GetModule<AccountModule>();

            var talents = GameEntry.DataTable.GetDataTable<DRTalent>();
            foreach (var talent in talents)
            {
                if (!talent.DefaultEnabled && !accountModule.HasTalent(talent.Id))
                {
                    continue;
                }

                if (userModule.HasSelectedTalent(talent.Id))
                {
                    selectedIndex.Add(talentData.Count);
                }

                talentData.Add(talent);
            }
        }


        private void SpawnAvatar()
        {
            var rowCount = Mathf.CeilToInt((float)talentData.Count / PerRow);

            for (var row = 0; row < rowCount; row++)
            {
                if (!SpawnAvatar(row))
                {
                    return;
                }
            }

            talentDescription.text = "";
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
            if (idx >= talentData.Count)
            {
                return true;
            }

            var spawn = GetSpawn();
            if (spawn == null)
            {
                return false;
            }

            activeTalentItemObject.Add(spawn);

            var avatarItem = (TalentItem)spawn.Target;
            avatarItem.transform.SetParent(rowGameObject.transform, false);
            avatarItem.SetData(idx, talentData[idx], OnItemClick);
            avatarItem.SetSelected(selectedIndex.Contains(idx));

            return true;
        }

        private void UnSpawnAvatar()
        {
            foreach (var element in activeTalentItemObject)
            {
                var item = (TalentItem)element.Target;
                if (item != null && item.gameObject != null)
                {
                    item.transform.SetParent(null, false);
                }
                pool.Unspawn(element);
            }

            activeTalentItemObject.Clear();

            foreach (var rowGameObject in rowGameObjects)
            {
                DestroyImmediate(rowGameObject);
            }

            rowGameObjects.Clear();
        }

        private TalentItemObject GetSpawn()
        {
            var result = pool.Spawn();
            if (result != null) return result;

            var itemGameObject = Instantiate(itemPrefab.gameObject, null);
            if (itemGameObject.TryGetComponent<TalentItem>(out var item))
            {
                var avatarItemObject = TalentItemObject.Create(item);
                pool.Register(avatarItemObject, true);
                pool.Unspawn(avatarItemObject);
                result = pool.Spawn();

                return result;
            }

            Log.Error("预制体没挂 TalentItem");
            Destroy(itemGameObject);
            return null;
        }

        private void OnItemClick(int index)
        {
            talentDescription.text = GameEntry.Localization.GetString(talentData[index].Description);

            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            if (userModule.HasTalent(talentData[index].Id))
            {
                userModule.RemoveTalent(talentData[index].Id);
                selectedIndex.Remove(index);
                UpdateSelected();
            }
            else
            {
                if (!userModule.CanAddTalent(talentData[index].Id))
                {
                    return;
                }

                selectedIndex.Add(index);

                userModule.AddTalent(talentData[index].Id);

                UpdateSelected();
            }
        }

        private void UpdateSelected()
        {
            for (var i = 0; i < activeTalentItemObject.Count; i++)
            {
                var avatarItem = (TalentItem)(activeTalentItemObject[i].Target);
                avatarItem.SetSelected(selectedIndex.Contains(i));
            }
        }
    }
}