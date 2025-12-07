using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Scripts.Main.Runtime.Game;
using Game.Scripts.Main.Runtime.GameModule.User;
using Game.Scripts.Main.Runtime.UI.UIMenu;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.Login
{
    public class LoginController
    {
        private readonly MenuForm menuForm;
        private int serialId;

        public LoginController(MenuForm menuForm)
        {
            this.menuForm = menuForm;
            serialId = 0;
        }

        /// <summary>
        ///     执行游客登录。结果将通过回调传递给 MenuForm。
        /// </summary>
        public void GuestLogin()
        {
            if (serialId != 0)
            {
                Log.Warning("Login request is already in progress.");
                return;
            }

            var url = GameEntry.Account.guestLoginUrl;
            var device_id = menuForm.GetInputField();
            var queryParams = new Dictionary<string, string>
            {
                { "device_id", string.IsNullOrEmpty(device_id) ? SystemInfo.deviceUniqueIdentifier : device_id }
            };
            var queryString = string.Join("&",
                queryParams.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));
            var finalUri = $"{url}?{queryString}";

            // 订阅事件
            GameEntry.Event.Subscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
            GameEntry.Event.Subscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFailure);

            // 发送请求并保存序列号
            serialId = GameEntry.WebRequest.AddWebRequest(finalUri, this);

            if (serialId == 0)
            {
                Log.Error("Failed to add web request to the queue.");
                CleanUp();
                menuForm.OnLoginFailure(GameEntry.Localization.GetString("Login.ConnectServerFailed"));
            }
        }

        private void OnWebRequestSuccess(object sender, GameEventArgs args)
        {
            try
            {
                var webRequestSuccessEventArgs = (WebRequestSuccessEventArgs)args;
                if (webRequestSuccessEventArgs.SerialId != serialId)
                {
                    return;
                }

                Success(webRequestSuccessEventArgs);
            }
            catch (Exception error)
            {
                Log.Error("An exception occurred during web request success handling: " + error);
                menuForm.OnLoginFailure(GameEntry.Localization.GetString("Login.ConnectServerExceptional"));
            }
            finally
            {
                CleanUp();
            }
        }

        private void Success(WebRequestSuccessEventArgs webRequestSuccessEventArgs)
        {
            var responseJson = Encoding.UTF8.GetString(webRequestSuccessEventArgs.GetWebResponseBytes());
            if (string.IsNullOrEmpty(responseJson))
            {
                Log.Error("Guest login request failed: The response was null or empty.");
                menuForm.OnLoginFailure(GameEntry.Localization.GetString("Login.ConnectServerFailed"));
            }
            else
            {
                var tokenResponse = JsonUtility.FromJson<TokenHttpResponse>(responseJson);
                if (tokenResponse == null)
                {
                    Log.Error("Failed to deserialize the login response JSON.");
                    menuForm.OnLoginFailure(GameEntry.Localization.GetString("Login.ConnectServerFailed"));
                }
                else if (tokenResponse.code == GameErrorType.Success)
                {
                    Log.Info("Guest login successful.");

                    var accountModule = GameEntry.ModuleComponent.GetModule<AccountModule>();
                    accountModule.token.SetToken(tokenResponse.token, tokenResponse.expire_milliseconds);

                    // 直接将结果传递出去，不再持有它
                    menuForm.OnLoginSuccess(tokenResponse);
                }
                else
                {
                    Log.Warning(
                        $"Guest login failed with code: {tokenResponse.code}, Message: {tokenResponse.message}");
                    menuForm.OnLoginFailure(tokenResponse.message);
                }
            }
        }

        private void OnWebRequestFailure(object sender, GameEventArgs args)
        {
            var webRequestFailureEventArgs = (WebRequestFailureEventArgs)args;
            if (webRequestFailureEventArgs.SerialId != serialId)
            {
                return;
            }

            Log.Error(
                $"Web request failed. Uri: {webRequestFailureEventArgs.WebRequestUri}, Error: {webRequestFailureEventArgs.ErrorMessage}");
            menuForm.OnLoginFailure(GameEntry.Localization.GetString("Login.ConnectServerFailed"));
            CleanUp();
        }

        private void CleanUp()
        {
            GameEntry.Event.Unsubscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
            GameEntry.Event.Unsubscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFailure);
            serialId = 0;
        }
    }
}