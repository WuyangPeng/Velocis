using Game.Scripts.Main.Runtime.Definition.DataStruct;
using Game.Scripts.Main.Runtime.Entity.EntityData;
using Game.Scripts.Main.Runtime.Sound;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.Entity.EntityLogic
{
    /// <summary>
    /// 小行星类。
    /// </summary>
    public class Asteroid : TargetableObject
    {
        [SerializeField]
        private AsteroidData asteroidData;

        private Vector3 rotateSphere = Vector3.zero;


        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            asteroidData = userData as AsteroidData;
            if (asteroidData == null)
            {
                Log.Error("Asteroid data is invalid.");
                return;
            }

            rotateSphere = Random.insideUnitSphere;
        }


        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            CachedTransform.Translate(Vector3.back * (asteroidData.Speed * elapseSeconds), Space.World);
            CachedTransform.Rotate(rotateSphere * (asteroidData.AngularSpeed * elapseSeconds), Space.Self);
        }

        protected override void OnDead(Entity attacker)
        {
            base.OnDead(attacker);

            Base.GameEntry.Entity.ShowEffect(new EffectData(Base.GameEntry.Entity.GenerateSerialId(), asteroidData.DeadEffectId)
            {
                Position = CachedTransform.localPosition,
            });
            Base.GameEntry.Sound.PlaySound(asteroidData.DeadSoundId);
        }

        public override ImpactData GetImpactData()
        {
            return new ImpactData(asteroidData.Camp, asteroidData.Hp, asteroidData.Attack, 0);
        }
    }
}
