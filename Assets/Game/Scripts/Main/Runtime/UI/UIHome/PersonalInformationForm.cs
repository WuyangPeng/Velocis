using Game.Scripts.Main.Runtime.GameModule.Role;
using Game.Scripts.Main.Runtime.GameModule.User;
using Game.Scripts.Main.Runtime.Procedure.Scene;
using Game.Scripts.Main.Runtime.UI.UICommon;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.UI.UIHome
{
    public class PersonalInformationForm : UGuiForm
    {
        [SerializeField] private Text roleName;

        [SerializeField] private Text userId;

        [SerializeField] private Text serverName;

        [SerializeField] private Text version;

        private ProcedureHome procedureHome;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            procedureHome = (ProcedureHome)GetCurrentProcedure();
            if (procedureHome == null)
            {
                Log.Warning("ProcedureHome is invalid when open UpperForm.");
            }

            SetText();

            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            userId.text = userModule.GetUserId().ToString();

            var accountModule = GameEntry.ModuleComponent.GetModule<AccountModule>();
            serverName.text = accountModule.GetCurrentGameServerName();
            version.text = "1.0.0";
        }

        public void SetText()
        {
            var roleModule = GameEntry.ModuleComponent.GetModule<RoleModule>();
            roleName.text = roleModule.GetName();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            procedureHome = null;

            base.OnClose(isShutdown, userData);
        }

        public void OnReturnButtonClick()
        {
            procedureHome.RemoveUIForm(UIFormId.PersonalInformationForm);
        }

        public void OnSetButtonClick()
        {
            procedureHome.OpenUIForm(UIFormId.SettingForm);
        }

        public void OnServerListButtonClick()
        {
            procedureHome.OpenUIForm(UIFormId.ServerListForm);
        }
    }
}