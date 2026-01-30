using Game.Scripts.Main.Runtime.Procedure.Scene;
using Game.Scripts.Main.Runtime.UI.UICommon;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.UI.UIHome
{
    public class DebugForm : UGuiForm
    {
        [SerializeField] private InputField idInputField;
        [SerializeField] private InputField parameterInputField;

        private ProcedureHome procedureHome;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            procedureHome = (ProcedureHome)GetCurrentProcedure();
            if (procedureHome == null)
            {
                Log.Warning("ProcedureHome is invalid when open DebugForm.");
            }
        }


        protected override void OnClose(bool isShutdown, object userData)
        {
            procedureHome = null;

            base.OnClose(isShutdown, userData);
        }

        public void OnReturnButtonClick()
        {
            procedureHome.RemoveUIForm(UIFormId.DebugForm);
        }
        
        public void OnDebugButtonClick()
        {
             
        }
    }
}