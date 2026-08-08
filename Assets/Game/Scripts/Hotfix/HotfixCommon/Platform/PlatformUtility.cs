// 创建时间：2026-08-02
// 修改时间：2026-08-02
// 审核时间：2026-08-02

namespace Game.Scripts.Hotfix.HotfixCommon.Platform
{
    /// <summary>
    ///     跨平台能力入口。内部按编译平台选择对应 Impl。
    /// </summary>
    public static class PlatformUtility
    {
        private static readonly IPlatformImpl Impl;

        static PlatformUtility()
        {
#if UNITY_IOS
            Impl = new IOSPlatformImpl();
#elif UNITY_ANDROID
            Impl = new AndroidPlatformImpl();
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            Impl = new WindowsPlatformImpl();
#else
            Impl = new DefaultPlatformImpl();
#endif
        }

        /// <summary>
        ///     当前平台是否支持震动。
        /// </summary>
        public static bool SupportsVibration => Impl.SupportsVibration;

        /// <summary>
        ///     当前平台是否支持全屏开关（设置界面用其替代震动项）。
        /// </summary>
        public static bool SupportsFullscreen => Impl.SupportsFullscreen;

        /// <summary>
        ///     当前是否全屏。不支持的平台恒为 false，赋值无效。
        /// </summary>
        public static bool IsFullscreen
        {
            get => Impl.IsFullscreen;
            set => Impl.IsFullscreen = value;
        }

        /// <summary>
        ///     触发设备震动。不支持的平台为空操作。
        /// </summary>
        public static void Vibrate()
        {
            Impl.Vibrate();
        }

        /// <summary>
        ///     根据本地偏好应用显示相关设置。不支持的平台为空操作。
        /// </summary>
        public static void ApplyDisplaySettings()
        {
            Impl.ApplyDisplaySettings();
        }
    }
}