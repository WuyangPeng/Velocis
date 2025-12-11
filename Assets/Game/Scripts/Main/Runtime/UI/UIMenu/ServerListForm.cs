using Game.Scripts.Main.Runtime.Procedure.Scene;
using Game.Scripts.Main.Runtime.UI.UICommon;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.UI.UIMenu
{
    public class ServerListForm : UGuiForm
    {
        private ProcedureMenu _procedureMenu;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            _procedureMenu = (ProcedureMenu)GetCurrentProcedure();
            if (_procedureMenu == null)
            {
                Log.Warning("ProcedureMenu is invalid when open ServerListForm.");
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            _procedureMenu = null;

            base.OnClose(isShutdown, userData);
        }

        public void OnReturnButtonClick()
        {
            _procedureMenu.RemoveUIForm(UIFormId.ServerListForm);
        }
    }
}