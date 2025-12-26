using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Game.Scripts.Main.Editor.Luban
{
    public static class FileCopier
    {
        public static void CopyFiles(string sourceLubanPath, string sourceProtoPath)
        {
            var targetBase = Path.Combine(Application.dataPath, "Game");
            var lubanTarget = Path.Combine(targetBase, "luban");
            var protoTarget = Path.Combine(targetBase, "proto");

            var success1 = CopyDirectory(sourceLubanPath, lubanTarget);
            var success2 = CopyDirectory(sourceProtoPath, protoTarget);

            if (!success1 && !success2)
            {
                EditorUtility.DisplayDialog("Result", "Copy operation error! Check console for details.", "OK");

                return;
            }

            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("Result", "Copy operation completed. Check console for details.", "OK");

            Debug.Log("Copy operation completed");
        }

        private static bool CopyDirectory(string sourceDirectory, string targetDirectory)
        {
            try
            {
                return DoCopyDirectory(sourceDirectory, targetDirectory);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error copying files from {sourceDirectory} to {targetDirectory}: {e.Message}");
                return false;
            }
        }

        private static bool DoCopyDirectory(string sourceDirectory, string targetDirectory)
        {
            if (!ValidateSourceDirectory(sourceDirectory))
            {
                return false;
            }

            PrepareTargetDirectory(targetDirectory);
            CopyFilesInDirectory(sourceDirectory, targetDirectory);

            return CopyAllSubDirectories(sourceDirectory, targetDirectory);
        }

        private static bool ValidateSourceDirectory(string sourceDirectory)
        {
            if (string.IsNullOrEmpty(sourceDirectory))
            {
                Debug.LogError("Source directory is empty.");
                return false;
            }

            if (Directory.Exists(sourceDirectory))
            {
                return true;
            }

            Debug.LogError($"Source directory not found: {sourceDirectory}");

            return false;
        }

        private static void PrepareTargetDirectory(string targetDirectory)
        {
            if (Directory.Exists(targetDirectory))
            {
                Directory.Delete(targetDirectory, true);
            }

            Directory.CreateDirectory(targetDirectory);
        }

        private static void CopyFilesInDirectory(string sourceDirectory, string targetDirectory)
        {
            var files = Directory.GetFiles(sourceDirectory);
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                var destFile = Path.Combine(targetDirectory, fileName);
                File.Copy(file, destFile, true);
            }
        }

        private static bool CopyAllSubDirectories(string sourceDirectory, string targetDirectory)
        {
            var subDirectories = Directory.GetDirectories(sourceDirectory);
            return subDirectories.All(subDirectory => CopySubDirectory(targetDirectory, subDirectory));
        }

        private static bool CopySubDirectory(string targetDirectory, string subDirectory)
        {
            var directoryName = Path.GetFileName(subDirectory);
            var destSubDirectory = Path.Combine(targetDirectory, directoryName);

            return CopyDirectory(subDirectory, destSubDirectory);
        }
    }
}