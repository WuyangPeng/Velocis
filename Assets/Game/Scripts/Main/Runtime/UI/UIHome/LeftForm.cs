using Game.Scripts.Main.Runtime.Procedure;
using Game.Scripts.Main.Runtime.UI.UICommon;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.UI.UIHome
{
    public class LeftForm : UGuiForm
    {
        private IProcedureFormHost _procedureHome;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            _procedureHome = (IProcedureFormHost)GetCurrentProcedure();
            if (_procedureHome == null)
            {
                Log.Warning("ProcedureHome is invalid when open LeftForm.");
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            _procedureHome = null;

            base.OnClose(isShutdown, userData);
        }

        public void OnFriendButtonClick()
        {
            _procedureHome.OpenUIForm(UIFormId.FriendForm);
        }

        public void OnTaskButtonClick()
        {
            _procedureHome.OpenUIForm(UIFormId.TaskForm);
        }

        public void OnSectButtonClick()
        {
            _procedureHome.OpenUIForm(UIFormId.SectForm);
        }
    }
}