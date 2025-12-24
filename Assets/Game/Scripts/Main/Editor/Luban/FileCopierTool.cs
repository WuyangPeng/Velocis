using UnityEngine;
using UnityEditor;
using System.IO;

namespace Game.Scripts.Main.Editor.Luban
{
    public class FileCopierTool : EditorWindow
    {
        private string sourcePath1 = "";
        private string sourcePath2 = "";

        private const string PREF_KEY_SOURCE1 = "FileCopierTool_SourcePath1";
        private const string PREF_KEY_SOURCE2 = "FileCopierTool_SourcePath2";

        [MenuItem("Velocis/File Copier")]
        public static void ShowWindow()
        {
            GetWindow<FileCopierTool>("File Copier");
        }

        private void OnEnable()
        {
            sourcePath1 = EditorPrefs.GetString(PREF_KEY_SOURCE1, "");
            sourcePath2 = EditorPrefs.GetString(PREF_KEY_SOURCE2, "");
        }

        private void OnGUI()
        {
            GUILayout.Label("Copy Files to Runtime", EditorStyles.boldLabel);

            GUILayout.Space(10);

            // Input 1
            GUILayout.Label("Source for luban:");
            EditorGUILayout.BeginHorizontal();
            string newSourcePath1 = EditorGUILayout.TextField(sourcePath1);
            if (newSourcePath1 != sourcePath1)
            {
                sourcePath1 = newSourcePath1;
                EditorPrefs.SetString(PREF_KEY_SOURCE1, sourcePath1);
            }
            
            if (GUILayout.Button("Browse", GUILayout.Width(60)))
            {
                string path = EditorUtility.OpenFolderPanel("Select Source 1", sourcePath1, "");
                if (!string.IsNullOrEmpty(path))
                {
                    sourcePath1 = path;
                    EditorPrefs.SetString(PREF_KEY_SOURCE1, sourcePath1);
                }
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5);

            // Input 2
            GUILayout.Label("Source for proto:");
            EditorGUILayout.BeginHorizontal();
            string newSourcePath2 = EditorGUILayout.TextField(sourcePath2);
            if (newSourcePath2 != sourcePath2)
            {
                sourcePath2 = newSourcePath2;
                EditorPrefs.SetString(PREF_KEY_SOURCE2, sourcePath2);
            }

            if (GUILayout.Button("Browse", GUILayout.Width(60)))
            {
                string path = EditorUtility.OpenFolderPanel("Select Source 2", sourcePath2, "");
                if (!string.IsNullOrEmpty(path))
                {
                    sourcePath2 = path;
                    EditorPrefs.SetString(PREF_KEY_SOURCE2, sourcePath2);
                }
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(20);

            if (GUILayout.Button("Confirm"))
            {
                CopyFiles();
            }
        }

        private void CopyFiles()
        {
            string targetBase = Path.Combine(Application.dataPath, "Game");
            string target1 = Path.Combine(targetBase, "luban");
            string target2 = Path.Combine(targetBase, "proto");

            bool success1 = CopyDirectory(sourcePath1, target1);
            bool success2 = CopyDirectory(sourcePath2, target2);

            if (success1 || success2)
            {
                AssetDatabase.Refresh();
                EditorUtility.DisplayDialog("Result", "Copy operation completed. Check console for details.", "OK");
            }
        }

        private bool CopyDirectory(string sourceDir, string targetDir)
        {
            if (string.IsNullOrEmpty(sourceDir))
            {
                // It's okay if one is empty, just skip
                return false;
            }

            if (!Directory.Exists(sourceDir))
            {
                Debug.LogError($"Source directory not found: {sourceDir}");
                return false;
            }

            try
            {
                if (!Directory.Exists(targetDir))
                {
                    Directory.CreateDirectory(targetDir);
                }

                // Copy files
                string[] files = Directory.GetFiles(sourceDir);
                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    string destFile = Path.Combine(targetDir, fileName);
                    File.Copy(file, destFile, true);
                }

                // Copy subdirectories
                string[] subDirs = Directory.GetDirectories(sourceDir);
                foreach (string subDir in subDirs)
                {
                    string dirName = Path.GetFileName(subDir);
                    string destSubDir = Path.Combine(targetDir, dirName);
                    CopyDirectory(subDir, destSubDir);
                }
                
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error copying files from {sourceDir} to {targetDir}: {e.Message}");
                return false;
            }
        }
    }
}
