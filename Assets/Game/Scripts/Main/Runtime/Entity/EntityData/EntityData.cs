using System;
using UnityEngine;

namespace Game.Scripts.Main.Runtime.Entity.EntityData
{
    [Serializable]
    public abstract class EntityData
    {
        [SerializeField]
        private int id;

        [SerializeField]
        private int typeId;

        [SerializeField]
        private Vector3 position = Vector3.zero;

        [SerializeField]
        private Quaternion rotation = Quaternion.identity;

        protected EntityData(int entityId, int typeId)
        {
            id = entityId;
            this.typeId = typeId;
        }

        /// <summary>
        /// 实体编号。
        /// </summary>
        public int Id => id;

        /// <summary>
        /// 实体类型编号。
        /// </summary>
        public int TypeId => typeId;

        /// <summary>
        /// 实体位置。
        /// </summary>
        public Vector3 Position
        {
            get => position;
            set => position = value;
        }

        /// <summary>
        /// 实体朝向。
        /// </summary>
        public Quaternion Rotation
        {
            get => rotation;
            set => rotation = value;
        }
    }
}
