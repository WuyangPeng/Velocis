using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Scripts.Main.Runtime.UI.UIMenu;
using Game.Scripts.Main.Runtime.Utils;
using GameFramework.Event;
using UnityGameFramework.Runtime;
// using Game.Scripts.Main.Runtime.GameModule.User;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.Login
{
    public class ServerListController
    {
        private readonly MenuForm _menuForm;
        private int _serialId;

        public ServerListController(MenuForm menuForm)
        {
            _menuForm = menuForm;
            _serialId = 0;
        }

        public void OnEnter()
        {
            // 订阅事件
            GameEntry.Event.Subscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
            GameEntry.Event.Subscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFailure);
        }

        public void OnLeave()
        {
            GameEntry.Event.Unsubscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
            GameEntry.Event.Unsubscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFailure);
        }

        public void ServerList(string zone = "")
        {
            if (_serialId != 0)
            {
                Log.Warning("Server List request is already in progress.");
                return;
            }

            var url = GameEntry.Account.ServerListUrl;
            // var accountModule = GameEntry.ModuleComponent.GetModule<AccountModule>();
            var token = ""; // accountModule.GetToken();
            var appId = GameEntry.Account.appId;
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            var onlyPreferred = false;
            var includeDetails = true;
            var websocket = false;
            var sign = HmacSha256Util.ComputeHash(GameEntry.Account.secret,
                appId,
                token,
                zone,
                onlyPreferred ? "1" : "0",
                includeDetails ? "1" : "0",
                websocket ? "1" : "0",
                timestamp);

            var queryParams = new Dictionary<string, string>
            {
                { "token", token },
                { "app_id", appId },
                { "timestamp", timestamp },
                { "only_preferred", onlyPreferred ? "true" : "false" },
                { "include_details", includeDetails ? "true" : "false" },
                { "websocket", websocket ? "true" : "false" },
                { "sign", sign }
            };

            if (!string.IsNullOrEmpty(zone))
            {
                queryParams.Add("zone", zone);
            }

            var queryString = string.Join("&",
                queryParams.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));
            var finalUri = $"{url}?{queryString}";


            // 发送请求并保存序列号
            _serialId = GameEntry.WebRequest.AddWebRequest(finalUri, this);

            if (_serialId == 0)
            {
                Log.Error("Failed to add web request to the queue.");
                CleanUp();
                _menuForm.OnServerListFailure(GameEntry.Localization.GetString("Login.ConnectServerFailed"));
            }
        }

        private void OnWebRequestSuccess(object sender, GameEventArgs args)
        {
            try
            {
                var webRequestSuccessEventArgs = (WebRequestSuccessEventArgs)args;
                if (webRequestSuccessEventArgs.UserData != this)
                {
                    return;
                }

                Success(webRequestSuccessEventArgs);
            }
            catch (Exception error)
            {
                Log.Error("An exception occurred during web request success handling: " + error);
                _menuForm.OnServerListFailure(GameEntry.Localization.GetString("Login.ConnectServerExceptional"));
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
                Log.Error("Server List request failed: The response was null or empty.");
                _menuForm.OnServerListFailure(GameEntry.Localization.GetString("Login.ConnectServerFailed"));
            }
            else
            {
                Log.Info("Server List Response Json =" + responseJson);

                /*  var loginServersResponse = JsonUtility.FromJson<LoginServersResponse>(responseJson);
                  if (loginServersResponse == null)
                  {
                      Log.Error("Failed to deserialize the login response JSON.");
                      _menuForm.OnServerListFailure(GameEntry.Localization.GetString("Login.ConnectServerFailed"));
                  }
                  else if (loginServersResponse.code == GameErrorType.Success)
                  {
                      Log.Info("Server List successful.");

  //            var accountModule = GameEntry.ModuleComponent.GetModule<AccountModule>();

  //            accountModule.SetLoginServerInfo(loginServersResponse.login_server_info);

              // 直接将结果传递出去，不再持有它
              _menuForm.OnServerListSuccess(loginServersResponse);
                  }
                  else
                  {
                      Log.Warning(
                          $"Server List failed with code: {loginServersResponse.code}, Message: {loginServersResponse.message}");
                      _menuForm.OnServerListFailure(loginServersResponse.message);
                  }*/
            }
        }

        private void OnWebRequestFailure(object sender, GameEventArgs args)
        {
            var webRequestFailureEventArgs = (WebRequestFailureEventArgs)args;
            if (webRequestFailureEventArgs.UserData != this)
            {
                return;
            }

            Log.Error(
                $"Web request failed. Uri: {webRequestFailureEventArgs.WebRequestUri}, Error: {webRequestFailureEventArgs.ErrorMessage}");
            _menuForm.OnServerListFailure(GameEntry.Localization.GetString("Login.ConnectServerFailed"));
            CleanUp();
        }

        private void CleanUp()
        {
            _serialId = 0;
        }
    }
}