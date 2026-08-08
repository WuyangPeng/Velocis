using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Celeritas.Config;
using Game.Scripts.Hotfix.HotfixBusiness.UI.Common;
using Game.Scripts.Hotfix.HotfixCommon.Event;
using Game.Scripts.Hotfix.HotfixCommon.GameModule.RedDot;
using Game.Scripts.Main.Runtime.Definition.Constant;
using Game.Scripts.Main.Runtime.Sound;
using Game.Scripts.Main.Runtime.UI.UICommon;
using Game.Scripts.Main.Runtime.UI.UIMenu;
using Game.Scripts.Main.Runtime.Utils;
using GameFramework;
using GameFramework.Event;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Hotfix.HotfixBusiness.UI.Main
{
    /// <summary>
    ///     安民告示（游戏公告）界面。
    /// </summary>
    public class AnnouncementForm : UGuiForm
    {
        private const int MaxContentLength = 10000;
        private const int BannerTimeoutSeconds = 5;
        private const int ListTimeoutSeconds = 10;
        private const long PriorityScoreFactor = 1_000_000_000L;

        // ──────────────────────────────────────────────
        // 序列化字段（由 AnnouncementFormCreator 反射绑定）
        // ──────────────────────────────────────────────
        [SerializeField] private BaseButton closeButton;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private RectTransform listContainer;
        [SerializeField] private AnnouncementItem itemTemplate;
        [SerializeField] private Sprite itemNormalSprite;
        [SerializeField] private Sprite itemSelectedSprite;
        [SerializeField] private AnnouncementDetail detailPanel;
        [SerializeField] [UISoundId] private int openSoundId;
        [SerializeField] [UISoundId] private int selectSoundId;
        [SerializeField] [UISoundId] private int stampSoundId;

        // ──────────────────────────────────────────────
        // 私有状态
        // ──────────────────────────────────────────────
        private readonly List<AnnouncementData> _announcements = new();
        private readonly List<AnnouncementItem> _itemsPool = new();
        private Coroutine _bannerCoroutine;
        private bool _isFetching;
        private Texture2D _loadedBannerTexture;
        private int _selectedIndex = -1;
        private int _serialId;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            Log.Info("AnnouncementForm OnOpen.");

            _selectedIndex = -1;
            _serialId = 0;

            InitUI();
            PlayUISound(openSoundId);

            GameEntry.Event.Subscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
            GameEntry.Event.Subscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFailure);

            FetchAnnouncements();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            if (GameEntry.Event != null)
            {
                GameEntry.Event.Unsubscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
                GameEntry.Event.Unsubscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFailure);
            }

            _serialId = 0;
            _selectedIndex = -1;

            StopBannerLoad();
            ReleaseBannerTexture();
            base.OnClose(isShutdown, userData);
        }

        private void InitUI()
        {
            if (closeButton != null)
            {
                closeButton.OnClick.RemoveAllListeners();
                closeButton.OnClick.AddListener(OnCloseButtonClick);
            }

            if (titleText != null)
            {
                titleText.text = GameEntry.Localization.GetString("AnnouncementForm.Title");
            }

            if (itemTemplate != null)
            {
                itemTemplate.gameObject.SetActive(false);
            }

            if (detailPanel != null && detailPanel.BannerImage != null)
            {
                detailPanel.BannerImage.gameObject.SetActive(false);
            }

            if (detailPanel != null && detailPanel.ContentText != null)
            {
                detailPanel.ContentText.text = string.Empty;
                detailPanel.ContentText.richText = true;
                detailPanel.ContentText.raycastTarget = true;

                var linkHandler = detailPanel.ContentText.GetComponent<AnnouncementContentLinkHandler>();
                if (linkHandler == null)
                {
                    linkHandler = detailPanel.ContentText.gameObject.AddComponent<AnnouncementContentLinkHandler>();
                }

                linkHandler.Bind(detailPanel.ContentText);
            }

            ClearListItems();
        }

        private void FetchAnnouncements()
        {
            _announcements.Clear();
            ClearListItems();
            _isFetching = true;

            var apiUrl = GameEntry.Account.AnnouncementUrl;
            var appId = GameEntry.Account.appId;
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            var sign = HmacSha256Util.ComputeHash(GameEntry.Account.secret, appId, timestamp);
            var queryParams = new Dictionary<string, string>
            {
                { "app_id", appId },
                { "timestamp", timestamp },
                { "sign", sign }
            };
            var queryString = string.Join("&",
                queryParams.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));
            var finalUri = $"{apiUrl}?{queryString}";
            Log.Info("FetchAnnouncements started. URL: {0}", finalUri);

            _serialId = GameEntry.WebRequest.AddWebRequest(finalUri, this);
            if (_serialId == 0)
            {
                Log.Error("Failed to add announcement web request to queue.");
                _isFetching = false;
                ShowFetchFailedDialog();
            }
        }

        private void OnWebRequestSuccess(object sender, GameEventArgs args)
        {
            var successArgs = (WebRequestSuccessEventArgs)args;
            if (successArgs.UserData as AnnouncementForm != this)
            {
                return;
            }

            _isFetching = false;
            var responseBytes = successArgs.GetWebResponseBytes();
            var json = responseBytes != null ? Encoding.UTF8.GetString(responseBytes) : string.Empty;
            Log.Info("Announcement request success. Response: {0}", json);

            if (!TryParseAnnouncements(json, _announcements))
            {
                Log.Error("Parse announcements failed.");
                ShowFetchFailedDialog();
                return;
            }

            SortAnnouncements(_announcements);
            RefreshListAndSelectFirst();
        }

        private void OnWebRequestFailure(object sender, GameEventArgs args)
        {
            var failureArgs = (WebRequestFailureEventArgs)args;
            if (failureArgs.UserData as AnnouncementForm != this)
            {
                return;
            }

            _isFetching = false;
            Log.Error("Announcement request failed. ErrorMessage: {0}", failureArgs.ErrorMessage);
            ShowFetchFailedDialog();
        }


        private void ShowFetchFailedDialog()
        {
            GameEntry.UI.OpenDialog(new DialogParams
            {
                Mode = 2,
                Title = GameEntry.Localization.GetString("AnnouncementForm.FetchFailedTitle"),
                Message = GameEntry.Localization.GetString("AnnouncementForm.FetchFailedMessage"),
                ConfirmText = GameEntry.Localization.GetString("AnnouncementForm.Retry"),
                CancelText = GameEntry.Localization.GetString("AnnouncementForm.Close"),
                OnClickConfirm = _ =>
                {
                    if (!_isFetching)
                    {
                        FetchAnnouncements();
                    }
                },
                OnClickCancel = _ => Close(true)
            });
        }

        private void RefreshListAndSelectFirst()
        {
            RefreshListItems();
            if (_announcements.Count > 0)
            {
                SelectAnnouncement(0, false);
            }
            else if (detailPanel != null && detailPanel.ContentText != null)
            {
                detailPanel.ContentText.text = GameEntry.Localization.GetString("AnnouncementForm.NoContent");
            }
        }

        private void RefreshListItems()
        {
            ClearListItems();

            if (listContainer == null || itemTemplate == null)
            {
                Log.Error("Announcement listContainer or itemTemplate is null.");
                return;
            }

            for (var i = 0; i < _announcements.Count; i++)
            {
                AnnouncementItem item;
                if (i < _itemsPool.Count)
                {
                    item = _itemsPool[i];
                    item.gameObject.SetActive(true);
                }
                else
                {
                    item = Instantiate(itemTemplate, listContainer, false);
                    item.gameObject.SetActive(true);
                    _itemsPool.Add(item);
                }

                BindListItem(item, i);
            }
        }

        private void BindListItem(AnnouncementItem item, int index)
        {
            var data = _announcements[index];

            if (item.TitleText != null)
            {
                item.TitleText.text = data.Title;
            }

            if (item.TagText != null)
            {
                var tagKey = GetTagLocalizationKey(data.Tag);
                if (string.IsNullOrEmpty(tagKey))
                {
                    item.TagText.gameObject.SetActive(false);
                }
                else
                {
                    item.TagText.gameObject.SetActive(true);
                    item.TagText.text = GameEntry.Localization.GetString(tagKey);
                }
            }

            ApplyItemSelectedVisual(item, index == _selectedIndex);

            if (item.Button != null)
            {
                var captured = index;
                item.Button.OnClick.RemoveAllListeners();
                item.Button.OnClick.AddListener(() => SelectAnnouncement(captured, true));
            }
        }

        private void SelectAnnouncement(int index, bool playSelectSound)
        {
            if (index < 0 || index >= _announcements.Count)
            {
                return;
            }

            _selectedIndex = index;

            if (playSelectSound)
            {
                PlayUISound(selectSoundId);
            }

            for (var i = 0; i < _itemsPool.Count; i++)
            {
                if (_itemsPool[i].gameObject.activeSelf)
                {
                    ApplyItemSelectedVisual(_itemsPool[i], i == _selectedIndex);
                }
            }

            var data = _announcements[_selectedIndex];
            RenderDetail(data);
        }

        private void ApplyItemSelectedVisual(AnnouncementItem item, bool selected)
        {
            if (item.BackgroundImage != null)
            {
                var sprite = selected ? itemSelectedSprite : itemNormalSprite;
                if (sprite != null)
                {
                    item.BackgroundImage.sprite = sprite;
                }
            }

            if (item.TitleText != null)
            {
                item.TitleText.color = selected
                    ? new Color(0.17f, 0.12f, 0.08f, 1f)
                    : Color.white;
            }
        }

        private void RenderDetail(AnnouncementData data)
        {
            StopBannerLoad();

            if (detailPanel != null && detailPanel.ContentText != null)
            {
                var content = data.Content ?? string.Empty;
                if (content.Length > MaxContentLength)
                {
                    content = content.Substring(0, MaxContentLength);
                }

                detailPanel.ContentText.text = content;
            }

            if (detailPanel != null && detailPanel.DetailContent != null)
            {
                detailPanel.DetailContent.anchoredPosition = new Vector2(detailPanel.DetailContent.anchoredPosition.x, 0f);
            }

            if (detailPanel == null || detailPanel.BannerImage == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(data.BannerUrl))
            {
                detailPanel.BannerImage.gameObject.SetActive(false);
                ReleaseBannerTexture();
                return;
            }

            _bannerCoroutine = StartCoroutine(LoadBannerCoroutine(data.BannerUrl));
        }

        private IEnumerator LoadBannerCoroutine(string url)
        {
            if (detailPanel == null || detailPanel.BannerImage == null)
            {
                yield break;
            }

            detailPanel.BannerImage.gameObject.SetActive(false);

            using (var request = UnityWebRequestTexture.GetTexture(url))
            {
                request.timeout = BannerTimeoutSeconds;
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    ReleaseBannerTexture();
                    detailPanel.BannerImage.gameObject.SetActive(false);
                    yield break;
                }

                var texture = DownloadHandlerTexture.GetContent(request);
                if (texture == null)
                {
                    detailPanel.BannerImage.gameObject.SetActive(false);
                    yield break;
                }

                ReleaseBannerTexture();
                _loadedBannerTexture = texture;
                detailPanel.BannerImage.texture = texture;
                detailPanel.BannerImage.gameObject.SetActive(true);
            }
        }

        private void ClearListItems()
        {
            foreach (var item in _itemsPool)
            {
                item.gameObject.SetActive(false);
            }
        }

        private void OnCloseButtonClick()
        {
            PlayUISound(stampSoundId);

            // 记录玩家本次阅读的公告中最新的那一封的时间
            if (_announcements.Count > 0)
            {
                var latestTime = _announcements.Max(a => a.PublishTime);
                GameEntry.Setting.SetObject(Constant.Setting.LastReadAnnouncementTime, latestTime);
                GameEntry.Setting.Save();
            }

            // 修改红点
            var redDotModule = GameEntry.ModuleComponent.GetModule<RedDotModule>();
            if (redDotModule != null)
            {
                redDotModule.AddRedDotNode(new RedDotNode(red_dot_type.announcement, 0));
            }

            var redDotData = new Dictionary<red_dot_type, int>
            {
                { red_dot_type.announcement, 0 }
            };
            GameEntry.Event.Fire(this, ChangeRedDotEventArgs.Create(redDotData));

            Close();
        }

        private void StopBannerLoad()
        {
            if (_bannerCoroutine != null)
            {
                StopCoroutine(_bannerCoroutine);
                _bannerCoroutine = null;
            }
        }

        private void ReleaseBannerTexture()
        {
            if (_loadedBannerTexture != null)
            {
                Destroy(_loadedBannerTexture);
                _loadedBannerTexture = null;
            }

            if (detailPanel != null && detailPanel.BannerImage != null)
            {
                detailPanel.BannerImage.texture = null;
            }
        }

        private static string GetTagLocalizationKey(int tag)
        {
            return tag switch
            {
                1 => "Tag.New",
                2 => "Tag.Hot",
                3 => "Tag.Maintenance",
                _ => null
            };
        }

        private static void SortAnnouncements(List<AnnouncementData> list)
        {
            list.Sort((a, b) =>
            {
                var scoreA = a.Priority * PriorityScoreFactor + a.PublishTime;
                var scoreB = b.Priority * PriorityScoreFactor + b.PublishTime;
                return scoreB.CompareTo(scoreA);
            });
        }

        private static bool TryParseAnnouncements(string json, List<AnnouncementData> output)
        {
            output.Clear();
            if (string.IsNullOrWhiteSpace(json))
            {
                return false;
            }

            try
            {
                var trimmed = json.TrimStart();
                AnnouncementDto[] dtos = null;

                if (trimmed.StartsWith("[", StringComparison.Ordinal))
                {
                    dtos = Utility.Json.ToObject<AnnouncementDto[]>(json);
                }
                else
                {
                    var response = Utility.Json.ToObject<AnnouncementListResponse>(json);
                    dtos = response?.announcements ?? response?.data ?? response?.list;
                    if (dtos == null && response?.data_wrapper != null)
                    {
                        dtos = response.data_wrapper.list;
                    }
                }

                if (dtos == null || dtos.Length == 0)
                {
                    return true;
                }

                foreach (var dto in dtos)
                {
                    if (dto == null)
                    {
                        continue;
                    }

                    long timestamp = 0;
                    var timeStr = !string.IsNullOrEmpty(dto.publish_time) ? dto.publish_time : dto.publish_time_str;
                    if (!string.IsNullOrEmpty(timeStr))
                    {
                        if (long.TryParse(timeStr, out var parsedLong))
                        {
                            timestamp = parsedLong;
                        }
                        else if (DateTime.TryParse(timeStr, out var parsedDt))
                        {
                            timestamp = new DateTimeOffset(parsedDt).ToUnixTimeMilliseconds();
                        }
                    }

                    output.Add(new AnnouncementData
                    {
                        Id = dto.id,
                        Title = dto.title ?? string.Empty,
                        Tag = dto.tag,
                        BannerUrl = dto.banner_url,
                        Content = dto.content ?? string.Empty,
                        Priority = Mathf.Clamp(dto.priority, 0, 99),
                        PublishTime = timestamp
                    });
                }

                return true;
            }
            catch (Exception e)
            {
                Log.Warning("Parse announcements json failed: {0}", e.Message);
                return false;
            }
        }

        public static int GetMaxAnnouncementId(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return 0;
            }

            try
            {
                var trimmed = json.TrimStart();
                AnnouncementDto[] dtos = null;

                if (trimmed.StartsWith("[", StringComparison.Ordinal))
                {
                    dtos = Utility.Json.ToObject<AnnouncementDto[]>(json);
                }
                else
                {
                    var response = Utility.Json.ToObject<AnnouncementListResponse>(json);
                    dtos = response?.announcements ?? response?.data ?? response?.list;
                    if (dtos == null && response?.data_wrapper != null)
                    {
                        dtos = response.data_wrapper.list;
                    }
                }

                if (dtos == null || dtos.Length == 0)
                {
                    return 0;
                }

                var maxId = 0;
                foreach (var dto in dtos)
                {
                    if (dto != null && dto.id > maxId)
                    {
                        maxId = dto.id;
                    }
                }

                return maxId;
            }
            catch (Exception e)
            {
                Log.Warning("GetMaxAnnouncementId failed: {0}", e.Message);
                return 0;
            }
        }


        [Serializable]
        private class AnnouncementListResponse
        {
            public AnnouncementDto[] announcements;
            public AnnouncementDto[] data;
            public AnnouncementDto[] list;
            public AnnouncementDataWrapper data_wrapper;
        }

        [Serializable]
        private class AnnouncementDataWrapper
        {
            public AnnouncementDto[] list;
        }

        [Serializable]
        private class AnnouncementDto
        {
            public int id;
            public string title;
            public int tag;
            public string banner_url;
            public string content;
            public int priority;
            public string publish_time;
            public string publish_time_str;
        }

        private class AnnouncementData
        {
            public string BannerUrl;
            public string Content;
            public int Id;
            public int Priority;
            public long PublishTime;
            public int Tag;
            public string Title;
        }
    }
}