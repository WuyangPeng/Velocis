using System;
using Game.Scripts.Main.Runtime.Definition.Enum;
using UnityEngine;

namespace Game.Scripts.Main.Runtime.Entity.EntityData
{
    [Serializable]
    public class MyAircraftData : AircraftData
    {
        [SerializeField]
        private string name;

        public MyAircraftData(int entityId, int typeId)
            : base(entityId, typeId, CampType.Player)
        {
        }

        /// <summary>
        /// 角色名称。
        /// </summary>
        public string Name
        {
            get => name;
            set => name = value;
        }
    }
}
