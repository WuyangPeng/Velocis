using Game.Scripts.Main.Runtime.GameModule.User;
using Game.Scripts.Main.Runtime.Procedure.Scene;
using Game.Scripts.Main.Runtime.UI.UICommon;
using Game.Scripts.Main.Runtime.UIDisplay.UIMenu;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.UI.UIMenu
{
    public class LoadForm : UGuiForm
    {
        private ProcedureMenu procedureMenu;

        [SerializeField]
        private HeadDataDisplay headDataDisplay;

        public void OnReturnButtonClick()
        {
            procedureMenu.RemoveUIForm(UIFormId.LoadForm);
        }

        public void OnEnterButtonClick(int index)
        {
            if (procedureMenu.HasHeadData(index))
            {
                var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
                userModule.SetInitWorld();

                procedureMenu.LoadGame();
            }
            else
            {
                var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
                userModule.SetSaveIndex(index);

                procedureMenu.StartGame();
            }
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            procedureMenu = (ProcedureMenu)GetCurrentProcedure();

            if (procedureMenu == null)
            {
                Log.Warning("ProcedureMenu is invalid when open LoadForm.");
                return;
            }

            procedureMenu.LoadHeadData();

            var headData = procedureMenu.GetHeadData();

            headDataDisplay.Refresh(headData);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            procedureMenu = null;

            base.OnClose(isShutdown, userData);

            headDataDisplay.ReleaseAsset();
        }
    }
}