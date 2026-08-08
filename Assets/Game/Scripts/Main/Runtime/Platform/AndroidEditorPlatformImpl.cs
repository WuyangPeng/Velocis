#if UNITY_ANDROID
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Game.Scripts.Main.Runtime.Platform
{
    /// <summary>
    ///     Android 平台的编辑器/平台扩展能力实现（使用 Android 原生 Intent 选择图片）。
    /// </summary>
    public sealed class AndroidEditorPlatformImpl : IEditorPlatformImpl
    {
        public bool HasHotfixAssemblies(List<Assembly> hotfixAssemblies)
        {
            return true;
        }
        public string OpenFilePanel(string title, string directory, string extension)
        {
            try
            {
                using (var intentClass = new AndroidJavaClass("android.content.Intent"))
                {
                    using (var intent = new AndroidJavaObject("android.content.Intent", intentClass.GetStatic<string>("ACTION_GET_CONTENT")))
                    {
                        intent.Call<AndroidJavaObject>("setType", "image/*");
                        intent.Call<AndroidJavaObject>("addCategory", intentClass.GetStatic<string>("CATEGORY_OPENABLE"));

                        using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                        {
                            using (var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                            {
                                var chooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intent, title);
                                currentActivity.Call("startActivity", chooser);
                            }
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"[AndroidEditorPlatformImpl] OpenFilePanel failed: {e.Message}");
            }

            return null;
        }
    }
}
#endif
