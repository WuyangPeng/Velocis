// 创建时间：2026-08-02
// 修改时间：2026-08-02
// 审核时间：2026-08-02

namespace Game.Scripts.Hotfix.HotfixCommon.Platform
{
    /// <summary>
    ///     默认平台实现（其它未单独处理的平台）。
    /// </summary>
    public sealed class DefaultPlatformImpl : IPlatformImpl
    {
        public bool SupportsVibration => false;

        public bool SupportsFullscreen => false;

        public bool IsFullscreen
        {
            get => false;
            set { }
        }

        public void Vibrate()
        {
        }

        public void ApplyDisplaySettings()
        {
        }
    }
}