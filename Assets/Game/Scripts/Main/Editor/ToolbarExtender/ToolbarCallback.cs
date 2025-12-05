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

        private static ScriptableObject s_CurrentToolbar;

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
            if (s_CurrentToolbar != null) return;

            var toolbars = Resources.FindObjectsOfTypeAll(ToolbarType);

            s_CurrentToolbar = toolbars.Length > 0 ? (ScriptableObject)toolbars[0] : null;
            if (s_CurrentToolbar == null) return;

            var root = s_CurrentToolbar.GetType().GetField("m_Root", BindingFlags.NonPublic | BindingFlags.Instance);
            if (root == null) return;

            var rawRoot = root.GetValue(s_CurrentToolbar);
            var mRoot = rawRoot as VisualElement;
            RegisterCallback("ToolbarZoneLeftAlign", OnToolbarGUILeft);
            RegisterCallback("ToolbarZoneRightAlign", OnToolbarGUIRight);
            return;

            void RegisterCallback(string rootName, Action cb)
            {
                var toolbarZone = mRoot.Q(rootName);

                var parent = new VisualElement()
                {
                    style = {
                            flexGrow = 1,
                            flexDirection = FlexDirection.Row,
                        }
                };
                var container = new IMGUIContainer();
                container.style.flexGrow = 1;
                container.onGUIHandler += () =>
                {
                    cb?.Invoke();
                };
                parent.Add(container);
                toolbarZone.Add(parent);
            }
        }

        private static void OnGUI()
        {
            OnToolbarGUI?.Invoke();
        }
    }
}
