using Game.Scripts.Main.Runtime.UIItem;
using GameFramework;
using GameFramework.ObjectPool;
using UnityEngine;

namespace Game.Scripts.Main.Runtime.UIObject
{
    public abstract class ItemObjectBase<Item> : ObjectBase where Item : ItemBase
    {
        public static Owner Create<Owner>(Item item) where Owner : ItemObjectBase<Item>, new()
        {
            var owner = ReferencePool.Acquire<Owner>();
            owner.Initialize(item);
            return owner;
        }

        protected override void OnSpawn()
        {
            ((Item)Target).gameObject.SetActive(true);
        }


        protected override void OnUnspawn()
        {
            ((Item)Target).gameObject.SetActive(false);
        }

        protected override void Release(bool isShutdown)
        {
            if (Target is not Item item || item == null)
            {
                return;
            }

            item.OnRecycle();
            Object.Destroy(item.gameObject);
        }
    }
}