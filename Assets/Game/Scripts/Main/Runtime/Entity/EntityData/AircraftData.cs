using System;
using System.Collections.Generic;
using Game.Scripts.Main.Runtime.DataTable;
using Game.Scripts.Main.Runtime.Definition.Enum;
using UnityEngine;

namespace Game.Scripts.Main.Runtime.Entity.EntityData
{
    [Serializable]
    public abstract class AircraftData : TargetableObjectData
    {
        [SerializeField]
        private ThrusterData thrusterData;

        [SerializeField]
        private List<WeaponData> weaponDatas = new();

        [SerializeField]
        private List<ArmorData> armorDatas = new();

        [SerializeField]
        private int maxHp;

        [SerializeField]
        private int defense;

        [SerializeField]
        private int deadEffectId;

        [SerializeField]
        private int deadSoundId;

        protected AircraftData(int entityId, int typeId, CampType camp)
            : base(entityId, typeId, camp)
        {
            var dtAircraft = Base.GameEntry.DataTable.GetDataTable<DRAircraft>();
            var drAircraft = dtAircraft.GetDataRow(TypeId);
            if (drAircraft == null)
            {
                return;
            }

            thrusterData = new ThrusterData(Base.GameEntry.Entity.GenerateSerialId(), drAircraft.ThrusterId, Id, Camp);

            for (int index = 0, weaponId = 0; (weaponId = drAircraft.GetWeaponIdAt(index)) > 0; index++)
            {
                AttachWeaponData(new WeaponData(Base.GameEntry.Entity.GenerateSerialId(), weaponId, Id, Camp));
            }

            for (int index = 0, armorId = 0; (armorId = drAircraft.GetArmorIdAt(index)) > 0; index++)
            {
                AttachArmorData(new ArmorData(Base.GameEntry.Entity.GenerateSerialId(), armorId, Id, Camp));
            }

            deadEffectId = drAircraft.DeadEffectId;
            deadSoundId = drAircraft.DeadSoundId;

            Hp = maxHp;
        }

        /// <summary>
        /// 最大生命。
        /// </summary>
        public override int MaxHp => maxHp;

        /// <summary>
        /// 防御。
        /// </summary>
        public int Defense => defense;

        /// <summary>
        /// 速度。
        /// </summary>
        public float Speed => thrusterData.Speed;

        public int DeadEffectId => deadEffectId;

        public int DeadSoundId => deadSoundId;

        public ThrusterData GetThrusterData()
        {
            return thrusterData;
        }

        public List<WeaponData> GetAllWeaponDatas()
        {
            return weaponDatas;
        }

        public void AttachWeaponData(WeaponData weaponData)
        {
            if (weaponData == null)
            {
                return;
            }

            if (weaponDatas.Contains(weaponData))
            {
                return;
            }

            weaponDatas.Add(weaponData);
        }

        public void DetachWeaponData(WeaponData weaponData)
        {
            if (weaponData == null)
            {
                return;
            }

            weaponDatas.Remove(weaponData);
        }

        public List<ArmorData> GetAllArmorDatas()
        {
            return armorDatas;
        }

        public void AttachArmorData(ArmorData armorData)
        {
            if (armorData == null)
            {
                return;
            }

            if (armorDatas.Contains(armorData))
            {
                return;
            }

            armorDatas.Add(armorData);
            RefreshData();
        }

        public void DetachArmorData(ArmorData armorData)
        {
            if (armorData == null)
            {
                return;
            }

            armorDatas.Remove(armorData);
            RefreshData();
        }

        private void RefreshData()
        {
            maxHp = 0;
            defense = 0;
            foreach (var data in armorDatas)
            {
                maxHp += data.MaxHp;
                defense += data.Defense;
            }

            if (Hp > maxHp)
            {
                Hp = maxHp;
            }
        }
    }
}
