using System.Collections.Generic;
using Game.Scripts.Main.Runtime.Definition.DataStruct;
using Game.Scripts.Main.Runtime.Entity.EntityData;
using Game.Scripts.Main.Runtime.Sound;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.Entity.EntityLogic
{
    /// <summary>
    /// 战机类。
    /// </summary>
    public abstract class Aircraft : TargetableObject
    {
        [SerializeField]
        private AircraftData aircraftData;

        [SerializeField]
        protected Thruster thruster;

        [SerializeField]
        protected List<Weapon> weapons = new();

        [SerializeField]
        protected List<Armor> armors = new();


        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            aircraftData = userData as AircraftData;
            if (aircraftData == null)
            {
                Log.Error("Aircraft data is invalid.");
                return;
            }

            Name = Utility.Text.Format("Aircraft ({0})", Id);

            Base.GameEntry.Entity.ShowThruster(aircraftData.GetThrusterData());

            var weaponDatas = aircraftData.GetAllWeaponDatas();
            foreach (var data in weaponDatas)
            {
                Base.GameEntry.Entity.ShowWeapon(data);
            }

            var armorDatas = aircraftData.GetAllArmorDatas();
            foreach (var data in armorDatas)
            {
                Base.GameEntry.Entity.ShowArmor(data);
            }
        }


        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
        }


        protected override void OnAttached(Plugins.GameFramework.Scripts.Runtime.Entity.EntityLogic childEntity, Transform parentTransform, object userData)
        {
            base.OnAttached(childEntity, parentTransform, userData);

            switch (childEntity)
            {
                case Thruster entity:
                    thruster = entity;
                    return;
                case Weapon weapon:
                    weapons.Add(weapon);
                    return;
                case Armor armor:
                    armors.Add(armor);
                    return;
            }
        }


        protected override void OnDetached(Plugins.GameFramework.Scripts.Runtime.Entity.EntityLogic childEntity, object userData)
        {
            base.OnDetached(childEntity, userData);

            switch (childEntity)
            {
                case Thruster:
                    thruster = null;
                    return;
                case Weapon weapon:
                    weapons.Remove(weapon);
                    return;
                case Armor armor:
                    armors.Remove(armor);
                    return;
            }
        }

        protected override void OnDead(Entity attacker)
        {
            base.OnDead(attacker);

            Base.GameEntry.Entity.ShowEffect(new EffectData(Base.GameEntry.Entity.GenerateSerialId(), aircraftData.DeadEffectId)
            {
                Position = CachedTransform.localPosition,
            });
            Base.GameEntry.Sound.PlaySound(aircraftData.DeadSoundId);
        }

        public override ImpactData GetImpactData()
        {
            return new ImpactData(aircraftData.Camp, aircraftData.Hp, 0, aircraftData.Defense);
        }
    }
}
