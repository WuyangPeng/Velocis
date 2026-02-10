using Game.Scripts.Main.Runtime.Procedure.Scene;
using Game.Scripts.Main.Runtime.UI.UICommon;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.UI.UIMenu
{
    public class LoginLoadForm : UGuiForm
    {
        private ProcedureMenu procedureMenu;


        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            procedureMenu = (ProcedureMenu)GetCurrentProcedure();

            if (procedureMenu == null)
            {
                Log.Warning("ProcedureMenu is invalid when open LoadForm.");
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            procedureMenu = null;

            base.OnClose(isShutdown, userData);
        }
    }
}