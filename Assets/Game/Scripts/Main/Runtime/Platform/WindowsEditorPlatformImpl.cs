#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.Platform
{
    /// <summary>
    ///     Windows 平台的编辑器/平台扩展能力实现（使用原生 Win32 GetOpenFileName）。
    /// </summary>
    public sealed class WindowsEditorPlatformImpl : IEditorPlatformImpl
    {
        public bool HasHotfixAssemblies(List<Assembly> hotfixAssemblies)
        {
            return true;
        }
        public string OpenFilePanel(string title, string directory, string extension)
        {
            try
            {
                var ofn = new OpenFileName();
                ofn.structSize = Marshal.SizeOf(typeof(OpenFileName));
                ofn.dlgOwner = GetActiveWindow();

                if (!string.IsNullOrEmpty(extension))
                {
                    var pattern = "*." + extension.Replace(",", ";*.");
                    ofn.filter = $"Image Files ({pattern})\0{pattern}\0All Files (*.*)\0*.*\0\0";
                }
                else
                {
                    ofn.filter = "All Files (*.*)\0*.*\0\0";
                }

                var fileChars = new char[1024];
                ofn.file = new string(fileChars);
                ofn.maxFile = ofn.file.Length;
                ofn.fileTitle = new string(new char[256]);
                ofn.maxFileTitle = ofn.fileTitle.Length;
                ofn.initialDir = string.IsNullOrEmpty(directory) ? null : directory.Replace('/', '\\');
                ofn.title = string.IsNullOrEmpty(title) ? "Select File" : title;

                // OFN_PATHMUSTEXIST | OFN_FILEMUSTEXIST | OFN_NOCHANGEDIR
                ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;

                Log.Info("[WindowsEditorPlatformImpl] OpenFilePanel calling Win32 GetOpenFileName directly.");
                if (GetOpenFileName(ref ofn))
                {
                    return ofn.file;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"[WindowsEditorPlatformImpl] Win32 dialog error: {ex}");
            }

            return null;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct OpenFileName
        {
            public int structSize;
            public IntPtr dlgOwner;
            public IntPtr instance;
            public string filter;
            public string customFilter;
            public int maxCustomFilter;
            public int filterIndex;
            public string file;
            public int maxFile;
            public string fileTitle;
            public int maxFileTitle;
            public string initialDir;
            public string title;
            public int flags;
            public short fileOffset;
            public short fileExtension;
            public string defExt;
            public IntPtr lCustData;
            public IntPtr lpfnHook;
            public string lpTemplateName;
            public IntPtr pvReserved;
            public int dwReserved;
            public int flagsEx;
        }

        [DllImport("comdlg32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool GetOpenFileName([In, Out] ref OpenFileName ofn);

        [DllImport("user32.dll")]
        private static extern IntPtr GetActiveWindow();
    }
}
#endif
