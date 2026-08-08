using System.Collections.Generic;
using System.Reflection;

namespace Game.Scripts.Main.Runtime.Platform
{
    /// <summary>
    ///     未独立实现平台的默认编辑器/平台扩展能力实现（为空操作）。
    /// </summary>
    public sealed class DefaultEditorPlatformImpl : IEditorPlatformImpl
    {
        public bool HasHotfixAssemblies(List<Assembly> hotfixAssemblies)
        {
            return true;
        }
        public string OpenFilePanel(string title, string directory, string extension)
        {
            return null;
        }
    }
}
