using Game.Scripts.Main.Runtime.GameModule.Base;

namespace Game.Scripts.Main.Runtime.GameModule.Debug
{
    [Module]
    public class DebugModule : BaseModule
    {
        public bool IsDebug { get; set; } = false;
    }
}