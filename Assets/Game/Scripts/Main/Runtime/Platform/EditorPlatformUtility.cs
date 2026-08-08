using System.Collections.Generic;
using System.Reflection;

namespace Game.Scripts.Main.Runtime.Platform
{
    /// <summary>
    ///     编辑器 / 开发环境扩展能力入口。
    /// </summary>
    public static class EditorPlatformUtility
    {
        private static readonly IEditorPlatformImpl Impl;

        static EditorPlatformUtility()
        {
#if UNITY_EDITOR
            Impl = new EditorPlatformImpl();
#elif UNITY_IOS
            Impl = new IOSEditorPlatformImpl();
#elif UNITY_ANDROID
            Impl = new AndroidEditorPlatformImpl();
#elif UNITY_STANDALONE_WIN
            Impl = new WindowsEditorPlatformImpl();
#else
            Impl = new DefaultEditorPlatformImpl();
#endif
        }

        /// <summary>
        ///     检查热更新程序集是否存在。
        /// </summary>
        public static bool HasHotfixAssemblies(List<Assembly> hotfixAssemblies)
        {
            return Impl.HasHotfixAssemblies(hotfixAssemblies);
        }

        /// <summary>
        ///     打开文件选择面板（仅在 Editor 环境生效）。
        /// </summary>
        public static string OpenFilePanel(string title, string directory, string extension)
        {
            return Impl.OpenFilePanel(title, directory, extension);
        }
    }
}
