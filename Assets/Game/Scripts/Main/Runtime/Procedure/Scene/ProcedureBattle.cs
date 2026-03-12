using System.Collections.Generic;
using Game.Scripts.Main.Runtime.Game;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Game.Scripts.Main.Runtime.Procedure.Scene
{
    public class ProcedureBattle : ProcedureBase
    {
        private const float GameOverDelayedSeconds = 2f;

        private readonly Dictionary<GameMode, GameBase> _games = new();
        private GameBase _currentGame;
        private bool _gotoMenu;
        private float _gotoMenuDelaySeconds;

        public override bool UseNativeDialog => false;

        public void GotoMenu()
        {
            _gotoMenu = true;
        }

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

            _gotoMenu = false;
            var gameMode = (GameMode)procedureOwner.GetData<VarByte>("GameMode").Value;
            _currentGame = _games[gameMode];
            _currentGame.Initialize();
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            if (_currentGame != null)
            {
                _currentGame.Shutdown();
                _currentGame = null;
            }

            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (_currentGame is { GameOver: false })
            {
                _currentGame.Update(elapseSeconds, realElapseSeconds);
                return;
            }

            if (!_gotoMenu)
            {
                _gotoMenu = true;
                _gotoMenuDelaySeconds = 0;
            }

            _gotoMenuDelaySeconds += elapseSeconds;
            if (!(_gotoMenuDelaySeconds >= GameOverDelayedSeconds))
            {
                return;
            }

            procedureOwner.SetData<VarInt32>("NextSceneId", GameEntry.Config.GetInt("Scene.Menu"));
            ChangeState<ProcedureChangeScene>(procedureOwner);
        }
    }
}