// 创建时间：2026-07-27
// 修改时间：2026-08-01

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Game.Scripts.Hotfix.HotfixBusiness.UI.Common;
using Game.Scripts.Hotfix.HotfixCommon.Event;
using Game.Scripts.Hotfix.HotfixCommon.GameModule.User;
using Game.Scripts.Hotfix.HotfixCommon.Network;
using Game.Scripts.Main.Runtime.Event;
using Game.Scripts.Hotfix.HotfixCommon.Login;
using Game.Scripts.Main.Runtime.Network;
using Game.Scripts.Main.Runtime.Procedure;
using Game.Scripts.Main.Runtime.UI.UICommon;
using Game.Scripts.Main.Runtime.UI.UIMenu;
using GameFramework.Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using AddressFamily = System.Net.Sockets.AddressFamily;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;
using ServerListController = Game.Scripts.Hotfix.HotfixBusiness.Login.ServerListController;

namespace Game.Scripts.Hotfix.HotfixBusiness.UI.Menu
{
    public class ServerListFormUserData
    {
        public LoginServersResponse Response;
        public string Token;
    }

    public class ServerListForm : UGuiForm
    {
        // ──────────────────────────────────────────────
        // 序列化字段（通过 ServerListFormCreator 反射绑定）
        // ──────────────────────────────────────────────
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private CategoryButtonGroup zoneTabList;
        [SerializeField] private ScrollRect serverCardList;
        [SerializeField] private RectTransform serverCardContainer;
        [SerializeField] private ServerListItem serverCardTemplate;
        [SerializeField] private TMP_Text selectedServerText;
        [SerializeField] private BaseButton confirmButton;

        [SerializeField] private BaseButton closeButton;

        // ──────────────────────────────────────────────
        // 私有状态
        // ──────────────────────────────────────────────
        private readonly List<LoginServerInfo> _allServers = new();
        private readonly List<ServerListItem> _cardPool = new();
        private readonly List<GameObject> _tabButtons = new();
        private readonly List<string> _zoneIds = new(); // 原始 zone id（传给后端）
        private readonly List<string> _zones = new(); // 显示用多语言名


        private int _currentZoneIndex = -1;
        private bool _isConnecting;
        private IProcedureFormHost _procedureMenu;
        private int _selectedServerIndex = -1;
        private ServerListController _serverListController;
        private string _token = "";

        protected override void OnOpen(object userData)
        {
            _isConnecting = false;
            base.OnOpen(userData);

            _procedureMenu = (IProcedureFormHost)GetCurrentProcedure();
            if (_procedureMenu == null)
            {
                Log.Warning("ProcedureMenu is invalid when open ServerListForm.");
            }



            _serverListController = new ServerListController(OnFetchSuccess, OnFetchFailure);
            _serverListController.OnEnter();

            InitUI();

            if (userData is ServerListFormUserData serverListUserData)
            {
                _token = serverListUserData.Token;
                FetchServerList(serverListUserData.Response);
            }
            else
            {
                _token = GameEntry.ModuleComponent.GetModule<AccountModule>().GetToken();

                FetchServerList(userData);
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            _allServers.Clear();
            _zones.Clear();
            _zoneIds.Clear();
            _currentZoneIndex = -1;
            _selectedServerIndex = -1;

            if (_serverListController != null)
            {
                _serverListController.OnLeave();
                _serverListController = null;
            }

            _procedureMenu = null;

            base.OnClose(isShutdown, userData);
        }

        protected override void OnResume()
        {
            base.OnResume();

            _isConnecting = false;
        }

        private void OnFetchSuccess(LoginServersResponse response)
        {
            FetchServerList(response);
        }

        private void OnFetchFailure(string errorMessage)
        {
            Log.Warning($"[ServerListForm] Fetch server list failed: {errorMessage}");
        }

        private void InitUI()
        {
            if (titleText != null)
            {
                titleText.text = GameEntry.Localization.GetString("ServerList.Title");
            }

            if (confirmButton != null)
            {
                confirmButton.OnClick.RemoveAllListeners();
                confirmButton.OnClick.AddListener(OnConfirmButtonClick);
                var btnText = confirmButton.GetComponentInChildren<TMP_Text>();
                if (btnText != null)
                {
                    btnText.text = GameEntry.Localization.GetString("ServerList.BtnConfirm");
                }
            }

            if (closeButton != null)
            {
                closeButton.OnClick.RemoveAllListeners();
                closeButton.OnClick.AddListener(OnCloseButtonClick);
            }

            if (serverCardTemplate != null)
            {
                serverCardTemplate.gameObject.SetActive(false);
            }

            if (zoneTabList != null && zoneTabList.CategoryButtonTemplate != null)
            {
                zoneTabList.CategoryButtonTemplate.SetActive(false);
            }

            if (selectedServerText != null)
            {
                selectedServerText.text = string.Empty;
            }
        }

        private void FetchServerList(object userData)
        {
            _allServers.Clear();

            List<LoginServerInfo> serverInfoList = null;
            List<string> zonesList = null;
            var hasNewZones = false;

            if (userData is LoginServersResponse response)
            {
                serverInfoList = response.login_server_info;
                zonesList = response.zones;
            }
            else if (userData is List<LoginServerInfo> list)
            {
                serverInfoList = list;
            }
            else
            {
                var accountModule = GameEntry.ModuleComponent.GetModule<AccountModule>();
                serverInfoList = accountModule.GetLoginServerInfo();
                zonesList = accountModule.GetZones();
            }

            if (zonesList != null && zonesList.Count > 0)
            {
                if (_zones.Count <= 1)
                {
                    _zones.Clear();
                    _zoneIds.Clear();
                    _zones.Add(GameEntry.Localization.GetString("ServerList.TabRecommended"));
                    _zoneIds.Add(""); // index 0 = 推荐，id 为空
                    foreach (var zoneStr in zonesList)
                    {
                        _zones.Add(GetZoneNameByZoneId(zoneStr));
                        _zoneIds.Add(zoneStr);
                    }

                    hasNewZones = true;
                }
            }

            if (serverInfoList != null)
            {
                _allServers.AddRange(serverInfoList);
            }

            if (_zones.Count <= 1)
            {
                ParseZones();
                hasNewZones = true;
            }

            if (hasNewZones)
            {
                RefreshZoneTabs();
            }

            if (_currentZoneIndex == -1)
            {
                _currentZoneIndex = 0;
                UpdateTabVisuals();
            }

            RefreshServerCards();
        }

        private void ParseZones()
        {
            _zones.Clear();
            _zoneIds.Clear();
            _zones.Add(GameEntry.Localization.GetString("ServerList.TabRecommended"));
            _zoneIds.Add(""); // index 0 = 推荐，id 为空

            // 按 zone 编号去重并保持顺序
            var uniqueZoneIds = new SortedSet<int>();
            foreach (var server in _allServers)
            {
                if (server.zone > 0)
                {
                    uniqueZoneIds.Add(server.zone);
                }
            }

            foreach (var zoneId in uniqueZoneIds)
            {
                var zoneIdStr = zoneId.ToString();
                _zones.Add(GetZoneNameByZoneId(zoneIdStr));
                _zoneIds.Add(zoneIdStr);
            }
        }

        /// <summary>
        ///     将服务器下发的 zone 编号字符串（"1"~"10"）转为对应九州多语言名称。
        /// </summary>
        private string GetZoneNameByZoneId(string zoneIdStr)
        {
            if (int.TryParse(zoneIdStr, out var zoneId) && zoneId >= 1 && zoneId <= 10)
            {
                return GameEntry.Localization.GetString($"ServerList.Zone.{zoneId}");
            }

            // 兜底：直接显示原始字符串
            return zoneIdStr;
        }


        private void RefreshZoneTabs()
        {
            foreach (var tab in _tabButtons)
            {
                Destroy(tab);
            }

            _tabButtons.Clear();

            if (zoneTabList == null || zoneTabList.CategoryButtonTemplate == null || zoneTabList.CategoryContainer == null)
            {
                return;
            }

            for (var i = 0; i < _zones.Count; i++)
            {
                var index = i;
                var tabGo = Instantiate(zoneTabList.CategoryButtonTemplate, zoneTabList.CategoryContainer, false);
                tabGo.SetActive(true);

                var txt = tabGo.GetComponentInChildren<TMP_Text>();
                if (txt != null)
                {
                    txt.text = _zones[i];
                }

                var baseBtn = tabGo.GetComponent<BaseButton>();
                if (baseBtn != null)
                {
                    baseBtn.OnClick.RemoveAllListeners();
                    baseBtn.OnClick.AddListener(() =>
                    {
                        if (zoneTabList != null)
                        {
                            zoneTabList.PlayTabSwitchSound();
                        }

                        SelectZone(index);
                    });
                }

                _tabButtons.Add(tabGo);
            }

            UpdateTabVisuals();
        }

        private void UpdateTabVisuals()
        {
            for (var i = 0; i < _tabButtons.Count; i++)
            {
                var btnImg = _tabButtons[i].transform.Find("Image")?.GetComponent<Image>();
                if (btnImg != null && zoneTabList != null)
                {
                    btnImg.sprite = i == _currentZoneIndex ? zoneTabList.CategorySelectedSprite : zoneTabList.CategoryNormalSprite;
                }

                var btnTxt = _tabButtons[i].transform.Find("Text")?.GetComponent<TMP_Text>();
                if (btnTxt != null)
                {
                    btnTxt.color = i == _currentZoneIndex ? new Color(0.17f, 0.12f, 0.08f, 1f) : Color.white;
                }
            }
        }

        private void SelectZone(int zoneIndex)
        {
            if (zoneIndex < 0 || zoneIndex >= _zones.Count)
            {
                return;
            }

            if (_currentZoneIndex == zoneIndex)
            {
                return;
            }

            _currentZoneIndex = zoneIndex;
            UpdateTabVisuals();

            // 用原始 zone id 传给后端，而非显示用的多语言名
            var zoneId = zoneIndex < _zoneIds.Count ? _zoneIds[zoneIndex] : "";
            _serverListController?.FetchServerList(_token, zoneId);
        }

        private List<LoginServerInfo> GetFilteredServers()
        {
            return _allServers;
        }

        private void RefreshServerCards()
        {
            foreach (var card in _cardPool)
            {
                card.gameObject.SetActive(false);
            }

            if (serverCardContainer == null || serverCardTemplate == null)
            {
                return;
            }

            var servers = GetFilteredServers();
            for (var i = 0; i < servers.Count; i++)
            {
                var index = i;
                ServerListItem card;
                if (i < _cardPool.Count)
                {
                    card = _cardPool[i];
                }
                else
                {
                    var cardGo = Instantiate(serverCardTemplate.gameObject, serverCardContainer, false);
                    card = cardGo.GetComponent<ServerListItem>();
                    _cardPool.Add(card);
                }

                card.gameObject.SetActive(true);

                var serverInfo = servers[i];
                var globalIndex = _allServers.IndexOf(serverInfo);

                card.SetData(serverInfo);

                if (card.Button != null)
                {
                    card.Button.OnClick.RemoveAllListeners();
                    card.Button.OnClick.AddListener(() => OnCardClick(globalIndex));
                }

                if (card.StatusIndicator != null)
                {
                    card.StatusIndicator.color = GetStatusColor(serverInfo.server_status);
                }

                if (card.PingText != null)
                {
                    card.PingText.text = "...";
                    MeasurePing(serverInfo, card);
                }

                if (card.CharacterMark != null)
                {
                    var hasRole = serverInfo.player_role != null && !string.IsNullOrEmpty(serverInfo.player_role.role_name);
                    card.CharacterMark.SetActive(hasRole);
                }
            }

            if (serverCardList != null)
            {
                serverCardList.content.anchoredPosition = new Vector2(serverCardList.content.anchoredPosition.x, 0f);
            }

            _selectedServerIndex = -1;
            UpdateSelectedServerText();
            UpdateCardSelections();
        }

        private void UpdateCardSelections()
        {
            var servers = GetFilteredServers();
            for (var i = 0; i < servers.Count; i++)
            {
                if (i < _cardPool.Count)
                {
                    var serverInfo = servers[i];
                    var globalIndex = _allServers.IndexOf(serverInfo);
                    _cardPool[i].SetSelected(globalIndex == _selectedServerIndex);
                }
            }
        }

        private Color GetStatusColor(ServerStatusType status)
        {
            switch (status)
            {
                case ServerStatusType.Normal: return Color.green;
                case ServerStatusType.Busy: return Color.yellow;
                case ServerStatusType.Crowded: return Color.yellow;
                case ServerStatusType.Full: return Color.red;
                case ServerStatusType.Maintenance: return Color.gray;
                default: return Color.gray;
            }
        }

        private async void MeasurePing(LoginServerInfo serverInfo, ServerListItem card)
        {
            if (serverInfo.connection_info == null || string.IsNullOrEmpty(serverInfo.connection_info.host))
            {
                SetPingUnknown(card);
                return;
            }

            var latency = await PingHost(serverInfo.connection_info.host);
            if (card == null || card.PingText == null)
            {
                return;
            }

            if (latency < 0)
            {
                SetPingUnknown(card);
                return;
            }

            card.PingText.text = $"{latency}ms";

            if (latency < 50)
            {
                card.PingText.color = Color.green;
            }
            else if (latency < 120)
            {
                card.PingText.color = Color.yellow;
            }
            else
            {
                card.PingText.color = Color.red;
            }
        }

        private static void SetPingUnknown(ServerListItem card)
        {
            if (card == null || card.PingText == null)
            {
                return;
            }

            card.PingText.text = "--ms";
            card.PingText.color = Color.gray;
        }

        private async Task<int> PingHost(string ipOrDomain)
        {
            try
            {
                var ip = ipOrDomain;
                if (!IPAddress.TryParse(ipOrDomain, out _))
                {
                    var addresses = await Dns.GetHostAddressesAsync(ipOrDomain);
                    if (addresses != null && addresses.Length > 0)
                    {
                        ip = addresses[0].ToString();
                    }
                }

                var ping = new Ping(ip);
                var timeout = 2.0f;
                var startTime = Time.realtimeSinceStartup;
                while (!ping.isDone && Time.realtimeSinceStartup - startTime < timeout)
                {
                    await Task.Delay(50);
                }

                if (ping.isDone)
                {
                    return ping.time;
                }

                return -1;
            }
            catch
            {
                return -1;
            }
        }

        private void OnCardClick(int globalIndex)
        {
            _selectedServerIndex = globalIndex;
            UpdateSelectedServerText();
            UpdateCardSelections();
        }

        private void UpdateSelectedServerText()
        {
            if (selectedServerText == null)
            {
                return;
            }

            if (_selectedServerIndex < 0 || _selectedServerIndex >= _allServers.Count)
            {
                selectedServerText.text = string.Empty;
                if (confirmButton != null)
                {
                    confirmButton.gameObject.SetActive(false);
                }

                return;
            }

            var server = _allServers[_selectedServerIndex];
            var statusStr = GetStatusText(server.server_status);
            selectedServerText.text = $"{server.GetDisplayServerName()} （{statusStr}）";

            if (confirmButton != null)
            {
                confirmButton.gameObject.SetActive(true);
                var canvasGroup = confirmButton.GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                {
                    canvasGroup = confirmButton.gameObject.AddComponent<CanvasGroup>();
                }

                var isMaintenance = server.server_status == ServerStatusType.Maintenance;
                canvasGroup.alpha = isMaintenance ? 0.5f : 1.0f;
                canvasGroup.interactable = !isMaintenance;
                canvasGroup.blocksRaycasts = !isMaintenance;
            }
        }

        private string GetStatusText(ServerStatusType status)
        {
            switch (status)
            {
                case ServerStatusType.Normal: return GameEntry.Localization.GetString("ServerList.StatusSmooth");
                case ServerStatusType.Busy: return GameEntry.Localization.GetString("ServerList.StatusHot");
                case ServerStatusType.Crowded: return GameEntry.Localization.GetString("ServerList.StatusHot");
                case ServerStatusType.Full: return GameEntry.Localization.GetString("ServerList.StatusFull");
                case ServerStatusType.Maintenance: return GameEntry.Localization.GetString("ServerList.StatusMaintain");
                default: return GameEntry.Localization.GetString("ServerList.StatusMaintain");
            }
        }

        private async void OnConfirmButtonClick()
        {
            if (_isConnecting)
            {
                return;
            }

            if (_selectedServerIndex < 0 || _selectedServerIndex >= _allServers.Count)
            {
                return;
            }

            var server = _allServers[_selectedServerIndex];
            if (server.server_status == ServerStatusType.Maintenance)
            {
                GameEntry.UI.OpenDialog(new DialogParams
                {
                    Mode = 1,
                    Title = GameEntry.Localization.GetString("Server.Error"),
                    Message = GameEntry.Localization.GetString("ServerList.StatusMaintain") + "，请主公稍后再往。"
                });
                return;
            }

            var connectionInfo = server.connection_info;
            if (connectionInfo == null || string.IsNullOrEmpty(connectionInfo.host))
            {
                Log.Error("Server connection info is invalid.");
                GameEntry.UI.OpenDialog(new DialogParams
                {
                    Mode = 1,
                    Title = GameEntry.Localization.GetString("Server.Error") ?? "错误",
                    Message = "服务器连接信息无效，请稍后重试。"
                });
                return;
            }

            _isConnecting = true;

            var accountModule = GameEntry.ModuleComponent.GetModule<AccountModule>();
            accountModule.SetLoginServerInfo(_allServers);
            accountModule.SetCurrentLoginServerInfo(_selectedServerIndex);

            // 切换到登录加载界面
            GameEntry.Event.Fire(this, LoginLoadEventArgs.Create());

            IPAddress ipAddress = null;
            try
            {
                if (!IPAddress.TryParse(connectionInfo.host, out ipAddress))
                {
                    // 在后台线程执行 DNS 解析，防止卡死 Unity 主线程/UI 渲染
                    var addresses = await Task.Run(() => Dns.GetHostAddresses(connectionInfo.host));
                    if (addresses == null || addresses.Length == 0)
                    {
                        throw new Exception("DNS resolution returned no addresses.");
                    }

                    ipAddress = Array.Find(addresses, a => a.AddressFamily == AddressFamily.InterNetwork) ??
                                addresses[0];
                }
            }
            catch (Exception ex)
            {
                Log.Error("Failed to resolve host '{0}': {1}", connectionInfo.host, ex.Message);

                _isConnecting = false;

                // 触发网络断开/关闭事件，使登录加载界面自动关闭
                GameEntry.Event.Fire(this, NetworkCloseEventArgs.Create());

                // 弹出提示框告知玩家
                GameEntry.UI.OpenDialog(new DialogParams
                {
                    Mode = 1,
                    Title = GameEntry.Localization.GetString("Server.Error") ?? "错误",
                    Message = "解析服务器地址失败，请检查您的网络连接并重试。"
                });
                return;
            }

            // 连接服务器
            var channel = GameEntry.Network.GetNetworkChannel("TcpChannel") ??
                          GameEntry.Network.CreateNetworkChannel("TcpChannel", ServiceType.Tcp,
                              new NetworkChannelHelper());

            if (channel.Connected)
            {
                channel.Close();
            }

            try
            {
                channel.Connect(ipAddress, connectionInfo.port);
            }
            catch (Exception ex)
            {
                Log.Error("Failed to initiate connection: {0}", ex.Message);

                _isConnecting = false;

                GameEntry.Event.Fire(this, NetworkCloseEventArgs.Create());

                GameEntry.UI.OpenDialog(new DialogParams
                {
                    Mode = 1,
                    Title = GameEntry.Localization.GetString("Server.Error") ?? "错误",
                    Message = "连接服务器失败，请稍后重试。"
                });
            }
        }

        private void OnCloseButtonClick()
        {
            _procedureMenu.RemoveUIForm(UIFormId.ServerListForm);
            GameEntry.Event.Fire(this, CloseServerListEventArgs.Create());
        }
    }
}