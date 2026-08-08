using Game.Scripts.Main.Runtime.Procedure;
using Game.Scripts.Main.Runtime.UI.UICommon;
using Game.Scripts.Main.Runtime.UIDisplay.UIMenu;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.UI.UIMenu
{
    public class LoadForm : UGuiForm
    {
        [SerializeField] private HeadDataDisplay headDataDisplay;

        private IProcedureMenuHost _procedureMenu;

        public void OnReturnButtonClick()
        {
            _procedureMenu.RemoveUIForm(UIFormId.LoadForm);
        }

        public void OnEnterButtonClick(int index)
        {
            /*  if (_procedureMenu.HasHeadData(index))
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
              }*/
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            _procedureMenu = (IProcedureMenuHost)GetCurrentProcedure();

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