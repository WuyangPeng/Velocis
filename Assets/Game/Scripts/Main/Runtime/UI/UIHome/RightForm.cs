using Game.Scripts.Main.Runtime.Event;
using Game.Scripts.Main.Runtime.GameModule.Debug;
using Game.Scripts.Main.Runtime.Procedure.Scene;
using Game.Scripts.Main.Runtime.UI.UICommon;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.UI.UIHome
{
    public class RightForm : UGuiForm
    {
        [SerializeField] private CommonButton commonButton;
        private ProcedureHome procedureHome;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            SetCommonButton();
            procedureHome = (ProcedureHome)GetCurrentProcedure();
            if (procedureHome == null)
            {
                Log.Warning("ProcedureHome is invalid when open RightForm.");
            }

            GameEntry.Event.Subscribe(DebugInfoEventArgs.EventId, OnDebugInfoChange);
        }

        private void OnDebugInfoChange(object sender, GameEventArgs e)
        {
            SetCommonButton();
        }

        private void SetCommonButton()
        {
            var debugModule = GameEntry.ModuleComponent.GetModule<DebugModule>();
            commonButton.gameObject.SetActive(debugModule.IsDebug);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            procedureHome = null;
            GameEntry.Event.Unsubscribe(DebugInfoEventArgs.EventId, OnDebugInfoChange);

            base.OnClose(isShutdown, userData);
        }

        public void OnDebugButtonClick()
        {
            procedureHome.OpenUIForm(UIFormId.DebugForm);
        }

        public void OnActivityButtonClick()
        {
            procedureHome.OpenUIForm(UIFormId.ActivityForm);
        }

        public void OnShopButtonClick()
        {
            procedureHome.OpenUIForm(UIFormId.ShopForm);
        }

        public void OnRankingButtonClick()
        {
            procedureHome.OpenUIForm(UIFormId.RankingForm);
        }


        public void OnMailButtonClick()
        {
            procedureHome.OpenUIForm(UIFormId.MailForm);
        }
    }
}