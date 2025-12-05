using Game.Scripts.Main.Runtime.Entity.EntityData;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.Entity.EntityLogic
{
    /// <summary>
    /// 推进器类。
    /// </summary>
    public class Thruster : Entity
    {
        private const string AttachPoint = "Thruster Point";

        [SerializeField]
        private ThrusterData thrusterData;

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            thrusterData = userData as ThrusterData;
            if (thrusterData == null)
            {
                Log.Error("Thruster data is invalid.");
                return;
            }

            Base.GameEntry.Entity.AttachEntity(this, thrusterData.OwnerId, AttachPoint);
        }


        protected override void OnAttachTo(Plugins.GameFramework.Scripts.Runtime.Entity.EntityLogic parentEntity, Transform parentTransform, object userData)
        {
            base.OnAttachTo(parentEntity, parentTransform, userData);

            Name = Utility.Text.Format("Thruster of {0}", parentEntity.Name);
            CachedTransform.localPosition = Vector3.zero;
        }
    }
}
