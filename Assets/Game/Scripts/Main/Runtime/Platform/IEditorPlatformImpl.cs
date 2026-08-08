using System.Collections.Generic;
using System.Reflection;

namespace Game.Scripts.Main.Runtime.Platform
{
    /// <summary>
    ///     Editor / 编辑器扩展能力实现接口。
    /// </summary>
    public interface IEditorPlatformImpl
    {
        /// <summary>
        ///     检查热更新程序集是否存在。
        /// </summary>
        bool HasHotfixAssemblies(List<Assembly> hotfixAssemblies);

        /// <summary>
        ///     打开文件选择面板（在 Editor 环境中有效，非 Editor 环境或取消时返回 null）。
        /// </summary>
        string OpenFilePanel(string title, string directory, string extension);
    }
}
