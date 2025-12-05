using Game.Scripts.Main.Runtime.GameModule.Base;
using UnityEngine;

namespace Game.Scripts.Main.Runtime.Base
{
    public partial class GameEntry : MonoBehaviour
    {
        public static ModuleComponent ModuleComponent
        {
            get;
            private set;
        }

        private static void InitModuleComponent()
        {
            ModuleComponent = UnityGameFramework.Runtime.GameEntry.GetComponent<ModuleComponent>(); 
        }
    }
}
