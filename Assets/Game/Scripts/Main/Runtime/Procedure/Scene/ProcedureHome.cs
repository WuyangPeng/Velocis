using System.Collections.Generic;
using Game.Scripts.Main.Runtime.Event;
using Game.Scripts.Main.Runtime.Game;
using Game.Scripts.Main.Runtime.UI.UICommon;
using Game.Scripts.Main.Runtime.UI.UIForm;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Game.Scripts.Main.Runtime.Procedure.Scene
{
    public class ProcedureHome : ProcedureBase
    {
        private readonly FormComponent _formComponent = new();

        private readonly Dictionary<GameMode, GameBase> _games = new();

        private GameBase _currentGame;


        private float _gotoMenuDelaySeconds;

        public override bool UseNativeDialog => false;

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);

            _games.Add(GameMode.Survival, new SurvivalGame());
        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);

            _games.Clear();
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);


            var gameMode = (GameMode)procedureOwner.GetData<VarByte>("GameMode").Value;
            _currentGame = _games[gameMode];
            _currentGame.Initialize();

            _formComponent.AddForm(UIFormId.BottomForm);
            _formComponent.AddForm(UIFormId.UpperForm);
            _formComponent.AddForm(UIFormId.LeftForm);
            _formComponent.AddForm(UIFormId.RightForm);

            _formComponent.OnEnter(procedureOwner);

            GameEntry.Event.Subscribe(ServerListEventArgs.EventId, OnServerListClose);
        }

        private void OnServerListClose(object sender, GameEventArgs e)
        {
            RemoveUIForm(UIFormId.ServerListForm);
        }

        public void OpenUIForm(UIFormId form)
        {
            _formComponent.OpenUIForm(form);
        }

        public void RemoveUIForm(UIFormId formId)
        {
            _formComponent.RemoveUIForm(formId);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            GameEntry.Event.Unsubscribe(ServerListEventArgs.EventId, OnServerListClose);

            if (_currentGame != null)
            {
                _currentGame.Shutdown();
                _currentGame = null;
            }

            base.OnLeave(procedureOwner, isShutdown);

            _formComponent.OnLeave(procedureOwner, isShutdown);
        }


        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);


            if (_currentGame is { GameOver: false })
            {
                _currentGame.Update(elapseSeconds, realElapseSeconds);
            }
        }
    }
}