using GameFramework.Debugger;
using GameFramework.Localization;
using UnityEngine;
using UnityGameFramework.Runtime;
using Constant = Game.Scripts.Main.Runtime.Definition.Constant.Constant;

namespace Game.Scripts.Main.Runtime.Debugger
{
    public class ChangeLanguageDebuggerWindow : IDebuggerWindow
    {
        private Vector2 scrollPosition = Vector2.zero;
        private bool needRestart;

        public void Initialize(params object[] args)
        {
        }

        public void Shutdown()
        {
        }

        public void OnEnter()
        {
        }

        public void OnLeave()
        {
        }

        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            if (!needRestart)
            {
                return;
            }

            needRestart = false;
            GameEntry.Shutdown(ShutdownType.Restart);
        }

        public void OnDraw()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            {
                DrawSectionChangeLanguage();
            }
            GUILayout.EndScrollView();
        }

        private void DrawSectionChangeLanguage()
        {
            GUILayout.Label("<b>Change Language</b>");
            GUILayout.BeginHorizontal("box");
            {
                if (GUILayout.Button("Chinese Simplified", GUILayout.Height(30)))
                {
                    Base.GameEntry.Localization.Language = Language.ChineseSimplified;
                    SaveLanguage();
                }
                if (GUILayout.Button("Chinese Traditional", GUILayout.Height(30)))
                {
                    Base.GameEntry.Localization.Language = Language.ChineseTraditional;
                    SaveLanguage();
                }
                if (GUILayout.Button("English", GUILayout.Height(30)))
                {
                    Base.GameEntry.Localization.Language = Language.English;
                    SaveLanguage();
                }
            }
            GUILayout.EndHorizontal();
        }

        private void SaveLanguage()
        {
            Base.GameEntry.Setting.SetString(Constant.Setting.Language, Base.GameEntry.Localization.Language.ToString());
            Base.GameEntry.Setting.Save();
            needRestart = true;
        }
    }
}
