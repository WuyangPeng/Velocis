using Game.Scripts.Main.Runtime.GameModule.Role;
using Game.Scripts.Main.Runtime.Procedure.Scene;
using Game.Scripts.Main.Runtime.UI.UICommon;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.UI.UIHome
{
    public class ChangeNameForm : UGuiForm
    {
        [SerializeField] private InputField surname;
        [SerializeField] private InputField name;
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
            surname.text = roleModule.GetSurname();
            name.text = roleModule.GetName();
        }


        protected override void OnClose(bool isShutdown, object userData)
        {
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
            roleModule.ChangeName(surname.text, name.text);
        }

        public void Return()
        {
            procedureHome.RemoveUIForm(UIFormId.ChangeNameForm);
        }
    }
}