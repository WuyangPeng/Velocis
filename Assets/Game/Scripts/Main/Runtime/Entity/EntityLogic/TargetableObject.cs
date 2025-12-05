using Game.Scripts.Main.Runtime.Definition.DataStruct;
using Game.Scripts.Main.Runtime.Entity.EntityData;
using Game.Scripts.Main.Runtime.GameUtility;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.Entity.EntityLogic
{
    /// <summary>
    /// 可作为目标的实体类。
    /// </summary>
    public abstract class TargetableObject : Entity
    {
        [SerializeField]
        private TargetableObjectData targetableObjectData;

        public bool IsDead => targetableObjectData.Hp <= 0;

        public abstract ImpactData GetImpactData();

        public void ApplyDamage(Entity attacker, int damageHp)
        {
            var fromHpRatio = targetableObjectData.HpRatio;
            targetableObjectData.Hp -= damageHp;
            var toHpRatio = targetableObjectData.HpRatio;
            if (fromHpRatio > toHpRatio)
            {
                Base.GameEntry.HpBar.ShowHpBar(this, fromHpRatio, toHpRatio);
            }

            if (targetableObjectData.Hp <= 0)
            {
                OnDead(attacker);
            }
        }


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            gameObject.SetLayerRecursively(Definition.Constant.Constant.Layer.TargetableObjectLayerId);
        }


        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            targetableObjectData = userData as TargetableObjectData;
            if (targetableObjectData != null) return;
            Log.Error("Targetable object data is invalid.");
        }

        protected virtual void OnDead(Entity attacker)
        {
            Base.GameEntry.Entity.HideEntity(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.TryGetComponent<Entity>(out var entity))
            {
                return;
            }

            if (entity is TargetableObject && entity.Id >= Id)
            {
                // 碰撞事件由 Id 小的一方处理，避免重复处理
                return;
            }

            AIUtility.PerformCollision(this, entity);
        }
    }
}
