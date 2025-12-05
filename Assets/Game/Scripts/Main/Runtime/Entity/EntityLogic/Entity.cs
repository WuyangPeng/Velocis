using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.Entity.EntityLogic
{
    public abstract class Entity : Plugins.GameFramework.Scripts.Runtime.Entity.EntityLogic
    {
        [SerializeField]
        private EntityData.EntityData entityData;

        public int Id => Entity.Id;

        public Animation CachedAnimation
        {
            get;
            private set;
        }


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            CachedAnimation = GetComponent<Animation>();
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            entityData = userData as EntityData.EntityData;
            if (entityData == null)
            {
                Log.Error("Entity data is invalid.");
                return;
            }

            Name = Utility.Text.Format("[Entity {0}]", Id);
            CachedTransform.SetLocalPositionAndRotation(entityData.Position, entityData.Rotation);
            CachedTransform.localScale = Vector3.one;
        }

    }
}
