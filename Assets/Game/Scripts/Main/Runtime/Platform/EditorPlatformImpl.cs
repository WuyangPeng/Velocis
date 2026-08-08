#if UNITY_EDITOR
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

namespace Game.Scripts.Main.Runtime.Platform
{
    /// <summary>
    ///     Unity Editor 环境下的编辑器能力实现。
    /// </summary>
    public sealed class EditorPlatformImpl : IEditorPlatformImpl
    {
        public bool HasHotfixAssemblies(List<Assembly> hotfixAssemblies)
        {
            if (hotfixAssemblies == null || hotfixAssemblies.Count == 0)
            {
                UnityGameFramework.Runtime.Log.Error("Hotfix assemblies are missing. Check HybridCLR hot-update assembly settings.");
                return false;
            }
            return true;
        }

        public string OpenFilePanel(string title, string directory, string extension)
        {
            return EditorUtility.OpenFilePanel(title, directory, extension);
        }
    }
}
#endif
