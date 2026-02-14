using Game.Scripts.Main.Runtime.Event;
using Game.Scripts.Main.Runtime.Procedure.Scene;
using Game.Scripts.Main.Runtime.UI.UICommon;
using GameFramework.Event;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.UI.UIMenu
{
    public class LoginLoadForm : UGuiForm
    {
        private const float ProgressSpeed = 0.9f; // 每秒增长速度

        [SerializeField] private Slider progressSlider;
        
        private float _currentProgress;
        private ProcedureMenu _procedureMenu;
        private float _targetProgress;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            progressSlider.value = 0f;
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            _procedureMenu = (ProcedureMenu)GetCurrentProcedure();

            if (_procedureMenu == null)
            {
                Log.Warning("ProcedureMenu is invalid when open LoadForm.");
            }

            _currentProgress = 0f;
            _targetProgress = 0f;

            progressSlider.value = 0f;

            GameEntry.Event.Subscribe(LoginProgressEventArgs.EventId, OnLoginProgress);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            GameEntry.Event.Unsubscribe(LoginProgressEventArgs.EventId, OnLoginProgress);

            _procedureMenu = null;

            base.OnClose(isShutdown, userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (_currentProgress >= _targetProgress && _currentProgress <= 1.0f)
            {
                return;
            }

            _currentProgress = Mathf.MoveTowards(_currentProgress, _targetProgress, ProgressSpeed * elapseSeconds);

            progressSlider.value = _currentProgress;

            if (_currentProgress >= 1.0f && _procedureMenu != null)
            {
                _procedureMenu.StartGame();
            }
        }

        private void OnLoginProgress(object sender, GameEventArgs e)
        {
            var eventArgs = (LoginProgressEventArgs)e;
            _targetProgress = eventArgs.Progress;
        }
    }
}