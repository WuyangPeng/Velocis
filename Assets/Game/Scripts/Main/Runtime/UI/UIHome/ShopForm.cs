using Game.Scripts.Main.Runtime.Procedure;
using Game.Scripts.Main.Runtime.UI.UICommon;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.UI.UIHome
{
    public class ShopForm : UGuiForm
    {
        private IProcedureFormHost _procedureHome;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            _procedureHome = (IProcedureFormHost)GetCurrentProcedure();
            if (_procedureHome == null)
            {
                Log.Warning("ProcedureHome is invalid when open ShopForm.");
            }
        }


        protected override void OnClose(bool isShutdown, object userData)
        {
            _procedureHome = null;

            base.OnClose(isShutdown, userData);
        }

        public void OnReturnButtonClick()
        {
            _procedureHome.RemoveUIForm(UIFormId.ShopForm);
        }
    }
}