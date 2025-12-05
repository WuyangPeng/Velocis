using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Game.Scripts.Main.Editor.ToolbarExtender
{
    [InitializeOnLoad]
    public static class ToolbarExtender
    {
        private static readonly int ToolCount;
        private static GUIStyle s_CommandStyle;

        public static readonly List<Action> LeftToolbarGUI = new();
        public static readonly List<Action> RightToolbarGUI = new();

        public const float Space = 8;
        public const float LargeSpace = 20;
        public const float ButtonWidth = 32;
        public const float DropdownWidth = 80;
        public const float PlayPauseStopWidth = 140;

        static ToolbarExtender()
        {
            var toolbarType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.Toolbar");

            const string fieldName = "k_ToolCount";

            var toolIcons = toolbarType.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

            ToolCount = toolIcons != null ? ((int)toolIcons.GetValue(null)) : 8;

            ToolbarCallback.OnToolbarGUI = OnGUI;
            ToolbarCallback.OnToolbarGUILeft = GUILeft;
            ToolbarCallback.OnToolbarGUIRight = GUIRight;
        }

        private static void OnGUI()
        {
            s_CommandStyle ??= new GUIStyle("CommandLeft");

            var screenWidth = EditorGUIUtility.currentViewWidth;

            float playButtonsPosition = Mathf.RoundToInt((screenWidth - PlayPauseStopWidth) / 2);

            var leftRect = new Rect(0, 0, screenWidth, Screen.height);
            leftRect.xMin += Space; // 左侧间距
            leftRect.xMin += ButtonWidth * ToolCount; // 工具按钮

            leftRect.xMin += Space; // 工具和枢轴之间的间距

            leftRect.xMin += 64 * 2; // 枢轴按钮
            leftRect.xMax = playButtonsPosition;

            var rightRect = new Rect(playButtonsPosition, screenWidth, screenWidth, Screen.height);

            rightRect.xMin += s_CommandStyle.fixedWidth * 3; // 播放按钮

            rightRect.xMax -= Space; // 右侧间距
            rightRect.xMax -= DropdownWidth; // 布局
            rightRect.xMax -= Space; // 布局和图层之间的间距
            rightRect.xMax -= DropdownWidth; // 图层

            rightRect.xMax -= Space; // 图层和账户之间的间距

            rightRect.xMax -= DropdownWidth; // 账户
            rightRect.xMax -= Space; // 账户和Cloud之间的间距
            rightRect.xMax -= ButtonWidth; // Cloud
            rightRect.xMax -= Space; // cloud和collab之间的间距
            rightRect.xMax -= 78; // Colab

            // 在现有控件周围添加间距
            leftRect.xMin += Space;
            leftRect.xMax -= Space;
            rightRect.xMin += Space;
            rightRect.xMax -= Space;

            // 添加上下边距
            leftRect.y = 4;
            leftRect.height = 22;
            rightRect.y = 4;
            rightRect.height = 22;

            if (leftRect.width > 0)
            {
                GUILayout.BeginArea(leftRect);
                GUILayout.BeginHorizontal();
                foreach (var handler in LeftToolbarGUI)
                {
                    handler();
                }

                GUILayout.EndHorizontal();
                GUILayout.EndArea();
            }

            if (rightRect.width <= 0) return;

            GUILayout.BeginArea(rightRect);
            GUILayout.BeginHorizontal();
            foreach (var handler in RightToolbarGUI)
            {
                handler();
            }

            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        public static void GUILeft()
        {
            GUILayout.BeginHorizontal();
            foreach (var handler in LeftToolbarGUI)
            {
                handler();
            }
            GUILayout.EndHorizontal();
        }

        public static void GUIRight()
        {
            GUILayout.BeginHorizontal();
            foreach (var handler in RightToolbarGUI)
            {
                handler();
            }
            GUILayout.EndHorizontal();
        }
    }
}