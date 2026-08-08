// 创建时间：2026-08-02
// 修改时间：2026-08-02
// 审核时间：2026-08-02

namespace Game.Scripts.Hotfix.HotfixCommon.Platform
{
    /// <summary>
    ///     平台能力实现接口。各平台通过具体 Impl 提供差异化行为。
    /// </summary>
    public interface IPlatformImpl
    {
        /// <summary>
        ///     当前平台是否支持震动。
        /// </summary>
        bool SupportsVibration { get; }

        /// <summary>
        ///     当前平台是否支持全屏开关（用于设置界面替代震动项）。
        /// </summary>
        bool SupportsFullscreen { get; }

        /// <summary>
        ///     当前是否全屏。不支持的平台恒为 false，赋值无效。
        /// </summary>
        bool IsFullscreen { get; set; }

        /// <summary>
        ///     触发设备震动。不支持的平台应为空操作。
        /// </summary>
        void Vibrate();

        /// <summary>
        ///     根据本地偏好应用显示相关设置。不支持的平台为空操作。
        /// </summary>
        void ApplyDisplaySettings();
    }
}