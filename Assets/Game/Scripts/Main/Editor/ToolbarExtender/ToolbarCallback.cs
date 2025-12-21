using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Scripts.Main.Editor.ToolbarExtender
{
    public static class ToolbarCallback
    {
        private static readonly Type ToolbarType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.Toolbar");
        private static readonly Type GuiViewType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.GUIView");

        private static readonly Type WindowBackendType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.IWindowBackend");

        private static readonly PropertyInfo WindowBackend = GuiViewType.GetProperty("windowBackend", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        private static readonly PropertyInfo ViewVisualTree = WindowBackendType.GetProperty("visualTree", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        private static readonly FieldInfo ImguiContainerOnGui = typeof(IMGUIContainer).GetField("m_OnGUIHandler", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        private static ScriptableObject _currentToolbar;

        public static Action OnToolbarGUI;
        public static Action OnToolbarGUILeft;
        public static Action OnToolbarGUIRight;

        static ToolbarCallback()
        {
            EditorApplication.update -= OnUpdate;
            EditorApplication.update += OnUpdate;
        }

        private static void OnUpdate()
        {
            if (_currentToolbar != null)
            {
                return;
            }

            FindToolbar();

            if (_currentToolbar != null)
            {
                SetupToolbar();
            }
        }

        private static void FindToolbar()
        {
            var toolbars = Resources.FindObjectsOfTypeAll(ToolbarType);
            _currentToolbar = toolbars.Length > 0 ? (ScriptableObject)toolbars[0] : null;
        }

        private static void SetupToolbar()
        {
            var root = _currentToolbar.GetType().GetField("m_Root", BindingFlags.NonPublic | BindingFlags.Instance);
            if (root == null)
            {
                return;
            }

            var rawRoot = root.GetValue(_currentToolbar);
            var visualElement = rawRoot as VisualElement;
            RegisterCallback(visualElement, "ToolbarZoneLeftAlign", OnToolbarGUILeft);
            RegisterCallback(visualElement, "ToolbarZoneRightAlign", OnToolbarGUIRight);
        }

        private static void RegisterCallback(VisualElement visualElement, string rootName, Action cb)
        {
            var toolbarZone = visualElement.Q(rootName);

            var parent = new VisualElement
            {
                style =
                {
                    flexGrow = 1,
                    flexDirection = FlexDirection.Row
                }
            };
            var container = new IMGUIContainer();
            container.style.flexGrow = 1;
            container.onGUIHandler += () => { cb?.Invoke(); };
            parent.Add(container);
            toolbarZone.Add(parent);
        }

        private static void OnGUI()
        {
            OnToolbarGUI?.Invoke();
        }
    }
}