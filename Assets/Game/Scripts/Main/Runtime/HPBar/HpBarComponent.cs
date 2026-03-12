using System.Collections.Generic;
using System.Linq;
using GameFramework.ObjectPool;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.HPBar
{
    public class HpBarComponent : GameFrameworkComponent
    {
        [SerializeField] private HpBarItem hpBarItemTemplate;

        [SerializeField] private Transform hpBarInstanceRoot;

        [SerializeField] private int instancePoolCapacity = 16;

        private List<HpBarItem> _activeHpBarItems;
        private Canvas _cachedCanvas;

        private IObjectPool<HpBarItemObject> _hpBarItemObjectPool;

        private void Start()
        {
            if (hpBarInstanceRoot == null)
            {
                Log.Error("You must set HP bar instance root first.");
                return;
            }

            _cachedCanvas = hpBarInstanceRoot.GetComponent<Canvas>();
            _hpBarItemObjectPool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<HpBarItemObject>("HPBarItem", instancePoolCapacity);
            _activeHpBarItems = new List<HpBarItem>();
        }

        private void Update()
        {
            for (var i = _activeHpBarItems.Count - 1; i >= 0; i--)
            {
                var hpBarItem = _activeHpBarItems[i];
                if (hpBarItem.Refresh())
                {
                    continue;
                }

                HideHpBar(hpBarItem);
            }
        }

        public void ShowHpBar(Entity.EntityLogic.Entity entity, float fromHpRatio, float toHpRatio)
        {
            if (entity == null)
            {
                Log.Warning("Entity is invalid.");
                return;
            }

            var hpBarItem = GetActiveHpBarItem(entity);
            if (hpBarItem == null)
            {
                hpBarItem = CreateHpBarItem(entity);
                _activeHpBarItems.Add(hpBarItem);
            }

            hpBarItem.Init(entity, _cachedCanvas, fromHpRatio, toHpRatio);
        }

        private void HideHpBar(HpBarItem hpBarItem)
        {
            hpBarItem.Reset();
            _activeHpBarItems.Remove(hpBarItem);
            _hpBarItemObjectPool.Unspawn(hpBarItem);
        }

        private HpBarItem GetActiveHpBarItem(Entity.EntityLogic.Entity entity)
        {
            return entity == null ? null : _activeHpBarItems.FirstOrDefault(item => item.Owner == entity);
        }

        private HpBarItem CreateHpBarItem(Entity.EntityLogic.Entity entity)
        {
            HpBarItem hpBarItem;
            var hpBarItemObject = _hpBarItemObjectPool.Spawn();
            if (hpBarItemObject != null)
            {
                hpBarItem = (HpBarItem)hpBarItemObject.Target;
            }
            else
            {
                hpBarItem = Instantiate(hpBarItemTemplate);
                var itemTransform = hpBarItem.GetComponent<Transform>();
                itemTransform.SetParent(hpBarInstanceRoot);
                itemTransform.localScale = Vector3.one;
                _hpBarItemObjectPool.Register(HpBarItemObject.Create(hpBarItem), true);
            }

            return hpBarItem;
        }
    }
}