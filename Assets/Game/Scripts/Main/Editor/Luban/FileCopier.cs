using System;
using System.IO;
using UnityEditor;
using UnityEngine; 

namespace Game.Scripts.Main.Editor.Luban
{
    public static class FileCopier
    {
        public static void CopyFiles(string sourcePath1, string sourcePath2)
        {
            var targetBase = Path.Combine(Application.dataPath, "Game");
            var target1 = Path.Combine(targetBase, "luban");
            var target2 = Path.Combine(targetBase, "proto");

            var success1 = CopyDirectory(sourcePath1, target1);
            var success2 = CopyDirectory(sourcePath2, target2);

            if (!success1 && !success2)
            {
                return;
            }

            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("Result", "Copy operation completed. Check console for details.", "OK"); 
        }

        private static bool CopyDirectory(string sourceDir, string targetDir)
        {
            if (string.IsNullOrEmpty(sourceDir))
            {
                return false;
            }

            if (!Directory.Exists(sourceDir))
            {
                Debug.Log($"Source directory not found: {sourceDir}");
                return false;
            }

            try
            {
                if (Directory.Exists(targetDir))
                {
                    Directory.Delete(targetDir, true);
                }
                Directory.CreateDirectory(targetDir);

                var files = Directory.GetFiles(sourceDir);
                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file);
                    var destFile = Path.Combine(targetDir, fileName);
                    File.Copy(file, destFile, true);
                }

                var subDirs = Directory.GetDirectories(sourceDir);
                foreach (var subDir in subDirs)
                {
                    var dirName = Path.GetFileName(subDir);
                    var destSubDir = Path.Combine(targetDir, dirName);
                    CopyDirectory(subDir, destSubDir);
                }

                return true;
            }
            catch (Exception e)
            {
                Debug.Log($"Error copying files from {sourceDir} to {targetDir}: {e.Message}");
                return false;
            }
        }
    }
}