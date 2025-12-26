using UnityEditor;
using UnityEngine;

namespace Game.Scripts.Main.Editor.Luban
{
    public sealed class FileCopierTool : EditorWindow
    {
        private const string LubanPrefKeySource = "FileCopierToolLubanSourcePath";
        private const string ProtoPrefKeySource = "FileCopierToolProtoSourcePath";
        private string _lubanSourcePath = "";
        private string _protoSourcePath = "";

        private void OnEnable()
        {
            _lubanSourcePath = EditorPrefs.GetString(LubanPrefKeySource, "");
            _protoSourcePath = EditorPrefs.GetString(ProtoPrefKeySource, "");
        }

        private void OnGUI()
        {
            GUILayout.Label("Copy Files to Runtime", EditorStyles.boldLabel);
            GUILayout.Space(10);

            DrawSourcePathInput("Source for luban:", ref _lubanSourcePath, LubanPrefKeySource, "Select Luban Source");
            GUILayout.Space(5);

            DrawSourcePathInput("Source for proto:", ref _protoSourcePath, ProtoPrefKeySource, "Select Proto Source");
            GUILayout.Space(20);

            if (GUILayout.Button("Confirm"))
            {
                FileCopier.CopyFiles(_lubanSourcePath, _protoSourcePath);
            }
        }

        private static void DrawSourcePathInput(string label, ref string path, string prefKey, string browserTitle)
        {
            GUILayout.Label(label);
            EditorGUILayout.BeginHorizontal();

            var newPath = EditorGUILayout.TextField(path);
            if (newPath != path)
            {
                path = newPath;
                EditorPrefs.SetString(prefKey, path);
            }

            if (GUILayout.Button("Browse", GUILayout.Width(60)))
            {
                var selectedPath = EditorUtility.OpenFolderPanel(browserTitle, path, "");
                if (!string.IsNullOrEmpty(selectedPath))
                {
                    path = selectedPath;
                    EditorPrefs.SetString(prefKey, path);
                }
            }

            EditorGUILayout.EndHorizontal();
        }


        [MenuItem("Velocis/File Copier")]
        public static void ShowWindow()
        {
            GetWindow<FileCopierTool>("File Copier");
        }
    }
}