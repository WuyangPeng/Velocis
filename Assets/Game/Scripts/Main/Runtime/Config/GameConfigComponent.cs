using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.Config
{
    public class GameConfigComponent : GameFrameworkComponent
    {
        public object ConfigInstance { get; private set; }

        public void SetConfigInstance(object configInstance)
        {
            ConfigInstance = configInstance;
        }
    }
}