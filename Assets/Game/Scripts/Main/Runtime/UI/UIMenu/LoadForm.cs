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
        [SerializeField] private HeadDataDisplay headDataDisplay;

        private ProcedureMenu _procedureMenu;

        public void OnReturnButtonClick()
        {
            _procedureMenu.RemoveUIForm(UIFormId.LoadForm);
        }

        public void OnEnterButtonClick(int index)
        {
            if (_procedureMenu.HasHeadData(index))
            {
                var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
                userModule.SetInitWorld();

                _procedureMenu.LoadGame();
            }
            else
            {
                var userModule = GameEntry.ModuleComponent.GetModule<UserModule>();
                userModule.SetSaveIndex(index);

                _procedureMenu.StartGame();
            }
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            _procedureMenu = (ProcedureMenu)GetCurrentProcedure();

            if (_procedureMenu == null)
            {
                Log.Warning("ProcedureMenu is invalid when open LoadForm.");
                return;
            }

            _procedureMenu.LoadHeadData();

            var headData = _procedureMenu.GetHeadData();

            headDataDisplay.Refresh(headData);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            _procedureMenu = null;

            base.OnClose(isShutdown, userData);

            headDataDisplay.ReleaseAsset();
        }
    }
}