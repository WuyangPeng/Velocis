using Game.Scripts.Main.Runtime.UIItem;
using GameFramework;
using GameFramework.ObjectPool;
using UnityEngine;

namespace Game.Scripts.Main.Runtime.UIObject
{
    public abstract class ItemObjectBase<TItem> : ObjectBase where TItem : ItemBase
    {
        protected static TOwner Create<TOwner>(TItem item) where TOwner : ItemObjectBase<TItem>, new()
        {
            var owner = ReferencePool.Acquire<TOwner>();
            owner.Initialize(item);
            return owner;
        }

        protected override void OnSpawn()
        {
            ((TItem)Target).gameObject.SetActive(true);
        }


        protected override void OnUnspawn()
        {
            ((TItem)Target).gameObject.SetActive(false);
        }

        protected override void Release(bool isShutdown)
        {
            if (Target is not TItem item || item == null)
            {
                return;
            }

            item.OnRecycle();
            Object.Destroy(item.gameObject);
        }
    }
}