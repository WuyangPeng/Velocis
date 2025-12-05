using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Game.Scripts.Main.Editor.ToolbarExtender
{
    [InitializeOnLoad]
    public class SceneSwitchLeftButton
    {
        private const string SceneName = "Velocis";

        private const string ButtonStyleName = "Tab middle";
        private static GUIStyle s_ButtonGuiStyle;

        static SceneSwitchLeftButton()
        {
            ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
        }

        private static void OnToolbarGUI()
        {
            s_ButtonGuiStyle ??= new GUIStyle(ButtonStyleName)
            {
                padding = new RectOffset(2, 8, 2, 2),
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold
            };

            GUILayout.FlexibleSpace();
            if (GUILayout.Button(new GUIContent("Launcher", EditorGUIUtility.FindTexture("PlayButton"), $"Start Scene Launcher"), s_ButtonGuiStyle))
            {
                SceneHelper.StartScene(SceneName);
            }
        }
    }

    internal static class SceneHelper
    {
        private static string s_SceneToOpen;

        public static void StartScene(string sceneName)
        {
            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
            }

            s_SceneToOpen = sceneName;
            EditorApplication.update += OnUpdate;
        }
        private static void OnUpdate()
        {
            if (s_SceneToOpen == null ||
                EditorApplication.isPlaying || 
                EditorApplication.isPaused ||
                EditorApplication.isCompiling || 
                EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }

            EditorApplication.update -= OnUpdate;

            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                var guids = AssetDatabase.FindAssets("t:scene " + s_SceneToOpen, null);
                if (guids.Length == 0)
                {
                    Debug.LogWarning("Couldn't find scene file");
                }
                else
                {
                    var scenePath = AssetDatabase.GUIDToAssetPath(guids[0]);
                    EditorSceneManager.OpenScene(scenePath);
                    EditorApplication.isPlaying = true;
                }
            }
            s_SceneToOpen = null;
        }
    }
}