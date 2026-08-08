// 创建时间：2026-08-02
// 修改时间：2026-08-02
// 审核时间：2026-08-02

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
using Game.Scripts.Main.Runtime.Definition.Constant;
using UnityEngine;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Hotfix.HotfixCommon.Platform
{
    /// <summary>
    ///     Windows 平台实现。
    /// </summary>
    public sealed class WindowsPlatformImpl : IPlatformImpl
    {
        public bool SupportsVibration => false;

        public bool SupportsFullscreen => true;

        public bool IsFullscreen
        {
            get => Screen.fullScreen;
            set => Screen.fullScreen = value;
        }

        public void Vibrate()
        {
            
        }

        public void ApplyDisplaySettings()
        {
            IsFullscreen = GameEntry.Setting.GetBool(Constant.Setting.FullscreenEnabled, true);
        }
    }
}
#endif