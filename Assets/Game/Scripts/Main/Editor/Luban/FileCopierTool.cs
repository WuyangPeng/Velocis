using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Editor.Luban;

public class FileCopierTool : EditorWindow
{
    private const string PrefKeySource1 = "FileCopierTool_SourcePath1";
    private const string PrefKeySource2 = "FileCopierTool_SourcePath2";
    private string _sourcePath1 = "";
    private string _sourcePath2 = "";

    private void OnEnable()
    {
        _sourcePath1 = EditorPrefs.GetString(PrefKeySource1, "");
        _sourcePath2 = EditorPrefs.GetString(PrefKeySource2, "");
    }

    private void OnGUI()
    {
        GUILayout.Label("Copy Files to Runtime", EditorStyles.boldLabel);

        GUILayout.Space(10);

        GUILayout.Label("Source for luban:");
        EditorGUILayout.BeginHorizontal();
        var newSourcePath1 = EditorGUILayout.TextField(_sourcePath1);
        if (newSourcePath1 != _sourcePath1)
        {
            _sourcePath1 = newSourcePath1;
            EditorPrefs.SetString(PrefKeySource1, _sourcePath1);
        }

        if (GUILayout.Button("Browse", GUILayout.Width(60)))
        {
            var path = EditorUtility.OpenFolderPanel("Select Source 1", _sourcePath1, "");
            if (!string.IsNullOrEmpty(path))
            {
                _sourcePath1 = path;
                EditorPrefs.SetString(PrefKeySource1, _sourcePath1);
            }
        }

        EditorGUILayout.EndHorizontal();

        GUILayout.Space(5);

        GUILayout.Label("Source for proto:");
        EditorGUILayout.BeginHorizontal();
        var newSourcePath2 = EditorGUILayout.TextField(_sourcePath2);
        if (newSourcePath2 != _sourcePath2)
        {
            _sourcePath2 = newSourcePath2;
            EditorPrefs.SetString(PrefKeySource2, _sourcePath2);
        }

        if (GUILayout.Button("Browse", GUILayout.Width(60)))
        {
            var path = EditorUtility.OpenFolderPanel("Select Source 2", _sourcePath2, "");
            if (!string.IsNullOrEmpty(path))
            {
                _sourcePath2 = path;
                EditorPrefs.SetString(PrefKeySource2, _sourcePath2);
            }
        }

        EditorGUILayout.EndHorizontal();

        GUILayout.Space(20);

        if (GUILayout.Button("Confirm"))
        {
            CopyFiles();
        }
    }

    [MenuItem("Velocis/File Copier")]
    public static void ShowWindow()
    {
        GetWindow<FileCopierTool>("File Copier");
    }

    private void CopyFiles()
    {
        var targetBase = Path.Combine(Application.dataPath, "Game");
        var target1 = Path.Combine(targetBase, "luban");
        var target2 = Path.Combine(targetBase, "proto");

        var success1 = CopyDirectory(_sourcePath1, target1);
        var success2 = CopyDirectory(_sourcePath2, target2);

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
            Log.Error($"Source directory not found: {sourceDir}");
            return false;
        }

        try
        {
            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }

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
            Log.Error($"Error copying files from {sourceDir} to {targetDir}: {e.Message}");
            return false;
        }
    }
}