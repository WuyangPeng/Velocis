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
        private const float Space = 8;
        private const float LargeSpace = 20;
        private const float ButtonWidth = 32;
        private const float DropdownWidth = 80;
        private const float PlayPauseStopWidth = 140;
        private static readonly int ToolCount;
        private static GUIStyle _commandStyle;

        public static readonly List<Action> LeftToolbarGUI = new();
        private static readonly List<Action> RightToolbarGUI = new();

        static ToolbarExtender()
        {
            var toolbarType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.Toolbar");

            const string fieldName = "k_ToolCount";

            var toolIcons = toolbarType.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

            ToolCount = toolIcons != null ? (int)toolIcons.GetValue(null) : 8;

            ToolbarCallback.OnToolbarGUI = OnGUI;
            ToolbarCallback.OnToolbarGUILeft = GUILeft;
            ToolbarCallback.OnToolbarGUIRight = GUIRight;
        }

        private static void OnGUI()
        {
            _commandStyle ??= new GUIStyle("CommandLeft");

            var screenWidth = EditorGUIUtility.currentViewWidth;
            var playButtonsPosition = Mathf.RoundToInt((screenWidth - PlayPauseStopWidth) / 2);

            var leftRect = GetLeftRect(screenWidth, playButtonsPosition);
            var rightRect = GetRightRect(screenWidth, playButtonsPosition);

            DrawLeftGUI(leftRect);
            DrawRightGUI(rightRect);
        }

        private static Rect GetLeftRect(float screenWidth, float playButtonsPosition)
        {
            var rect = new Rect(0, 0, screenWidth, Screen.height);
            rect.xMin += Space; // 左侧间距
            rect.xMin += ButtonWidth * ToolCount; // 工具按钮
            rect.xMin += Space; // 工具和枢轴之间的间距
            rect.xMin += 64 * 2; // 枢轴按钮
            rect.xMax = playButtonsPosition;

            // 在现有控件周围添加间距
            rect.xMin += Space;
            rect.xMax -= Space;

            // 添加上下边距
            rect.y = 4;
            rect.height = 22;

            return rect;
        }

        private static Rect GetRightRect(float screenWidth, float playButtonsPosition)
        {
            var rect = new Rect(playButtonsPosition, screenWidth, screenWidth, Screen.height);
            rect.xMin += _commandStyle.fixedWidth * 3; // 播放按钮
            rect.xMax -= Space; // 右侧间距
            rect.xMax -= DropdownWidth; // 布局
            rect.xMax -= Space; // 布局和图层之间的间距
            rect.xMax -= DropdownWidth; // 图层
            rect.xMax -= Space; // 图层和账户之间的间距
            rect.xMax -= DropdownWidth; // 账户
            rect.xMax -= Space; // 账户和Cloud之间的间距
            rect.xMax -= ButtonWidth; // Cloud
            rect.xMax -= Space; // cloud和collab之间的间距
            rect.xMax -= 78; // Colab

            // 在现有控件周围添加间距
            rect.xMin += Space;
            rect.xMax -= Space;

            // 添加上下边距
            rect.y = 4;
            rect.height = 22;

            return rect;
        }

        private static void DrawLeftGUI(Rect rect)
        {
            if (rect.width <= 0)
            {
                return;
            }

            GUILayout.BeginArea(rect);
            GUILayout.BeginHorizontal();
            foreach (var handler in LeftToolbarGUI)
            {
                handler();
            }

            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private static void DrawRightGUI(Rect rect)
        {
            if (rect.width <= 0)
            {
                return;
            }

            GUILayout.BeginArea(rect);
            GUILayout.BeginHorizontal();
            foreach (var handler in RightToolbarGUI)
            {
                handler();
            }

            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private static void GUILeft()
        {
            GUILayout.BeginHorizontal();
            foreach (var handler in LeftToolbarGUI)
            {
                handler();
            }

            GUILayout.EndHorizontal();
        }

        private static void GUIRight()
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