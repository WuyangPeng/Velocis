using Game.Scripts.Main.Runtime.Entity.EntityData;
using Game.Scripts.Main.Runtime.Sound;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.Entity.EntityLogic
{
    /// <summary>
    /// 武器类。
    /// </summary>
    public class Weapon : Entity
    {
        private const string AttachPoint = "Weapon Point";

        [SerializeField]
        private WeaponData weaponData;

        private float nextAttackTime;

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            weaponData = userData as WeaponData;
            if (weaponData == null)
            {
                Log.Error("Weapon data is invalid.");
                return;
            }

            Base.GameEntry.Entity.AttachEntity(Entity, weaponData.OwnerId, AttachPoint);
        }


        protected override void OnAttachTo(Plugins.GameFramework.Scripts.Runtime.Entity.EntityLogic parentEntity, Transform parentTransform, object userData)
        {
            base.OnAttachTo(parentEntity, parentTransform, userData);

            Name = Utility.Text.Format("Weapon of {0}", parentEntity.Name);
            CachedTransform.localPosition = Vector3.zero;
        }

        public void TryAttack()
        {
            if (Time.time < nextAttackTime)
            {
                return;
            }

            nextAttackTime = Time.time + weaponData.AttackInterval;
            Base.GameEntry.Entity.ShowBullet(new BulletData(Base.GameEntry.Entity.GenerateSerialId(), weaponData.BulletId, weaponData.OwnerId, weaponData.OwnerCamp, weaponData.Attack, weaponData.BulletSpeed)
            {
                Position = CachedTransform.position,
            });
            Base.GameEntry.Sound.PlaySound(weaponData.BulletSoundId);
        }
    }
}
