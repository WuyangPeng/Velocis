using Game.Scripts.Main.Runtime.Entity.EntityData;
using Game.Scripts.Main.Runtime.Scene;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.Entity.EntityLogic
{
    public class MyAircraft : Aircraft
    {
        [SerializeField] private MyAircraftData myAircraftData;

        private Rect _playerMoveBoundary;
        private Vector3 _targetPosition = Vector3.zero;


        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            myAircraftData = userData as MyAircraftData;
            if (myAircraftData == null)
            {
                Log.Error("My aircraft data is invalid.");
                return;
            }

            var sceneBackground = FindObjectOfType<ScrollableBackground>();
            if (sceneBackground == null)
            {
                Log.Warning("Can not find scene background.");
                return;
            }

            _playerMoveBoundary = new Rect(sceneBackground.PlayerMoveBoundary.bounds.min.x,
                sceneBackground.PlayerMoveBoundary.bounds.min.z,
                sceneBackground.PlayerMoveBoundary.bounds.size.x,
                sceneBackground.PlayerMoveBoundary.bounds.size.z);
        }


        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (Input.GetMouseButton(0))
            {
                if (Camera.main == null)
                {
                    Debug.LogError("camera main is null.");
                    return;
                }

                var point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _targetPosition = new Vector3(point.x, 0f, point.z);

                foreach (var weapon in weapons)
                {
                    weapon.TryAttack();
                }
            }

            var direction = _targetPosition - CachedTransform.localPosition;
            if (direction.sqrMagnitude <= Vector3.kEpsilon)
            {
                return;
            }

            var speed = Vector3.ClampMagnitude(direction.normalized * (myAircraftData.Speed * elapseSeconds), direction.magnitude);
            CachedTransform.localPosition = new Vector3
            (
                Mathf.Clamp(CachedTransform.localPosition.x + speed.x, _playerMoveBoundary.xMin, _playerMoveBoundary.xMax),
                0f,
                Mathf.Clamp(CachedTransform.localPosition.z + speed.z, _playerMoveBoundary.yMin, _playerMoveBoundary.yMax)
            );
        }
    }
}