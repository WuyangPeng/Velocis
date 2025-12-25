using UnityEditor;
using UnityEngine;

namespace Game.Scripts.Main.Editor.Luban;

public sealed class FileCopierTool : EditorWindow
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
            FileCopier.CopyFiles(_sourcePath1, _sourcePath2);
        }
    }

    [MenuItem("Velocis/File Copier")]
    public static void ShowWindow()
    {
        GetWindow<FileCopierTool>("File Copier");
    }
}