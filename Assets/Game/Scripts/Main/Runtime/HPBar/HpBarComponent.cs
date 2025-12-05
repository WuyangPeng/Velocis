using System.Collections.Generic;
using System.Linq;
using GameFramework.ObjectPool;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.HPBar
{
    public class HpBarComponent : GameFrameworkComponent
    {
        [SerializeField]
        private HpBarItem hpBarItemTemplate;

        [SerializeField]
        private Transform hpBarInstanceRoot;

        [SerializeField]
        private int instancePoolCapacity = 16;

        private IObjectPool<HpBarItemObject> hpBarItemObjectPool;
        private List<HpBarItem> activeHpBarItems;
        private Canvas cachedCanvas;

        private void Start()
        {
            if (hpBarInstanceRoot == null)
            {
                Log.Error("You must set HP bar instance root first.");
                return;
            }

            cachedCanvas = hpBarInstanceRoot.GetComponent<Canvas>();
            hpBarItemObjectPool = Base.GameEntry.ObjectPool.CreateSingleSpawnObjectPool<HpBarItemObject>("HPBarItem", instancePoolCapacity);
            activeHpBarItems = new List<HpBarItem>();
        }

        private void Update()
        {
            for (var i = activeHpBarItems.Count - 1; i >= 0; i--)
            {
                var hpBarItem = activeHpBarItems[i];
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
                activeHpBarItems.Add(hpBarItem);
            }

            hpBarItem.Init(entity, cachedCanvas, fromHpRatio, toHpRatio);
        }

        private void HideHpBar(HpBarItem hpBarItem)
        {
            hpBarItem.Reset();
            activeHpBarItems.Remove(hpBarItem);
            hpBarItemObjectPool.Unspawn(hpBarItem);
        }

        private HpBarItem GetActiveHpBarItem(Entity.EntityLogic.Entity entity)
        {
            return entity == null ? null : activeHpBarItems.FirstOrDefault(item => item.Owner == entity);
        }

        private HpBarItem CreateHpBarItem(Entity.EntityLogic.Entity entity)
        {
            HpBarItem hpBarItem;
            var hpBarItemObject = hpBarItemObjectPool.Spawn();
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
                hpBarItemObjectPool.Register(HpBarItemObject.Create(hpBarItem), true);
            }

            return hpBarItem;
        }
    }
}
