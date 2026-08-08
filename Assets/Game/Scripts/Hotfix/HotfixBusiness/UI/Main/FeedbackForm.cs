// 创建时间：2026-07-24
// 修改时间：2026-07-24

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Game.Scripts.Hotfix.HotfixBusiness.UI.Common;
using Game.Scripts.Hotfix.HotfixBusiness.Filter;
using Game.Scripts.Main.Runtime.Platform;
using Game.Scripts.Main.Runtime.Definition.Constant;
using Game.Scripts.Main.Runtime.UI.UICommon;
using Game.Scripts.Main.Runtime.UI.UIMenu;
using Game.Scripts.Main.Runtime.Utils;
using GameFramework;
using GameFramework.Event;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Hotfix.HotfixBusiness.UI.Main
{
    /// <summary>
    /// 求助军师（问题反馈）界面。
    /// </summary>
    public class FeedbackForm : UGuiForm
    {
        private const int MinLength = 10;
        private const int MaxLength = 500;
        private const int CooldownSeconds = 60;
        private const int MaxImageBytes = 5 * 1024 * 1024;
        private const int SubmitTimeoutSeconds = 15;

        // ──────────────────────────────────────────────
        // 序列化字段
        // ──────────────────────────────────────────────
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private BaseButton[] typeButtons;
        [SerializeField] private TMP_Text[] typeButtonLabels;
        [SerializeField] private Image[] typeButtonImages;
        [SerializeField] private Sprite typeNormalSprite;
        [SerializeField] private Sprite typeSelectedSprite;
        [SerializeField] private TMP_InputField contentInput;
        [SerializeField] private TMP_Text charCountText;
        [SerializeField] private BaseButton uploadButton;
        [SerializeField] private BaseButton deleteImageButton;
        [SerializeField] private RawImage screenshotPreview;
        [SerializeField] private GameObject screenshotPlaceholder;
        [SerializeField] private BaseButton submitButton;
        [SerializeField] private TMP_Text submitButtonText;
        [SerializeField] private BaseButton cancelButton;
        [SerializeField] private int openSoundId = 100018;
        [SerializeField] private int submitSoundId = 100019;
        [SerializeField] private int closeSoundId = 100020;
        [SerializeField] private int cooldownSoundId = 100021;
        [SerializeField] private LabeledInputFieldControl serverControl;
        [SerializeField] private LabeledInputFieldControl playerIdControl;
        [SerializeField] private ToggleControl anonymousToggle;

        private int _selectedType;
        private byte[] _imageBytes;
        private Texture2D _previewTexture;
        private bool _isSubmitting;
        private int _serialId;
        private int _lastCooldownRemain = -1;
        private Coroutine _submitCoroutine;

        private static readonly string[] TypeKeys =
        {
            "Feedback.Type.Bug",
            "Feedback.Type.Suggestion",
            "Feedback.Type.Report"
        };

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Event.Subscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
            GameEntry.Event.Subscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFailure);
            _isSubmitting = false;
            InitUI();
            PlayUISound(openSoundId);
            _lastCooldownRemain = -1;
            UpdateCooldownState();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            GameEntry.Event.Unsubscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
            GameEntry.Event.Unsubscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFailure);
            StopSubmit();
            _lastCooldownRemain = -1;

            ClearImage();
            base.OnClose(isShutdown, userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            UpdateCooldownState();
        }

        private void InitUI()
        {
            if (titleText != null)
            {
                titleText.text = GameEntry.Localization.GetString("FeedbackForm.Title");
            }

            if (contentInput != null)
            {
                contentInput.characterLimit = MaxLength;
                contentInput.lineType = TMP_InputField.LineType.MultiLineNewline;
                contentInput.onValueChanged.RemoveListener(OnContentChanged);
                contentInput.onValueChanged.AddListener(OnContentChanged);
                contentInput.text = string.Empty;
            }

            BindTypeButtons();
            SelectType(0);

            if (uploadButton != null)
            {
                uploadButton.OnClick.RemoveAllListeners();
                uploadButton.OnClick.AddListener(OnUploadButtonClick);
            }

            if (deleteImageButton != null)
            {
                deleteImageButton.OnClick.RemoveAllListeners();
                deleteImageButton.OnClick.AddListener(OnDeleteImageButtonClick);
                deleteImageButton.gameObject.SetActive(false);
            }

            if (submitButton != null)
            {
                submitButton.OnClick.RemoveAllListeners();
                submitButton.OnClick.AddListener(OnSubmitButtonClick);
            }

            if (cancelButton != null)
            {
                cancelButton.OnClick.RemoveAllListeners();
                cancelButton.OnClick.AddListener(OnCancelButtonClick);
            }

            if (screenshotPreview != null)
            {
                screenshotPreview.gameObject.SetActive(false);
            }

            if (screenshotPlaceholder != null)
            {
                screenshotPlaceholder.SetActive(true);
            }

            if (anonymousToggle != null)
            {
                anonymousToggle.Toggle.onValueChanged.RemoveListener(OnAnonymousToggleChanged);
                anonymousToggle.Toggle.onValueChanged.AddListener(OnAnonymousToggleChanged);
                anonymousToggle.Toggle.isOn = false;
            }

            if (serverControl != null)
            {
                if (serverControl.LabelText != null)
                {
                    serverControl.LabelText.text = GameEntry.Localization.GetString("FeedbackForm.ServerLabel");
                }
                if (serverControl.InputField != null)
                {
                    serverControl.InputField.text = string.Empty;
                    serverControl.InputField.interactable = true;
                    if (serverControl.InputField.placeholder is TMP_Text serverPlaceholder)
                    {
                        serverPlaceholder.text = GameEntry.Localization.GetString("FeedbackForm.ServerPlaceholder");
                    }
                }
            }

            if (playerIdControl != null)
            {
                if (playerIdControl.LabelText != null)
                {
                    playerIdControl.LabelText.text = GameEntry.Localization.GetString("FeedbackForm.PlayerIdLabel");
                }
                if (playerIdControl.InputField != null)
                {
                    playerIdControl.InputField.text = string.Empty;
                    playerIdControl.InputField.interactable = true;
                    if (playerIdControl.InputField.placeholder is TMP_Text playerPlaceholder)
                    {
                        playerPlaceholder.text = GameEntry.Localization.GetString("FeedbackForm.PlayerIdPlaceholder");
                    }
                }
            }

            RefreshCharCount();
            RefreshSubmitInteractable();
        }

        private void BindTypeButtons()
        {
            if (typeButtons == null)
            {
                return;
            }

            for (var i = 0; i < typeButtons.Length; i++)
            {
                var index = i;
                var button = typeButtons[i];
                if (button == null)
                {
                    continue;
                }

                if (typeButtonLabels != null && i < typeButtonLabels.Length && typeButtonLabels[i] != null && i < TypeKeys.Length)
                {
                    typeButtonLabels[i].text = GameEntry.Localization.GetString(TypeKeys[i]);
                }

                button.OnClick.RemoveAllListeners();
                button.OnClick.AddListener(() => SelectType(index));
            }
        }

        private void SelectType(int type)
        {
            _selectedType = Mathf.Clamp(type, 0, TypeKeys.Length - 1);
            if (typeButtons == null)
            {
                return;
            }

            for (var i = 0; i < typeButtons.Length; i++)
            {
                var button = typeButtons[i];
                if (button == null)
                {
                    continue;
                }

                var selected = i == _selectedType;
                if (typeButtonImages != null && i < typeButtonImages.Length && typeButtonImages[i] != null)
                {
                    var sprite = selected ? typeSelectedSprite : typeNormalSprite;
                    if (sprite != null)
                    {
                        typeButtonImages[i].sprite = sprite;
                    }
                }

                if (typeButtonLabels != null && i < typeButtonLabels.Length && typeButtonLabels[i] != null)
                {
                    typeButtonLabels[i].color = selected
                        ? new Color(0.17f, 0.12f, 0.08f, 1f)
                        : Color.white;
                }
            }
        }

        private void OnContentChanged(string _)
        {
            RefreshCharCount();
            RefreshSubmitInteractable();
        }

        private void RefreshCharCount()
        {
            if (charCountText == null)
            {
                return;
            }

            var length = contentInput != null ? contentInput.text.Length : 0;
            charCountText.text = GameEntry.Localization.GetString("FeedbackForm.CharCountFormat", length, MaxLength);
        }

        private void OnUploadButtonClick()
        {
            Log.Info("[FeedbackForm] OnUploadButtonClick triggered.");
            if (_isSubmitting)
            {
                return;
            }

            var title = GameEntry.Localization.GetString("FeedbackForm.SelectImageTitle");
            var path = EditorPlatformUtility.OpenFilePanel(title, "", "png,jpg,jpeg");
            Log.Info("[FeedbackForm] Selected image path: '{0}'.", path);
            if (!string.IsNullOrEmpty(path))
            {
                TryLoadImageFromFile(path);
            }
        }

        private void TryLoadImageFromFile(string path)
        {
            try
            {
                var bytes = File.ReadAllBytes(path);
                if (bytes.Length > MaxImageBytes)
                {
                    ShowMessage("Feedback.ImageTooLarge");
                    return;
                }

                var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                if (!texture.LoadImage(bytes))
                {
                    Destroy(texture);
                    ShowMessage("Feedback.ImageLoadFailed");
                    return;
                }

                SetImage(bytes, texture);
            }
            catch (Exception e)
            {
                Log.Warning("Load feedback image failed: {0}", e.Message);
                ShowMessage("Feedback.ImageLoadFailed");
            }
        }

        private void SetImage(byte[] bytes, Texture2D texture)
        {
            ClearImage(keepPreviewInactive: true);

            byte[] compressedJpg = null;
            try
            {
                compressedJpg = texture.EncodeToJPG(75);
            }
            catch (Exception e)
            {
                Log.Warning("Compress image to JPG failed, fallback to raw bytes: {0}", e.Message);
            }

            _imageBytes = compressedJpg ?? bytes;
            _previewTexture = texture;

            if (screenshotPreview != null)
            {
                screenshotPreview.texture = texture;
                screenshotPreview.gameObject.SetActive(true);
            }

            if (screenshotPlaceholder != null)
            {
                screenshotPlaceholder.SetActive(false);
            }

            if (deleteImageButton != null)
            {
                deleteImageButton.gameObject.SetActive(true);
            }
        }

        private void OnDeleteImageButtonClick()
        {
            ClearImage();
        }

        private void ClearImage(bool keepPreviewInactive = false)
        {
            _imageBytes = null;
            if (_previewTexture != null)
            {
                Destroy(_previewTexture);
                _previewTexture = null;
            }

            if (screenshotPreview != null)
            {
                screenshotPreview.texture = null;
                if (!keepPreviewInactive)
                {
                    screenshotPreview.gameObject.SetActive(false);
                }
            }

            if (!keepPreviewInactive && screenshotPlaceholder != null)
            {
                screenshotPlaceholder.SetActive(true);
            }

            if (deleteImageButton != null)
            {
                deleteImageButton.gameObject.SetActive(false);
            }
        }

        private void OnCancelButtonClick()
        {
            PlayUISound(closeSoundId);
            Close();
        }

        private void OnAnonymousToggleChanged(bool isAnonymous)
        {
            if (serverControl != null && serverControl.InputField != null)
            {
                serverControl.InputField.interactable = !isAnonymous;
                if (isAnonymous)
                {
                    serverControl.InputField.text = string.Empty;
                }
            }

            if (playerIdControl != null && playerIdControl.InputField != null)
            {
                playerIdControl.InputField.interactable = !isAnonymous;
                if (isAnonymous)
                {
                    playerIdControl.InputField.text = string.Empty;
                }
            }
        }

        private void OnSubmitButtonClick()
        {
            if (_isSubmitting)
            {
                return;
            }

            var remain = GetCooldownRemainSeconds();
            if (remain > 0)
            {
                ShowMessage("Feedback.CooldownError", remain);
                return;
            }

            var content = contentInput != null ? contentInput.text.Trim() : string.Empty;
            if (content.Length < MinLength)
            {
                ShowMessage("Feedback.MinLengthError");
                return;
            }

            if (content.Length > MaxLength)
            {
                return;
            }

            var isAnonymous = anonymousToggle != null && anonymousToggle.Toggle.isOn;
            if (!isAnonymous)
            {
                var serverName = (serverControl != null && serverControl.InputField != null) ? serverControl.InputField.text.Trim() : string.Empty;
                var playerIdStr = (playerIdControl != null && playerIdControl.InputField != null) ? playerIdControl.InputField.text.Trim() : string.Empty;

                if (string.IsNullOrEmpty(serverName))
                {
                    ShowMessage("Feedback.ServerEmptyError");
                    return;
                }

                if (string.IsNullOrEmpty(playerIdStr))
                {
                    ShowMessage("Feedback.PlayerIdEmptyError");
                    return;
                }
            }

            var apiUrl = GameEntry.Account?.FeedbackUrl;
            if (string.IsNullOrEmpty(apiUrl))
            {
                apiUrl = GameEntry.BuiltinData?.BuildInfo?.FeedbackUrl;
            }

            if (string.IsNullOrEmpty(apiUrl))
            {
                Log.Error("FeedbackUrl is empty, cannot submit feedback.");
                ShowMessage("Feedback.SubmitFailed");
                return;
            }

            var serverStr = isAnonymous ? string.Empty : ((serverControl != null && serverControl.InputField != null) ? serverControl.InputField.text.Trim() : string.Empty);
            var playerStr = isAnonymous ? string.Empty : ((playerIdControl != null && playerIdControl.InputField != null) ? playerIdControl.InputField.text.Trim() : string.Empty);
            var typeStr = _selectedType.ToString();
            var deviceInfo = SystemInfo.deviceModel ?? string.Empty;
            var appId = GameEntry.Account.appId;
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            var imageBase64 = _imageBytes != null && _imageBytes.Length > 0 ? Convert.ToBase64String(_imageBytes) : string.Empty;
            var imageHash = string.Empty;
            if (!string.IsNullOrEmpty(imageBase64))
            {
                using (var md5 = System.Security.Cryptography.MD5.Create())
                {
                    var hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(imageBase64));
                    imageHash = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
                }
            }
            var sign = HmacSha256Util.ComputeHash(GameEntry.Account.secret,
                appId,
                typeStr,
                content,
                imageHash,
                deviceInfo,
                serverStr,
                playerStr,
                isAnonymous ? "1" : "0",
                timestamp);

            var queryParams = new Dictionary<string, string>
            {
                { "type", typeStr },
                { "content", content },
                { "image_data", imageBase64 },
                { "device_info", deviceInfo },
                { "server", serverStr },
                { "player_id", playerStr },
                { "is_anonymous", isAnonymous ? "1" : "0" },
                { "app_id", appId },
                { "timestamp", timestamp },
                { "sign", sign }
            };
            var queryString = string.Join("&",
                queryParams.Select(kvp => $"{SafeEscapeDataString(kvp.Key)}={SafeEscapeDataString(kvp.Value)}"));
            var postData = Encoding.UTF8.GetBytes(queryString);

            _isSubmitting = true;
            RefreshSubmitInteractable();

            Log.Info("Feedback submit web request (POST) starting: {0}", apiUrl);
            _serialId = GameEntry.WebRequest.AddWebRequest(apiUrl, postData, this);
            if (_serialId == 0)
            {
                _isSubmitting = false;
                RefreshSubmitInteractable();
                ShowMessage("Feedback.SubmitFailed");
            }
        }

        private void OnWebRequestSuccess(object sender, GameEventArgs args)
        {
            var successArgs = (WebRequestSuccessEventArgs)args;
            if (successArgs.UserData as FeedbackForm != this)
            {
                return;
            }

            _isSubmitting = false;
            var responseBytes = successArgs.GetWebResponseBytes();
            var json = responseBytes != null ? Encoding.UTF8.GetString(responseBytes) : string.Empty;
            var response = Utility.Json.ToObject<FeedbackSubmitResponse>(json);
            if (response == null || response.status != 1)
            {
                RefreshSubmitInteractable();
                int statusCode = response != null ? response.status : 109;
                ShowMessage("ServerError." + statusCode);
                return;
            }

            StartCoroutine(FinishSubmitSuccess());
        }

        private void OnWebRequestFailure(object sender, GameEventArgs args)
        {
            var failureArgs = (WebRequestFailureEventArgs)args;
            if (failureArgs.UserData as FeedbackForm != this)
            {
                return;
            }

            _isSubmitting = false;
            RefreshSubmitInteractable();
            ShowMessage("Feedback.SubmitFailed");
        }



        private static string SafeEscapeDataString(string str)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;

            if (str.Length <= 30000)
            {
                return Uri.EscapeDataString(str).Replace("+", "%2B");
            }

            var sb = new StringBuilder(str.Length + 1024);
            const int chunkSize = 20000;
            for (int i = 0; i < str.Length; i += chunkSize)
            {
                int len = Math.Min(chunkSize, str.Length - i);
                sb.Append(Uri.EscapeDataString(str.Substring(i, len)).Replace("+", "%2B"));
            }
            return sb.ToString();
        }

        private IEnumerator FinishSubmitSuccess()
        {
            PlayUISound(submitSoundId);
            MarkSubmitTime();
            ShowMessage("Feedback.SubmitSuccess");

            // 清空输入框内容与已传图片
            if (contentInput != null)
            {
                contentInput.text = string.Empty;
            }
            ClearImage();
            SelectType(0);

            yield return new WaitForSeconds(0.6f);
            _isSubmitting = false;
            Close(true);
        }

        private void MarkSubmitTime()
        {
            GameEntry.Setting.SetString(Constant.Setting.FeedbackLastSubmitTime, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString());
            GameEntry.Setting.Save();
            _lastCooldownRemain = -1;
            UpdateCooldownState();
        }

        private long GetLastSubmitUnix()
        {
            var raw = GameEntry.Setting.GetString(Constant.Setting.FeedbackLastSubmitTime, "0");
            return long.TryParse(raw, out var value) ? value : 0L;
        }

        private int GetCooldownRemainSeconds()
        {
            var last = GetLastSubmitUnix();
            if (last <= 0)
            {
                return 0;
            }

            var elapsed = DateTimeOffset.UtcNow.ToUnixTimeSeconds() - last;
            var remain = CooldownSeconds - (int)elapsed;
            return remain > 0 ? remain : 0;
        }

        private void UpdateCooldownState()
        {
            var remain = GetCooldownRemainSeconds();
            if (remain != _lastCooldownRemain)
            {
                var isTick = _lastCooldownRemain > 0 && remain > 0;
                _lastCooldownRemain = remain;
                if (remain > 0)
                {
                    if (submitButtonText != null)
                    {
                        submitButtonText.text = GameEntry.Localization.GetString("FeedbackForm.CooldownFormat", remain);
                    }

                    if (isTick)
                    {
                        PlayUISound(cooldownSoundId);
                    }
                }
                else
                {
                    RestoreSubmitButtonLabel();
                }

                RefreshSubmitInteractable();
            }
        }

        private void RestoreSubmitButtonLabel()
        {
            if (submitButtonText != null)
            {
                submitButtonText.text = GameEntry.Localization.GetString("FeedbackForm.BtnSubmit");
            }
        }

        private void RefreshSubmitInteractable()
        {
            if (submitButton == null)
            {
                return;
            }

            var length = contentInput != null ? contentInput.text.Trim().Length : 0;
            var lengthOk = length >= MinLength && length <= MaxLength;
            var cooling = GetCooldownRemainSeconds() > 0;
            var enabled = !_isSubmitting && lengthOk && !cooling;
            submitButton.enabled = enabled;

            var canvasGroup = submitButton.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = enabled ? 1f : 0.45f;
            }
        }

        private void StopSubmit()
        {
            if (_submitCoroutine != null)
            {
                StopCoroutine(_submitCoroutine);
                _submitCoroutine = null;
            }

            _isSubmitting = false;
        }

        private void ShowMessage(string key, params object[] args)
        {
            var message = args != null && args.Length > 0
                ? GameEntry.Localization.GetString(key, args)
                : GameEntry.Localization.GetString(key);
            OpenSimpleDialog(message);
        }

        private void OpenSimpleDialog(string message)
        {
            GameEntry.UI.OpenDialog(new DialogParams
            {
                Mode = 1,
                Title = GameEntry.Localization.GetString("FeedbackForm.Title"),
                Message = message
            });
        }

        [Serializable]
        private class FeedbackSubmitResponse
        {
            public int status;
            public string message;
        }
    }
}
