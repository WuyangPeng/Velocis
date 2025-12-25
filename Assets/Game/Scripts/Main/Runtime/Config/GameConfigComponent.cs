using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.Config
{
    public class GameConfigComponent : GameFrameworkComponent
    {
        private readonly GameConfig gameConfig = new();

        public void Initialize()
        {
            gameConfig.Initialize();
        }
    }
}