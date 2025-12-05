using Game.Scripts.Main.Runtime.DataTable;
using Game.Scripts.Main.Runtime.Entity;
using Game.Scripts.Main.Runtime.Entity.EntityData;
using GameFramework;
using UnityEngine;

namespace Game.Scripts.Main.Runtime.Game
{
    public class SurvivalGame : GameBase
    {
        private float mElapseSeconds;

        public override GameMode GameMode => GameMode.Survival;

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            base.Update(elapseSeconds, realElapseSeconds);

            mElapseSeconds += elapseSeconds;
            if (mElapseSeconds < 1f)
            {
                return;
            }

            mElapseSeconds = 0f;
            var dtAsteroid = Base.GameEntry.DataTable.GetDataTable<DRAsteroid>();
            var randomPositionX = SceneBackground.EnemySpawnBoundary.bounds.min.x + SceneBackground.EnemySpawnBoundary.bounds.size.x * (float)Utility.Random.GetRandomDouble();
            var randomPositionZ = SceneBackground.EnemySpawnBoundary.bounds.min.z + SceneBackground.EnemySpawnBoundary.bounds.size.z * (float)Utility.Random.GetRandomDouble();
            Base.GameEntry.Entity.ShowAsteroid(new AsteroidData(Base.GameEntry.Entity.GenerateSerialId(), 60000 + Utility.Random.GetRandom(dtAsteroid.Count))
            {
                Position = new Vector3(randomPositionX, 0f, randomPositionZ),
            });
        }
    }
}
