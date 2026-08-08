// 创建时间：2026-08-02
// 修改时间：2026-08-02
// 审核时间：2026-08-02

#if UNITY_IOS
using UnityEngine;

namespace Game.Scripts.Hotfix.HotfixCommon.Platform
{
    /// <summary>
    /// iOS 平台实现。
    /// </summary>
    public sealed class IOSPlatformImpl : IPlatformImpl
    {
        public bool SupportsVibration => true;

        public bool SupportsFullscreen => false;

        public bool IsFullscreen
        {
            get => false;
            set { }
        }

        public void Vibrate()
        {
            Handheld.Vibrate();
        }

        public void ApplyDisplaySettings()
        {
        }
    }
}
#endif