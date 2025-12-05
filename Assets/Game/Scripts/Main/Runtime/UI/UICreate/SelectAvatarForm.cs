using Game.Scripts.Main.Runtime.GameEnum;
using Game.Scripts.Main.Runtime.GameModule.User;
using Game.Scripts.Main.Runtime.Procedure.Scene;
using Game.Scripts.Main.Runtime.UI.UICommon;
using Game.Scripts.Main.Runtime.UIDisplay.UICreate;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.UI.UICreate
{
    public class SelectAvatarForm : UGuiForm
    {
        private ProcedureCreate procedureCreate;

        [SerializeField]
        private GameSexDisplay gameSexDisplay;

        [SerializeField]
        private AvatarScrollDisplay avatarScrollDisplay;


        [SerializeField]
        private Toggle[] sexToggle;

        public void OnReturnButtonClick()
        {
            procedureCreate.RemoveUIForm(UIFormId.SelectAvatarForm);
        }

        public void OnEnterButtonClick()
        {
            procedureCreate.OpenUIForm(UIFormId.SelectCampForm);
        }

        public void OnSelectMaleButtonClick(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            userModule.SetSexType(SexType.Male);

            avatarScrollDisplay.Refresh();
        }

        public void OnSelectFemaleButtonClick(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
            userModule.SetSexType(SexType.Female);

            avatarScrollDisplay.Refresh();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            procedureCreate = (ProcedureCreate)GetCurrentProcedure();

            if (procedureCreate == null)
            {
                Log.Warning("ProcedureCreate is invalid when open SelectAvatarForm.");
            }

            gameSexDisplay.Refresh();

            var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();

            sexToggle[(int)(userModule.GetSexType() - 1)].isOn = true;
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            procedureCreate = null;

            base.OnClose(isShutdown, userData);
        }
    }
}