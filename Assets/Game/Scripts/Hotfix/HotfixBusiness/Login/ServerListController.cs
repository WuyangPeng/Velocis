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
    ///     服务器列表 HTTP 请求控制器。
    /// </summary>
    public class ServerListController
    {
        /// <summary>
        ///     获取服务器列表失败的回调，参数为错误消息。
        /// </summary>
        private readonly Action<string> _onFailure;

        /// <summary>
        ///     获取服务器列表成功的回调。
        /// </summary>
        private readonly Action<LoginServersResponse> _onSuccess;

        /// <summary>
        ///     当前进行中的网络请求序列号。
        /// </summary>
        private int _serialId;

        /// <summary>
        ///     初始化服务器列表请求控制器的新实例。
        /// </summary>
        /// <param name="onSuccess">请求成功的回调。</param>
        /// <param name="onFailure">请求失败的回调。</param>
        public ServerListController(Action<LoginServersResponse> onSuccess, Action<string> onFailure)
        {
            _onSuccess = onSuccess;
            _onFailure = onFailure;
            _serialId = 0;
        }

        /// <summary>
        ///     进入控制器，订阅网络请求事件。
        /// </summary>
        public void OnEnter()
        {
            GameEntry.Event.Subscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
            GameEntry.Event.Subscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFailure);
        }

        /// <summary>
        ///     离开控制器，退订网络请求事件。
        /// </summary>
        public void OnLeave()
        {
            GameEntry.Event.Unsubscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
            GameEntry.Event.Unsubscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFailure);
        }

        /// <summary>
        ///     发起拉取服务器列表的请求。
        /// </summary>
        /// <param name="token">用户的认证 Token 字符串。</param>
        /// <param name="zone">目标大区标识符。</param>
        public void FetchServerList(string token = "", string zone = "")
        {
            if (_serialId != 0)
            {
                Log.Warning("[ServerListController] Server list request is already in progress.");
                return;
            }

            var finalUri = BuildServerListUrl(token, zone);
            _serialId = GameEntry.WebRequest.AddWebRequest(finalUri, this);

            if (_serialId != 0)
            {
                return;
            }

            Log.Error("[ServerListController] Failed to add web request to the queue.");
            CleanUp();
            _onFailure?.Invoke(GameEntry.Localization.GetString("Login.ConnectServerFailed"));
        }

        /// <summary>
        ///     构建拉取服务器列表的完整网络请求地址（包含加密签名参数）。
        /// </summary>
        /// <param name="token">用户的认证 Token。</param>
        /// <param name="zone">目标分区（可选）。</param>
        /// <returns>带有签名及查询参数的完整 URI 字符串。</returns>
        private static string BuildServerListUrl(string token, string zone)
        {
            var url = GameEntry.Account.ServerListUrl;
            var appId = GameEntry.Account.appId;
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            const bool onlyPreferred = false;
            const bool includeDetails = true;
            const bool websocket = false;
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
                { NetworkConstant.Token, token },
                { NetworkConstant.AppId, appId },
                { NetworkConstant.Timestamp, timestamp },
                { NetworkConstant.OnlyPreferred, onlyPreferred ? "true" : "false" },
                { NetworkConstant.IncludeDetails, includeDetails ? "true" : "false" },
                { NetworkConstant.WebSocket, websocket ? "true" : "false" },
                { NetworkConstant.Zone, zone ?? "" },
                { NetworkConstant.Sign, sign }
            };

            var queryString = string.Join("&",
                queryParams.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));
            return $"{url}?{queryString}";
        }

        /// <summary>
        ///     网络请求成功回调函数。
        /// </summary>
        /// <param name="sender">事件发送者。</param>
        /// <param name="args">事件参数。</param>
        private void OnWebRequestSuccess(object sender, GameEventArgs args)
        {
            var e = (WebRequestSuccessEventArgs)args;
            if (e.UserData != this)
            {
                return;
            }

            try
            {
                var json = Encoding.UTF8.GetString(e.GetWebResponseBytes());
                Log.Info("[ServerListController] Response: " + json);
                HandleServerListResponse(json);
            }
            catch (Exception ex)
            {
                Log.Error("[ServerListController] Exception: " + ex);
                _onFailure?.Invoke(GameEntry.Localization.GetString("Login.ConnectServerExceptional"));
            }
            finally
            {
                CleanUp();
            }
        }

        /// <summary>
        ///     解析并处理服务器返回的服务器列表响应 JSON 数据。
        /// </summary>
        /// <param name="json">服务器返回的 JSON 字符串。</param>
        private void HandleServerListResponse(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                _onFailure?.Invoke(GameEntry.Localization.GetString("Login.ConnectServerFailed"));
                return;
            }

            var response = JsonUtility.FromJson<LoginServersResponse>(json);
            if (response == null)
            {
                _onFailure?.Invoke(GameEntry.Localization.GetString("Login.ConnectServerFailed"));
                return;
            }

            if (response.code == GameErrorType.Success)
            {
                Log.Info("[ServerListController] Server list fetched successfully.");
                _onSuccess?.Invoke(response);
            }
            else
            {
                Log.Warning($"[ServerListController] Failed: {response.code} {response.message}");
                _onFailure?.Invoke(GameEntry.Localization.GetString("Server.ErrorCode" + (int)response.code));
            }
        }

        /// <summary>
        ///     网络请求失败回调函数。
        /// </summary>
        /// <param name="sender">事件发送者。</param>
        /// <param name="args">事件参数。</param>
        private void OnWebRequestFailure(object sender, GameEventArgs args)
        {
            var e = (WebRequestFailureEventArgs)args;
            if (e.UserData != this)
            {
                return;
            }

            Log.Error($"[ServerListController] Web request failed. Uri: {e.WebRequestUri}, Error: {e.ErrorMessage}");
            _onFailure?.Invoke(GameEntry.Localization.GetString("Login.ConnectServerFailed"));
            CleanUp();
        }

        /// <summary>
        ///     清理当前请求的序列号。
        /// </summary>
        private void CleanUp()
        {
            _serialId = 0;
        }
    }
}