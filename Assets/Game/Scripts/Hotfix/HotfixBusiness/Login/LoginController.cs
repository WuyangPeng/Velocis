// 创建时间：2026-07-28
// 修改时间：2026-08-02
// 审核时间：2026-08-02

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Scripts.Hotfix.HotfixCommon.Definition;
using Game.Scripts.Hotfix.HotfixCommon.Game;
using Game.Scripts.Hotfix.HotfixCommon.Login;
using Game.Scripts.Main.Runtime.Utils;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Hotfix.HotfixBusiness.Login
{
    /// <summary>
    ///     游客登录 HTTP 请求控制器。
    /// </summary>
    public class LoginController
    {
        /// <summary>
        /// 登录成功回调。
        /// </summary>
        private readonly Action<TokenHttpResponse> _onSuccess;

        /// <summary>
        /// 登录失败回调，参数为错误消息。
        /// </summary>
        private readonly Action<string> _onFailure;

        /// <summary>
        /// 当前进行中的网络请求序列号。
        /// </summary>
        private int _serialId;

        /// <summary>
        /// 初始化登录控制器的新实例。
        /// </summary>
        /// <param name="onSuccess">登录成功的回调。</param>
        /// <param name="onFailure">登录失败的回调。</param>
        public LoginController(Action<TokenHttpResponse> onSuccess, Action<string> onFailure)
        {
            _onSuccess = onSuccess;
            _onFailure = onFailure;
            _serialId = 0;
        }

        /// <summary>
        /// 进入登录控制器，订阅网络请求事件。
        /// </summary>
        public void OnEnter()
        {
            GameEntry.Event.Subscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
            GameEntry.Event.Subscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFailure);
        }

        /// <summary>
        /// 离开登录控制器，退订网络请求事件。
        /// </summary>
        public void OnLeave()
        {
            GameEntry.Event.Unsubscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
            GameEntry.Event.Unsubscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFailure);
        }

        /// <summary>
        /// 发起游客登录请求。
        /// </summary>
        /// <param name="account">账号。如果为空，则默认使用系统的设备标识符。</param>
        public void GuestLogin(string account = null)
        {
            if (_serialId != 0)
            {
                Log.Warning("[LoginController] Login request is already in progress.");
                return;
            }

            var finalUri = BuildGuestLoginUrl(account);
            _serialId = GameEntry.WebRequest.AddWebRequest(finalUri, this);

            if (_serialId != 0)
            {
                return;
            }

            Log.Error("[LoginController] Failed to add web request to the queue.");
            CleanUp();
            _onFailure?.Invoke(GameEntry.Localization.GetString("Login.ConnectServerFailed"));
        }

        /// <summary>
        /// 构建游客登录的完整网络请求地址（包含加密签名参数）。
        /// </summary>
        /// <param name="account">账号。</param>
        /// <returns>带有签名及查询参数的完整 URI 字符串。</returns>
        private static string BuildGuestLoginUrl(string account)
        {
            var url = GameEntry.Account.GuestLoginUrl;
            if (string.IsNullOrEmpty(account))
            {
                account = SystemInfo.deviceUniqueIdentifier;
            }

            var appId = GameEntry.Account.appId;
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            var sign = HmacSha256Util.ComputeHash(GameEntry.Account.secret, appId, account, timestamp);
            var queryParams = new Dictionary<string, string>
            {
                { NetworkConstant.DeviceId, account },
                { NetworkConstant.AppId, appId },
                { NetworkConstant.Timestamp, timestamp },
                { NetworkConstant.Sign, sign }
            };
            var queryString = string.Join("&",
                queryParams.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));
            return $"{url}?{queryString}";
        }

        /// <summary>
        /// 网络请求成功回调函数。
        /// </summary>
        /// <param name="sender">事件发送者。</param>
        /// <param name="args">事件参数。</param>
        private void OnWebRequestSuccess(object sender, GameEventArgs args)
        {
            var e = (WebRequestSuccessEventArgs)args;
            if (e.UserData != this) return;

            try
            {
                var json = Encoding.UTF8.GetString(e.GetWebResponseBytes());
                HandleLoginResponse(json);
            }
            catch (Exception ex)
            {
                Log.Error("[LoginController] Exception: " + ex);
                _onFailure?.Invoke(GameEntry.Localization.GetString("Login.ConnectServerExceptional"));
            }
            finally
            {
                CleanUp();
            }
        }

        /// <summary>
        /// 解析并处理服务器返回的登录响应 JSON 数据。
        /// </summary>
        /// <param name="json">服务器返回的 JSON 字符串。</param>
        private void HandleLoginResponse(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                _onFailure?.Invoke(GameEntry.Localization.GetString("Login.ConnectServerFailed"));
                return;
            }

            var response = JsonUtility.FromJson<TokenHttpResponse>(json);
            if (response == null)
            {
                _onFailure?.Invoke(GameEntry.Localization.GetString("Login.ConnectServerFailed"));
                return;
            }

            if (response.code == GameErrorType.Success)
            {
                Log.Info("[LoginController] Guest login successful.");
                _onSuccess?.Invoke(response);
            }
            else
            {
                Log.Warning($"[LoginController] Login failed: {response.code} {response.message}");
                _onFailure?.Invoke(GameEntry.Localization.GetString("Server.ErrorCode" + (int)response.code));
            }
        }

        /// <summary>
        /// 网络请求失败回调函数。
        /// </summary>
        /// <param name="sender">事件发送者。</param>
        /// <param name="args">事件参数。</param>
        private void OnWebRequestFailure(object sender, GameEventArgs args)
        {
            var e = (WebRequestFailureEventArgs)args;
            if (e.UserData != this) return;

            Log.Error($"[LoginController] Web request failed. Uri: {e.WebRequestUri}, Error: {e.ErrorMessage}");
            _onFailure?.Invoke(GameEntry.Localization.GetString("Login.ConnectServerFailed"));
            CleanUp();
        }

        /// <summary>
        /// 清理当前请求的序列号。
        /// </summary>
        private void CleanUp()
        {
            _serialId = 0;
        }
    }
}
