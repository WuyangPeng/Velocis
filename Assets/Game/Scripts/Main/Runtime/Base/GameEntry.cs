using UnityEngine;

namespace Game.Scripts.Main.Runtime.Base
{
    /// <summary>
    ///     游戏入口。
    /// </summary>
    public partial class GameEntry : MonoBehaviour
    {
        private void Start()
        {
            InitBuiltinComponents();
            InitCustomComponents();
            InitModuleComponent();
            InitFileSystemComponent();
        }
    }
}