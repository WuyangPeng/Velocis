using Game.Scripts.Main.Runtime.Entity;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.Scene
{
    public class HideByBoundary : MonoBehaviour
    {
        private void OnTriggerExit(Collider other)
        {
            var go = other.gameObject;
            if (go.TryGetComponent<Entity.EntityLogic.Entity>(out var entity))
            {
                Base.GameEntry.Entity.HideEntity(entity);
                return;
            }

            Log.Warning("Unknown GameObject '{0}', you must use entity only.", go.name);
            Destroy(go);
        }
    }
}
