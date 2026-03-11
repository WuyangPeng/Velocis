using Game.Scripts.Main.Runtime.Event;
using Game.Scripts.Main.Runtime.UI.UICommon;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.UI.UIMenu
{
    public class ServerListForm : UGuiForm
    {
        private ProcedureBase _procedureBase;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            _procedureBase = GetCurrentProcedure();
            if (_procedureBase == null)
            {
                Log.Warning("ProcedureBase is invalid when open ServerListForm.");
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            _procedureBase = null;

            base.OnClose(isShutdown, userData);
        }

        public void OnReturnButtonClick()
        {
            GameEntry.Event.Fire(this, ServerListEventArgs.Create());
        }
    }
}