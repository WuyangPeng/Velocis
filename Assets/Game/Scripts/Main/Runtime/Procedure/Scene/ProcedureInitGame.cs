using Game.Scripts.Main.Runtime.GameEnum;
using Game.Scripts.Main.Runtime.GameModule.User;
using Game.Scripts.Main.Runtime.InitGame;
using Game.Scripts.Main.Runtime.LoadGame;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Game.Scripts.Main.Runtime.Procedure.Scene
{
    public class ProcedureInitGame : ProcedureBase
    {
        private const float DelayedSeconds = 2f;

        private float _gotoHomeDelaySeconds;

        private InitGameType _initGameType = InitGameType.Begin;
        public override bool UseNativeDialog => false;


        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            _gotoHomeDelaySeconds += elapseSeconds;

            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            if (userModule.IsInitWorld())
            {
                var loadGameBase = LoadGameBase.Create(_initGameType);
                loadGameBase.LoadGame();
            }
            else
            {
                var initGame = InitGameBase.Create(_initGameType);
                initGame.InitGame();
            }

            if (_initGameType < InitGameType.End)
            {
                ++_initGameType;
            }

            if (_gotoHomeDelaySeconds < DelayedSeconds)
            {
                return;
            }

            if (_initGameType < InitGameType.End)
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