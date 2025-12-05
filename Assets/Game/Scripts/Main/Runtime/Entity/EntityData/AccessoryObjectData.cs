using System;
using Game.Scripts.Main.Runtime.Definition.Enum;
using UnityEngine;

namespace Game.Scripts.Main.Runtime.Entity.EntityData
{
    [Serializable]
    public abstract class AccessoryObjectData : EntityData
    {
        [SerializeField]
        private int ownerId;

        [SerializeField]
        private CampType ownerCamp;

        protected AccessoryObjectData(int entityId, int typeId, int ownerId, CampType ownerCamp)
            : base(entityId, typeId)
        {
            this.ownerId = ownerId;
            this.ownerCamp = ownerCamp;
        }

        /// <summary>
        /// 拥有者编号。
        /// </summary>
        public int OwnerId => ownerId;

        /// <summary>
        /// 拥有者阵营。
        /// </summary>
        public CampType OwnerCamp => ownerCamp;
    }
}
