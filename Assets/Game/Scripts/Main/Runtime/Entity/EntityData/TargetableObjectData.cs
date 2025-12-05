using System;
using Game.Scripts.Main.Runtime.Definition.Enum;
using UnityEngine;

namespace Game.Scripts.Main.Runtime.Entity.EntityData
{
    [Serializable]
    public abstract class TargetableObjectData : EntityData
    {
        [SerializeField]
        private CampType camp;

        [SerializeField]
        private int hp;

        protected TargetableObjectData(int entityId, int typeId, CampType camp)
            : base(entityId, typeId)
        {
            this.camp = camp;
            hp = 0;
        }

        /// <summary>
        /// 角色阵营。
        /// </summary>
        public CampType Camp => camp;

        /// <summary>
        /// 当前生命。
        /// </summary>
        public int Hp
        {
            get => hp;
            set => hp = value;
        }

        /// <summary>
        /// 最大生命。
        /// </summary>
        public abstract int MaxHp
        {
            get;
        }

        /// <summary>
        /// 生命百分比。
        /// </summary>
        public float HpRatio => MaxHp > 0 ? (float)Hp / MaxHp : 0f;
    }
}
