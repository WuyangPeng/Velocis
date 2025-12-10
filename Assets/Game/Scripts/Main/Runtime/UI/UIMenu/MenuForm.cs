using Game.Scripts.Main.Runtime.Login;
using Game.Scripts.Main.Runtime.Procedure.Scene;
using Game.Scripts.Main.Runtime.UI.UICommon;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.UI.UIMenu
{
    public class MenuForm : UGuiForm
    {
        [SerializeField] private CommonButton guestLoginButton;

        [SerializeField] private GameObject quitButton;

        [SerializeField] private InputField inputField;

        private LoginController loginController;

        private ProcedureMenu procedureMenu;

        private ServerListController serverListController;

        public string GetInputField()
        {
            return inputField.text;
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            procedureMenu = (ProcedureMenu)GetCurrentProcedure();
            if (procedureMenu == null)
            {
                Log.Warning("ProcedureMenu is invalid when open MenuForm.");
                return;
            }

            loginController = new LoginController(this);
            loginController.OnEnter();


            serverListController = new ServerListController(this);
            serverListController.OnEnter();

            quitButton.SetActive(Application.platform != RuntimePlatform.IPhonePlayer);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            loginController.OnLeave();
            serverListController.OnLeave();
            procedureMenu = null;

            base.OnClose(isShutdown, userData);
        }

        #region UI Event

        public void OnGuestLoginButtonClick()
        {
            guestLoginButton.enabled = false;
            loginController.GuestLogin();
        }

        public void OnSettingButtonClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.SettingForm);
        }

        public void OnAboutButtonClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.AboutForm);
        }

        public void OnQuitButtonClick()
        {
            GameEntry.UI.OpenDialog(new DialogParams
            {
                Mode = 2,
                Title = GameEntry.Localization.GetString("AskQuitGame.Title"),
                Message = GameEntry.Localization.GetString("AskQuitGame.Message"),
                OnClickConfirm = delegate { UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Quit); }
            });
        }

        #endregion

        #region Login Callbacks

        /// <summary>
        ///     由 LoginController 在登录成功时调用。
        /// </summary>
        public void OnLoginSuccess(TokenHttpResponse responseData)
        {
            Log.Info($"Login successful. Token: {responseData.token}");

            serverListController.ServerList();
        }

        /// <summary>
        ///     由 LoginController 在登录失败时调用。
        /// </summary>
        public void OnLoginFailure(string errorMessage)
        {
            // 重新启用按钮
            guestLoginButton.enabled = true;

            GameEntry.UI.OpenDialog(new DialogParams
            {
                Mode = 1,
                Title = GameEntry.Localization.GetString("Login.LoginFailed"),
                Message = string.IsNullOrEmpty(errorMessage)
                    ? GameEntry.Localization.GetString("Login.ConnectServerFailed")
                    : errorMessage
            });
        }

        #endregion

        #region ServerList Callbacks

        /// <summary>
        ///     由 LoginController 在登录成功时调用。
        /// </summary>
        public void OnServerListSuccess(LoginServersResponse responseData)
        {
            // 重新启用按钮
            guestLoginButton.enabled = true;

            procedureMenu.OpenUIForm(UIFormId.ServerListForm);
        }

        /// <summary>
        ///     由 LoginController 在登录失败时调用。
        /// </summary>
        public void OnServerListFailure(string errorMessage)
        {
            // 重新启用按钮
            guestLoginButton.enabled = true;

            GameEntry.UI.OpenDialog(new DialogParams
            {
                Mode = 1,
                Title = GameEntry.Localization.GetString("Login.LoginFailed"),
                Message = string.IsNullOrEmpty(errorMessage)
                    ? GameEntry.Localization.GetString("Login.ConnectServerFailed")
                    : errorMessage
            });
        }

        #endregion
    }
}