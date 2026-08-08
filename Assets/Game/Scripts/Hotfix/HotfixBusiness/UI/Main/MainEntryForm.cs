using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Celeritas.Config;
using Game.Scripts.Hotfix.HotfixBusiness.UI.Common;
using Game.Scripts.Hotfix.HotfixCommon.Event;
using Game.Scripts.Hotfix.HotfixCommon.GameModule.RedDot;
using Game.Scripts.Hotfix.HotfixCommon.GameModule.User;
using Game.Scripts.Hotfix.HotfixCommon.Login;
using Game.Scripts.Main.Runtime.Definition.Constant;
using Game.Scripts.Main.Runtime.Procedure;
using Game.Scripts.Main.Runtime.UI.UICommon;
using Game.Scripts.Main.Runtime.UI.UIMenu;
using Game.Scripts.Main.Runtime.Utils;
using GameFramework;
using GameFramework.Event;
using TMPro;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;
using LoginController = Game.Scripts.Hotfix.HotfixBusiness.Login.LoginController;
using ServerListController = Game.Scripts.Hotfix.HotfixBusiness.Login.ServerListController;

namespace Game.Scripts.Hotfix.HotfixBusiness.UI.Main
{
    public class MainEntryForm : UGuiForm
    {
        // ──────────────────────────────────────────────
        // 序列化字段（由 MainEntryFormCreator 通过反射绑定）
        // ──────────────────────────────────────────────

        [SerializeField] private TMP_InputField accountInputField;
        [SerializeField] private BaseButton enterGameButton;
        [SerializeField] private TMP_Text enterGameButtonText;
        [SerializeField] private BaseButton settingButton;
        [SerializeField] private BaseButton aboutButton;
        [SerializeField] private BaseButton announcementButton;
        [SerializeField] private RedDot announcementRedDot;
        [SerializeField] private BaseButton serviceButton;
        [SerializeField] private BaseButton quitButton;
        [SerializeField] private TMP_Text versionText;
        [SerializeField] private GameObject loadingSpinner;
        private int _announcementRequestSerialId;

        // ──────────────────────────────────────────────
        // 私有状态
        // ──────────────────────────────────────────────

        /// <summary>
        ///     当前输入框显示的是否为"默认占位文字"（非用户真实保存的账号）。
        ///     若为 true，点击输入框时自动清空；若为 false（真实账号），保留不清除。
        /// </summary>
        private bool _isShowingDefaultText;

        private LoginController _loginController;
        private string _loginToken;
        private ServerListController _serverListController;

        // ──────────────────────────────────────────────
        // 生命周期
        // ──────────────────────────────────────────────

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            InitVersionText();
            InitAccountInputField();

            _announcementRequestSerialId = 0;
            _loginToken = string.Empty;

            _loginController = new LoginController(OnLoginSuccess, OnLoginFailure);
            _loginController.OnEnter();

            _serverListController = new ServerListController(OnServerListSuccess, OnServerListFailure);
            _serverListController.OnEnter();

            if (GameEntry.Event != null)
            {
                GameEntry.Event.Subscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
                GameEntry.Event.Subscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFailure);
            }

            FetchAnnouncements();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            _loginController?.OnLeave();
            _loginController = null;

            _serverListController?.OnLeave();
            _serverListController = null;

            base.OnClose(isShutdown, userData);

            if (GameEntry.Event != null)
            {
                GameEntry.Event.Unsubscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
                GameEntry.Event.Unsubscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFailure);
            }

            _announcementRequestSerialId = 0;

            if (accountInputField == null)
            {
                return;
            }

            accountInputField.onSelect.RemoveListener(OnInputFieldSelected);
            accountInputField.onDeselect.RemoveListener(OnInputFieldDeselected);
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (loadingSpinner)
            {
                loadingSpinner.SetActive(false);
            }

            if (enterGameButton)
            {
                enterGameButton.enabled = true;
            }

            if (enterGameButtonText)
            {
                enterGameButtonText.gameObject.SetActive(true);
            }
        }

        public void OnQuitButtonClick()
        {
            GameEntry.UI.OpenDialog(new DialogParams
            {
                Mode = 2,
                Title = GameEntry.Localization.GetString("Dialog.AskQuitGameTitle"),
                Message = GameEntry.Localization.GetString("Dialog.AskQuitGameMessage"),
                OnClickConfirm = delegate { UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Quit); }
            });
        }

        public void OnSettingButtonClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.SystemSettingForm);
        }

        public void OnAboutButtonClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.HelpForm);
        }

        public void OnAnnouncementButtonClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.AnnouncementForm);
        }

        public void OnServiceButtonClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.FeedbackForm);
        }

        public void OnEnterGameButtonClick()
        {
            if (accountInputField != null)
            {
                var account = accountInputField.text.Trim();
                GameEntry.Setting.SetString(Constant.Setting.LastAccount, account);
                GameEntry.Setting.Save();
            }

            SetLoading(true);
            _loginController.GuestLogin(accountInputField != null ? accountInputField.text.Trim() : null);
        }

        private void OnLoginSuccess(TokenHttpResponse response)
        {
            _loginToken = response.token;
            var accountModule = GameEntry.ModuleComponent.GetModule<AccountModule>();
            if (accountModule != null)
            {
                accountModule.SetToken(response.token, response.expire_milliseconds);
            }

            _serverListController.FetchServerList(_loginToken);
        }

        private void OnLoginFailure(string errorMessage)
        {
            SetLoading(false);
            GameEntry.UI.OpenDialog(new DialogParams
            {
                Mode = 1,
                Title = GameEntry.Localization.GetString("Login.LoginFailed"),
                Message = string.IsNullOrEmpty(errorMessage)
                    ? GameEntry.Localization.GetString("Login.ConnectServerFailed")
                    : errorMessage
            });
        }

        private void OnServerListSuccess(LoginServersResponse response)
        {
            SetLoading(false);
            var accountModule = GameEntry.ModuleComponent.GetModule<AccountModule>();
            if (accountModule != null)
            {
                accountModule.SetLoginServerInfo(response.login_server_info);
                accountModule.SetZones(response.zones);
            }

            var host = (IProcedureFormHost)GetCurrentProcedure();
            host.OpenUIForm(UIFormId.ServerListForm);
        }

        private void OnServerListFailure(string errorMessage)
        {
            SetLoading(false);
            GameEntry.UI.OpenDialog(new DialogParams
            {
                Mode = 1,
                Title = GameEntry.Localization.GetString("Login.LoginFailed"),
                Message = string.IsNullOrEmpty(errorMessage)
                    ? GameEntry.Localization.GetString("Login.ConnectServerFailed")
                    : errorMessage
            });
        }

        private void SetLoading(bool isLoading)
        {
            if (enterGameButton)
            {
                enterGameButton.enabled = !isLoading;
            }

            if (enterGameButtonText)
            {
                enterGameButtonText.gameObject.SetActive(!isLoading);
            }

            if (loadingSpinner)
            {
                loadingSpinner.SetActive(isLoading);
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (loadingSpinner != null && loadingSpinner.activeSelf)
            {
                loadingSpinner.transform.Rotate(Vector3.forward, -360f * elapseSeconds);
            }
        }

        // ──────────────────────────────────────────────
        // 初始化
        // ──────────────────────────────────────────────

        private void InitVersionText()
        {
            if (versionText != null && GameEntry.Account != null)
            {
                versionText.text = $"v{GameEntry.Account.appVersion}";
            }
        }

        private void InitAccountInputField()
        {
            if (accountInputField == null)
            {
                return;
            }

            var savedAccount = GameEntry.Setting.GetString(Constant.Setting.LastAccount, string.Empty);

            // 有保存的账号：填入输入框，不会在点击时清除
            // 无保存账号：留空，TMP 自动展示 Placeholder
            // 若外部系统将默认占位文字写入 text，_isShowingDefaultText 标记为 true
            accountInputField.text = !string.IsNullOrEmpty(savedAccount) ? savedAccount : string.Empty;

            _isShowingDefaultText = false;

            accountInputField.onSelect.AddListener(OnInputFieldSelected);
            accountInputField.onDeselect.AddListener(OnInputFieldDeselected);
        }

        // ──────────────────────────────────────────────
        // 事件
        // ──────────────────────────────────────────────

        /// <summary>
        ///     点击输入框时触发：立即隐藏 placeholder，保留真实保存账号不清除。
        /// </summary>
        private void OnInputFieldSelected(string currentText)
        {
            // 无论如何，先把 placeholder 隐藏（TMP 默认聚焦后仍显示）
            if (accountInputField.placeholder != null)
            {
                accountInputField.placeholder.gameObject.SetActive(false);
            }

            // 若当前显示的是默认占位文字（非保存账号），则清空
            if (!_isShowingDefaultText)
            {
                return;
            }

            accountInputField.text = string.Empty;
            _isShowingDefaultText = false;
        }

        /// <summary>
        ///     失去焦点时触发：若内容为空则恢复 placeholder 显示。
        /// </summary>
        private void OnInputFieldDeselected(string currentText)
        {
            if (string.IsNullOrEmpty(currentText) && accountInputField.placeholder != null)
            {
                accountInputField.placeholder.gameObject.SetActive(true);
            }
        }

        // ──────────────────────────────────────────────
        // 公共方法（供外部业务调用）
        // ──────────────────────────────────────────────

        /// <summary>
        ///     将指定文字设为"默认占位文字"模式填入输入框。
        ///     点击后会自动清除，区别于真实保存账号。
        /// </summary>
        public void SetDefaultText(string defaultText)
        {
            if (accountInputField == null)
            {
                return;
            }

            accountInputField.text = defaultText;
            _isShowingDefaultText = true;
        }

        /// <summary>返回当前输入框的账号文字。</summary>
        public string GetInputAccount()
        {
            return accountInputField != null ? accountInputField.text.Trim() : string.Empty;
        }

        private void FetchAnnouncements()
        {
            if (GameEntry.Account == null || GameEntry.WebRequest == null)
            {
                return;
            }

            var apiUrl = GameEntry.Account.AnnouncementUrl;
            if (string.IsNullOrEmpty(apiUrl))
            {
                return;
            }

            // 以玩家上次阅读公告时的 UTC 时间戳作为基准，服务端据此筛选 update_time 更新的公告
            var lastReadTime = GameEntry.Setting.GetObject(Constant.Setting.LastReadAnnouncementTime, 0L).ToString();
            var appId = GameEntry.Account.appId;
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            var sign = HmacSha256Util.ComputeHash(GameEntry.Account.secret, appId, lastReadTime, timestamp);
            var queryParams = new Dictionary<string, string>
            {
                { "last_time", lastReadTime },
                { "app_id", appId },
                { "timestamp", timestamp },
                { "sign", sign }
            };
            var queryString = string.Join("&",
                queryParams.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));
            var queryUrl = $"{apiUrl}/red_dot?{queryString}";
            Log.Info("FetchAnnouncements red dot started. URL: {0}", queryUrl);
            _announcementRequestSerialId = GameEntry.WebRequest.AddWebRequest(queryUrl, this);
        }

        private void OnWebRequestSuccess(object sender, GameEventArgs args)
        {
            var successArgs = (WebRequestSuccessEventArgs)args;
            if (successArgs.SerialId != _announcementRequestSerialId || (MainEntryForm)successArgs.UserData != this)
            {
                return;
            }

            var responseBytes = successArgs.GetWebResponseBytes();
            var json = responseBytes != null ? Encoding.UTF8.GetString(responseBytes) : string.Empty;
            Log.Info("Fetch announcements red dot response: {0}", json);

            var result = ParseServerAnnouncementRedDotResponse(json);

            var redDotModule = GameEntry.ModuleComponent.GetModule<RedDotModule>();
            if (redDotModule != null)
            {
                redDotModule.AddRedDotNode(new RedDotNode(red_dot_type.announcement, result));
            }

            var redDotData = new Dictionary<red_dot_type, int>
            {
                { red_dot_type.announcement, result }
            };
            GameEntry.Event.Fire(this, ChangeRedDotEventArgs.Create(redDotData));
        }

        private void OnWebRequestFailure(object sender, GameEventArgs args)
        {
            var failureArgs = (WebRequestFailureEventArgs)args;
            if (failureArgs.SerialId != _announcementRequestSerialId || (MainEntryForm)failureArgs.UserData != this)
            {
                return;
            }

            Log.Warning("MainEntryForm: Fetch announcements red dot failed in background check.");
        }

        private static int ParseServerAnnouncementRedDotResponse(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return 0;
            }

            try
            {
                var response = Utility.Json.ToObject<AnnouncementRedDotResponse>(json);
                if (response != null && response.data != null)
                {
                    return response.data.red_dot_count;
                }
            }
            catch (Exception e)
            {
                Log.Warning("ParseServerAnnouncementRedDotResponse failed: {0}", e.Message);
            }

            return 0;
        }

        [Serializable]
        private class AnnouncementRedDotResponse
        {
            public int code;
            public AnnouncementRedDotData data;
        }

        [Serializable]
        private class AnnouncementRedDotData
        {
            public int red_dot_count;
        }
    }
}