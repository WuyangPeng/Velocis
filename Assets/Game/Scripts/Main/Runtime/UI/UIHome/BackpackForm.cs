using Game.Scripts.Main.Runtime.Procedure;
using Game.Scripts.Main.Runtime.UI.UICommon;
using Game.Scripts.Main.Runtime.UI.UIItem;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.UI.UIHome
{
    public class BackpackForm : UGuiForm
    {
        [SerializeField] private UIItemIcon uiItemIcon1;
        private IProcedureFormHost _procedureHome;


        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            _procedureHome = (IProcedureFormHost)GetCurrentProcedure();
            if (_procedureHome == null)
            {
                Log.Warning("ProcedureHome is invalid when open DebugForm.");
            }

            if (uiItemIcon1 != null)
            {
//                var config = GameEntry.GameConfig.GetGameConfig().GetTables().ItemConfigContainer.GetOrDefault(1001001);
//                uiItemIcon1.SetData(config, 10000);
            }
        }


        protected override void OnClose(bool isShutdown, object userData)
        {
            _procedureHome = null;

            base.OnClose(isShutdown, userData);
        }

        public void OnReturnButtonClick()
        {
            _procedureHome.RemoveUIForm(UIFormId.BackpackForm);
        }
    }
}