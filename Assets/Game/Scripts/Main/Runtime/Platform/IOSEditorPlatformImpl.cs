#if UNITY_IOS
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Game.Scripts.Main.Runtime.Platform
{
    /// <summary>
    ///     iOS 平台的编辑器/平台扩展能力实现（使用 iOS 原生 UIImagePickerController 交互）。
    /// </summary>
    public sealed class IOSEditorPlatformImpl : IEditorPlatformImpl
    {
        public bool HasHotfixAssemblies(List<Assembly> hotfixAssemblies)
        {
            return true;
        }
        [DllImport("__Internal")]
        private static extern void _openIOSPhotoLibrary();

        public string OpenFilePanel(string title, string directory, string extension)
        {
            try
            {
                _openIOSPhotoLibrary();
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogWarning($"[IOSEditorPlatformImpl] OpenFilePanel failed: {e.Message}");
            }
            return null;
        }
    }
}
#endif
