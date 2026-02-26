using Game.Scripts.Main.Runtime.Procedure.Scene;
using Game.Scripts.Main.Runtime.UI.UICommon;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.UI.UIHome
{
    public class CopyForm : UGuiForm
    {
        private ProcedureHome procedureHome;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            procedureHome = (ProcedureHome)GetCurrentProcedure();
            if (procedureHome == null)
            {
                Log.Warning("ProcedureHome is invalid when open CopyForm.");
            }
        }


        protected override void OnClose(bool isShutdown, object userData)
        {
            procedureHome = null;

            base.OnClose(isShutdown, userData);
        }

        public void OnReturnButtonClick()
        {
            procedureHome.RemoveUIForm(UIFormId.CopyForm);
        }
    }
}