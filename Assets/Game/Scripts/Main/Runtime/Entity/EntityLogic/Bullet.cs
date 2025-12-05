using Game.Scripts.Main.Runtime.Definition.DataStruct;
using Game.Scripts.Main.Runtime.Entity.EntityData;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.Entity.EntityLogic
{
    /// <summary>
    /// 子弹类。
    /// </summary>
    public class Bullet : Entity
    {
        [SerializeField]
        private BulletData bulletData;

        public ImpactData GetImpactData()
        {
            return new ImpactData(bulletData.OwnerCamp, 0, bulletData.Attack, 0);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            bulletData = userData as BulletData;
            if (bulletData != null)
            {
                return;
            }
            Log.Error("Bullet data is invalid.");
        }


        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            CachedTransform.Translate(Vector3.forward * (bulletData.Speed * elapseSeconds), Space.World);
        }
    }
}
