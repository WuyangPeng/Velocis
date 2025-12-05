using Game.Scripts.Main.Runtime.GameEnum;
using Game.Scripts.Main.Runtime.GameModule.User;
using Game.Scripts.Main.Runtime.InitGame;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Game.Scripts.Main.Runtime.Procedure.Scene
{
    public class ProcedureInitGame : ProcedureBase
    {
        public override bool UseNativeDialog => false;

        private const float DelayedSeconds = 2f;

        private float gotoHomeDelaySeconds;

        private InitGameType initGameType = InitGameType.Begin;

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            gotoHomeDelaySeconds += elapseSeconds;

            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            if (userModule.IsInitWorld())
            {
                var loadGameBase = LoadGame.LoadGameBase.Create(initGameType);
                loadGameBase.LoadGame();
            }
            else
            {
                var initGame = InitGameBase.Create(initGameType);
                initGame.InitGame();
            }

            if (initGameType < InitGameType.End)
            {
                ++initGameType;
            }

            if (gotoHomeDelaySeconds < DelayedSeconds)
            {
                return;
            }

            if (initGameType < InitGameType.End)
            {
                return;
            }

            if (!userModule.IsInitWorld())
            {
                for (var i = InitGameType.Begin; i <= InitGameType.End; ++i)
                {
                    var initGame = InitGameBase.Create(i);
                    initGame.SaveGame();
                }
            }

            procedureOwner.SetData<VarInt32>("NextSceneId", GameEntry.Config.GetInt("Scene.Home"));
            ChangeState<ProcedureChangeScene>(procedureOwner);
        }
    }
}