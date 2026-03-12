using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Scripts.Main.Runtime.Event;
using Game.Scripts.Main.Runtime.Game;
using Game.Scripts.Main.Runtime.GameModule.User;
using Game.Scripts.Main.Runtime.SaveData;
using Game.Scripts.Main.Runtime.UI.UICommon;
using Game.Scripts.Main.Runtime.UI.UIForm;
using GameFramework;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Game.Scripts.Main.Runtime.Procedure.Scene
{
    public class ProcedureMenu : ProcedureBase
    {
        private const int SaveMaxCount = 2;
        private readonly FormComponent _formComponent = new();
        private readonly List<HeadSaveData> _headData = new();
        private int _mNextSceneId;

        public override bool UseNativeDialog => false;

        public void LoadGame()
        {
            _mNextSceneId = GameEntry.Config.GetInt("Scene.InitGame");
        }

        public void StartGame()
        {
            _mNextSceneId = GameEntry.Config.GetInt("Scene.Home");
        }

        public void OpenUIForm(UIFormId form)
        {
            _formComponent.OpenUIForm(form);
        }

        public void RemoveUIForm(UIFormId form)
        {
            _formComponent.RemoveUIForm(form);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            _mNextSceneId = 0;

            _formComponent.AddForm(UIFormId.MenuForm);
            _formComponent.OnEnter(procedureOwner);

            GameEntry.ModuleComponent.ResetModule();

            LoadAccountData();

            GameEntry.Event.Subscribe(ServerListEventArgs.EventId, OnServerListClose);
            GameEntry.Event.Subscribe(LoginLoadEventArgs.EventId, OnLoginLoad);
            GameEntry.Event.Subscribe(NetworkClosedEventArgs.EventId, OnNetworkClosed);
        }

        private void OnLoginLoad(object sender, GameEventArgs e)
        {
            OpenUIForm(UIFormId.LoginLoadForm);
        }

        private void OnServerListClose(object sender, GameEventArgs e)
        {
            RemoveUIForm(UIFormId.ServerListForm);

            var accountModule = GameEntry.ModuleComponent.GetModule<AccountModule>();
            accountModule.Clear();
        }

        private static void LoadAccountData()
        {
            var accountModule = GameEntry.ModuleComponent.GetModule<AccountModule>();
            accountModule.Clear();
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            GameEntry.Event.Unsubscribe(ServerListEventArgs.EventId, OnServerListClose);
            GameEntry.Event.Unsubscribe(LoginLoadEventArgs.EventId, OnLoginLoad);
            GameEntry.Event.Unsubscribe(NetworkClosedEventArgs.EventId, OnNetworkClosed);

            base.OnLeave(procedureOwner, isShutdown);

            _formComponent.OnLeave(procedureOwner, isShutdown);
        }

        private void OnNetworkClosed(object sender, GameEventArgs e)
        {
            RemoveUIForm(UIFormId.LoginLoadForm);

            var accountModule = GameEntry.ModuleComponent.GetModule<AccountModule>();
            accountModule.ClearCurrentLogin();
        }


        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (_mNextSceneId <= 0)
            {
                return;
            }

            procedureOwner.SetData<VarInt32>("NextSceneId", _mNextSceneId);
            procedureOwner.SetData<VarByte>("GameMode", (byte)GameMode.Survival);
            ChangeState<ProcedureChangeScene>(procedureOwner);
        }

        public void LoadHeadData()
        {
            for (var i = 0; i < SaveMaxCount; ++i)
            {
                var fileSystems = GameEntry.FileSystemComponent.CreateFileSystem("GameSaves/" + i, "HeadData.idx");

                var bytes = fileSystems?.ReadFile("GameSaves");

                if (bytes == null)
                {
                    continue;
                }

                var json = Encoding.UTF8.GetString(bytes);
                var data = Utility.Json.ToObject<HeadSaveData>(json);
                _headData.Add(data);
            }
        }

        public bool HasHeadData(int index)
        {
            return _headData.Any(data => data.Index == index);
        }

        public List<HeadSaveData> GetHeadData()
        {
            return _headData;
        }
    }
}