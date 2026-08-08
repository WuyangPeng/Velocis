// using Game.Scripts.Main.Runtime.Event;
// using Game.Scripts.Main.Runtime.GameModule.Debug;
using Game.Scripts.Main.Runtime.Procedure;
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
        private IProcedureFormHost _procedureHome;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            SetCommonButton();
            _procedureHome = (IProcedureFormHost)GetCurrentProcedure();
            if (_procedureHome == null)
            {
                Log.Warning("ProcedureHome is invalid when open RightForm.");
            }

//            GameEntry.Event.Subscribe(DebugInfoEventArgs.EventId, OnDebugInfoChange);
        }

        private void OnDebugInfoChange(object sender, GameEventArgs e)
        {
            SetCommonButton();
        }

        private void SetCommonButton()
        {
            // var debugModule = GameEntry.ModuleComponent.GetModule<DebugModule>();
            // commonButton.gameObject.SetActive(debugModule.IsDebug);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            _procedureHome = null;
//            GameEntry.Event.Unsubscribe(DebugInfoEventArgs.EventId, OnDebugInfoChange);

            base.OnClose(isShutdown, userData);
        }

        public void OnDebugButtonClick()
        {
            _procedureHome.OpenUIForm(UIFormId.DebugForm);
        }

        public void OnActivityButtonClick()
        {
            _procedureHome.OpenUIForm(UIFormId.ActivityForm);
        }

        public void OnShopButtonClick()
        {
            _procedureHome.OpenUIForm(UIFormId.ShopForm);
        }

        public void OnRankingButtonClick()
        {
            _procedureHome.OpenUIForm(UIFormId.RankingForm);
        }


        public void OnMailButtonClick()
        {
            _procedureHome.OpenUIForm(UIFormId.MailForm);
        }
    }
}