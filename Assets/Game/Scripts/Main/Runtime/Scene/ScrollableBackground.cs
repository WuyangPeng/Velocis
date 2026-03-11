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

        private Transform m_CachedTransform;
        private Vector3 m_StartPosition = Vector3.zero;

        private void Start()
        {
            m_CachedTransform = transform;
            m_StartPosition = m_CachedTransform.position;
        }

        private void Update()
        {
            var newPosition = Mathf.Repeat(Time.time * scrollSpeed, tileSize);
            m_CachedTransform.position = m_StartPosition + Vector3.forward * newPosition;
        }

        public BoxCollider VisibleBoundary => visibleBoundary;

        public BoxCollider PlayerMoveBoundary => playerMoveBoundary;

        public BoxCollider EnemySpawnBoundary => enemySpawnBoundary;
    }
}
