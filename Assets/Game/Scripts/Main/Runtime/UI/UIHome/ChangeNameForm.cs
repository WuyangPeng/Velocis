using Game.Scripts.Main.Runtime.Event;
using Game.Scripts.Main.Runtime.GameModule.Role;
using Game.Scripts.Main.Runtime.Procedure.Scene;
using Game.Scripts.Main.Runtime.UI.UICommon;
using GameFramework.Event;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.UI.UIHome
{
    public class ChangeNameForm : UGuiForm
    {
        [SerializeField] private TMP_InputField surnameInputField;
        [SerializeField] private TMP_InputField nameInputField;
        private ProcedureHome procedureHome;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            procedureHome = (ProcedureHome)GetCurrentProcedure();
            if (procedureHome == null)
            {
                Log.Warning("ProcedureHome is invalid when open ChangeNameForm.");
            }

            var roleModule = GameEntry.ModuleComponent.GetModule<RoleModule>();
            surnameInputField.text = roleModule.GetSurname();
            nameInputField.text = roleModule.GetName();

            GameEntry.Event.Subscribe(ChangeNameEventArgs.EventId, OnSetTextSuccess);
        }

        private void OnSetTextSuccess(object sender, GameEventArgs e)
        {
            procedureHome.RemoveUIForm(UIFormId.ChangeNameForm);
        }


        protected override void OnClose(bool isShutdown, object userData)
        {
            GameEntry.Event.Unsubscribe(ChangeNameEventArgs.EventId, OnSetTextSuccess);

            procedureHome = null;

            base.OnClose(isShutdown, userData);
        }

        public void OnReturnButtonClick()
        {
            procedureHome.RemoveUIForm(UIFormId.ChangeNameForm);
        }

        public void OnChangeNameButtonClick()
        {
            var roleModule = GameEntry.ModuleComponent.GetModule<RoleModule>();
            roleModule.ChangeName(surnameInputField.text, nameInputField.text);
        }
    }
}