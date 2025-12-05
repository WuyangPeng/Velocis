using Game.Scripts.Main.Runtime.Entity.EntityData;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.Entity.EntityLogic
{
    /// <summary>
    /// 特效类。
    /// </summary>
    public class Effect : Entity
    {
        [SerializeField]
        private EffectData effectData;

        private float mElapseSeconds;


        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            effectData = userData as EffectData;
            if (effectData == null)
            {
                Log.Error("Effect data is invalid.");
                return;
            }

            mElapseSeconds = 0f;
        }


        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            mElapseSeconds += elapseSeconds;
            if (mElapseSeconds >= effectData.KeepTime)
            {
                Base.GameEntry.Entity.HideEntity(this);
            }
        }
    }
}
