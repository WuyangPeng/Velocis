using Game.Scripts.Main.Runtime.Entity.EntityData;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.Entity.EntityLogic
{
    /// <summary>
    ///     特效类。
    /// </summary>
    public class Effect : Entity
    {
        [SerializeField] private EffectData effectData;

        private float _elapseSeconds;


        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            effectData = userData as EffectData;
            if (effectData == null)
            {
                Log.Error("Effect data is invalid.");
                return;
            }

            _elapseSeconds = 0f;
        }


        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            _elapseSeconds += elapseSeconds;
            if (_elapseSeconds >= effectData.KeepTime)
            {
                GameEntry.Entity.HideEntity(this);
            }
        }
    }
}