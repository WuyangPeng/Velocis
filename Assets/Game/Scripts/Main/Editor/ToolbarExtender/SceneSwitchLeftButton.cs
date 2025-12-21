using UnityEditor;
using UnityEngine;

namespace Game.Scripts.Main.Editor.ToolbarExtender
{
    [InitializeOnLoad]
    public class SceneSwitchLeftButton
    {
        private const string SceneName = "Velocis";

        private const string ButtonStyleName = "Tab middle";
        private static GUIStyle _buttonGuiStyle;

        static SceneSwitchLeftButton()
        {
            ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
        }

        private static void OnToolbarGUI()
        {
            _buttonGuiStyle ??= new GUIStyle(ButtonStyleName)
            {
                padding = new RectOffset(2, 8, 2, 2),
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold
            };

            GUILayout.FlexibleSpace();
            if (GUILayout.Button(new GUIContent("Launcher", EditorGUIUtility.FindTexture("PlayButton"), "Start Scene Launcher"), _buttonGuiStyle))
            {
                SceneHelper.StartScene(SceneName);
            }
        }
    }
}