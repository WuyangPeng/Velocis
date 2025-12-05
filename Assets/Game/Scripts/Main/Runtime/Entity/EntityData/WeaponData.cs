using System;
using Game.Scripts.Main.Runtime.DataTable;
using Game.Scripts.Main.Runtime.Definition.Enum;
using UnityEngine;

namespace Game.Scripts.Main.Runtime.Entity.EntityData
{
    [Serializable]
    public class WeaponData : AccessoryObjectData
    {
        [SerializeField]
        private int attack;

        [SerializeField]
        private float attackInterval;

        [SerializeField]
        private int bulletId;

        [SerializeField]
        private float bulletSpeed;

        [SerializeField]
        private int bulletSoundId;

        public WeaponData(int entityId, int typeId, int ownerId, CampType ownerCamp)
            : base(entityId, typeId, ownerId, ownerCamp)
        {
            var dtWeapon = Base.GameEntry.DataTable.GetDataTable<DRWeapon>();
            var drWeapon = dtWeapon.GetDataRow(TypeId);
            if (drWeapon == null)
            {
                return;
            }

            attack = drWeapon.Attack;
            attackInterval = drWeapon.AttackInterval;
            bulletId = drWeapon.BulletId;
            bulletSpeed = drWeapon.BulletSpeed;
            bulletSoundId = drWeapon.BulletSoundId;
        }

        /// <summary>
        /// 攻击力。
        /// </summary>
        public int Attack => attack;

        /// <summary>
        /// 攻击间隔。
        /// </summary>
        public float AttackInterval => attackInterval;

        /// <summary>
        /// 子弹编号。
        /// </summary>
        public int BulletId => bulletId;

        /// <summary>
        /// 子弹速度。
        /// </summary>
        public float BulletSpeed => bulletSpeed;

        /// <summary>
        /// 子弹声音编号。
        /// </summary>
        public int BulletSoundId => bulletSoundId;
    }
}
