using GameFramework;
using GameFramework.ObjectPool;
using UnityEngine;

namespace Game.Scripts.Main.Runtime.HPBar
{
    public class HpBarItemObject : ObjectBase
    {
        public static HpBarItemObject Create(object target)
        {
            var hpBarItemObject = ReferencePool.Acquire<HpBarItemObject>();
            hpBarItemObject.Initialize(target);
            return hpBarItemObject;
        }

        protected override void Release(bool isShutdown)
        {
            var hpBarItem = (HpBarItem)Target;
            if (hpBarItem == null)
            {
                return;
            }

            Object.Destroy(hpBarItem.gameObject);
        }
    }
}
