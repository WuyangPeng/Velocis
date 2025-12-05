using Game.Scripts.Main.Runtime.SaveData;
using Game.Scripts.Main.Runtime.UI.UICommon;
using Game.Scripts.Main.Runtime.UI.UIForm;
using System.Text;
using Game.Scripts.Main.Runtime.GameModule.User;
using GameFramework;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Game.Scripts.Main.Runtime.Procedure.Scene
{
    public class ProcedureCreate : ProcedureBase
    {
        private readonly FormComponent formComponent = new FormComponent();

        public override bool UseNativeDialog => false;

        private int nextSceneId;

        public void EnterGame()
        {
            nextSceneId = GameEntry.Config.GetInt("Scene.InitGame");
        }

        public void ReturnMenu()
        {
            nextSceneId = GameEntry.Config.GetInt("Scene.Menu");
        }

        public void OpenUIForm(UIFormId form)
        {
            formComponent.OpenUIForm(form);
        }
        
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            formComponent.AddForm(UIFormId.SelectGameDifficultyForm);
            formComponent.OnEnter(procedureOwner);

            nextSceneId = 0;

            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            userModule.Init();
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            formComponent.OnLeave(procedureOwner, isShutdown);
        }


        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (nextSceneId == 0) return;

            procedureOwner.SetData<VarInt32>("NextSceneId", nextSceneId);
            ChangeState<ProcedureChangeScene>(procedureOwner);
        }

        public void RemoveUIForm(UIFormId formId)
        {
            formComponent.RemoveUIForm(formId);
        }

        public void SaveData()
        {
            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            var index = userModule.GetSaveIndex();
            var fileSystems = GameEntry.FileSystemComponent.CreateFileSystem("GameSaves/" + index, "HeadData.idx");

            var headData = new HeadSaveData
            {
                Index = index,
                Avatar = userModule.GetAvatarId(),
                GameDifficultyType = userModule.GetGameDifficultyType(),
                Name = userModule.GetFullName()
            };


            var json = Utility.Json.ToJson(headData);

            fileSystems.WriteFile("GameSaves", Encoding.UTF8.GetBytes(json));
        }
    }
}
