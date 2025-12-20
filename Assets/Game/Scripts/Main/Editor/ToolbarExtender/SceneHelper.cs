using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Game.Scripts.Main.Editor.ToolbarExtender
{
    internal static class SceneHelper
    {
        private static string _sceneToOpen;

        public static void StartScene(string sceneName)
        {
            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
            }

            _sceneToOpen = sceneName;
            EditorApplication.update += OnUpdate;
        }

        private static void OnUpdate()
        {
            if (_sceneToOpen == null ||
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
                var guids = AssetDatabase.FindAssets("t:scene " + _sceneToOpen, null);
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

            _sceneToOpen = null;
        }
    }
}