using UnityEngine;

namespace Game.Scripts.Main.Runtime.Scene
{
    public class ScrollableBackground : MonoBehaviour
    {
        [SerializeField]
        private float scrollSpeed = -0.25f;

        [SerializeField]
        private float tileSize = 30f;

        [SerializeField]
        private BoxCollider visibleBoundary;

        [SerializeField]
        private BoxCollider playerMoveBoundary;

        [SerializeField]
        private BoxCollider enemySpawnBoundary;

        private Transform _cachedTransform;
        private Vector3 _startPosition = Vector3.zero;

        private void Start()
        {
            _cachedTransform = transform;
            _startPosition = _cachedTransform.position;
        }

        private void Update()
        {
            var newPosition = Mathf.Repeat(Time.time * scrollSpeed, tileSize);
            _cachedTransform.position = _startPosition + Vector3.forward * newPosition;
        }

        public BoxCollider VisibleBoundary => visibleBoundary;

        public BoxCollider PlayerMoveBoundary => playerMoveBoundary;

        public BoxCollider EnemySpawnBoundary => enemySpawnBoundary;
    }
}
