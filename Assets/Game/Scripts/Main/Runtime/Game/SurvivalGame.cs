using Game.Scripts.Main.Runtime.Base;
using Game.Scripts.Main.Runtime.DataTable;
using Game.Scripts.Main.Runtime.Entity;
using Game.Scripts.Main.Runtime.Entity.EntityData;
using GameFramework;
using UnityEngine;

namespace Game.Scripts.Main.Runtime.Game
{
    public class SurvivalGame : GameBase
    {
        private float _elapseSeconds;

        public override GameMode GameMode => GameMode.Survival;

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            base.Update(elapseSeconds, realElapseSeconds);

            _elapseSeconds += elapseSeconds;
            if (_elapseSeconds < 1f)
            {
                return;
            }

            _elapseSeconds = 0f;
            var dtAsteroid = GameEntry.DataTable.GetDataTable<DRAsteroid>();
            var randomPositionX = SceneBackground.EnemySpawnBoundary.bounds.min.x + SceneBackground.EnemySpawnBoundary.bounds.size.x * (float)Utility.Random.GetRandomDouble();
            var randomPositionZ = SceneBackground.EnemySpawnBoundary.bounds.min.z + SceneBackground.EnemySpawnBoundary.bounds.size.z * (float)Utility.Random.GetRandomDouble();
            GameEntry.Entity.ShowAsteroid(new AsteroidData(GameEntry.Entity.GenerateSerialId(), 60000 + Utility.Random.GetRandom(dtAsteroid.Count))
            {
                Position = new Vector3(randomPositionX, 0f, randomPositionZ)
            });
        }
    }
}