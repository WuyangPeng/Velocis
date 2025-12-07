using System;
using Game.Scripts.Main.Runtime.Login;
using Game.Scripts.Main.Runtime.Procedure.Scene;
using Game.Scripts.Main.Runtime.UI.UICommon;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.UI.UIMenu
{
    public class MenuForm : UGuiForm
    {
        [SerializeField]
        private GameObject quitButton = null;

        private ProcedureMenu procedureMenu = null; 

        public void OnStartButtonClick()
        {
            procedureMenu.OpenUIForm(UIFormId.LoadForm);
        }

        public async void OnGuestLoginButtonClick()
        {
            try
            {
                var loginController = new LoginController();
                var isSuccess = await loginController.GuestLoginAsync();

                if (isSuccess)
                {
                    // 登录成功，从成员变量获取 token
                    Log.Info($"Login successful. Token: {loginController.TokenResponse.token}");
                    GameEntry.UI.OpenUIForm(UIFormId.ServerListForm);
                }
                else
                {
                    // 登录失败，显示错误提示
                    var message = loginController.TokenResponse != null ? loginController.TokenResponse.message : GameEntry.Localization.GetString("Login.ConnectServerFailed");
                    GameEntry.UI.OpenDialog(new DialogParams
                    {
                        Mode = 1,
                        Title = GameEntry.Localization.GetString("Login.LoginFailed"),
                        Message = message,
                    });
                }
            }
            catch (Exception error)
            {
                // 捕获所有未预料到的异常，防止程序崩溃
                Log.Error("An unexpected error occurred in the login UI: " + error);
                GameEntry.UI.OpenDialog(new DialogParams
                {
                    Mode = 1,
                    Title = GameEntry.Localization.GetString("Login.LoginFailed"),
                    Message = GameEntry.Localization.GetString("Login.ConnectServerExceptional"),
                });
            }
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
                OnClickConfirm = delegate  { UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Quit); },
            });
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

            quitButton.SetActive(Application.platform != RuntimePlatform.IPhonePlayer);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            procedureMenu = null;

            base.OnClose(isShutdown, userData);
        }
    }
}
