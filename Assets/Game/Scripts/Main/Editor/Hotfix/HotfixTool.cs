using System.IO;
using UnityEditor;
using UnityEngine;
using Game.Scripts.Main.Editor.ResourceTools;

namespace Game.Scripts.Main.Editor.Hotfix
{
    public static class HotfixTool
    {
        [MenuItem("Velocis/Hotfix/Compile And Copy DLLs", priority = 100)]
        public static void CompileAndCopyDlls()
        {
            // 1. Compile DLLs using HybridCLR API
            Debug.Log("[HotfixTool] Starting HybridCLR DLL compilation...");
            HybridCLR.Editor.Commands.CompileDllCommand.CompileDllActiveBuildTarget();
            Debug.Log("[HotfixTool] HybridCLR DLL compilation completed.");

            // 2. Copy compiled DLLs to Assets/Game/HotfixDll/Hotfix
            var target = EditorUserBuildSettings.activeBuildTarget.ToString();
            var sourceDir = Path.Combine(Directory.GetCurrentDirectory(), "HybridCLRData", "HotUpdateDlls", target);
            var destDir = Path.Combine(Application.dataPath, "Game", "HotfixDll", "Hotfix");

            if (!Directory.Exists(sourceDir))
            {
                Debug.LogError($"[HotfixTool] Compiled DLLs source directory not found: {sourceDir}");
                EditorUtility.DisplayDialog("Hotfix Tool Error", $"Source directory not found: {sourceDir}", "OK");
                return;
            }

            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }

            var dlls = new[]
            {
                "Velocis.HotfixCommon.dll",
                "Velocis.HotfixFramework.Runtime.dll",
                "Velocis.HotfixMain.dll",
                "Velocis.HotfixBusiness.dll"
            };

            int successCount = 0;
            foreach (var dll in dlls)
            {
                var sourcePath = Path.Combine(sourceDir, dll);
                var destPath = Path.Combine(destDir, dll + ".bytes");

                if (File.Exists(sourcePath))
                {
                    File.Copy(sourcePath, destPath, true);
                    Debug.Log($"[HotfixTool] Copied hotfix DLL: {sourcePath} -> {destPath}");
                    successCount++;
                }
                else
                {
                    Debug.LogError($"[HotfixTool] Hotfix DLL not found at source: {sourcePath}");
                }
            }

            // 3. Refresh AssetDatabase so Sync can find the new files
            AssetDatabase.Refresh();

            // 4. Sync resources in ResourceCollection.xml
            Debug.Log("[HotfixTool] Syncing resource collection...");
            ResourceCollectionSyncMenu.Sync();
            Debug.Log("[HotfixTool] Resource collection sync completed.");

            if (successCount == dlls.Length)
            {
                EditorUtility.DisplayDialog("Hotfix Tool", "Compile, copy, and sync hotfix DLLs completed successfully! Please run Resource Builder next.", "OK");
            }
            else
            {
                EditorUtility.DisplayDialog("Hotfix Tool", "Process completed with some errors. Please check the Console logs.", "OK");
            }
        }
    }
}
