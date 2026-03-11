using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Main.Runtime.Sound;
using GameFramework.Procedure;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.UI.UICommon
{
    public abstract class UGuiForm : UIFormLogic
    {
        private const int DepthFactor = 100;
        private const float FadeTime = 0.3f;

        private static Font _mainFont;
        private static TMP_FontAsset _mainTMPFont;
        private readonly List<Canvas> _cachedCanvasContainer = new();
        private Canvas _cachedCanvas;
        private CanvasGroup _canvasGroup;

        public int OriginalDepth { get; private set; }

        public int Depth => _cachedCanvas.sortingOrder;

        public void Close()
        {
            Close(false);
        }

        public void Close(bool ignoreFade)
        {
            StopAllCoroutines();

            if (ignoreFade)
            {
                GameEntry.UI.CloseUIForm(this);
            }
            else
            {
                StartCoroutine(CloseCo(FadeTime));
            }
        }

        public void PlayUISound(int uiSoundId)
        {
            GameEntry.Sound.PlayUISound(uiSoundId);
        }

        public static void SetMainFont(Font mainFont)
        {
            if (mainFont == null)
            {
                Log.Error("Main font is invalid.");
                return;
            }

            _mainFont = mainFont;
        }

        public static void SetMainTMPFont(TMP_FontAsset mainTMPFont)
        {
            if (mainTMPFont == null)
            {
                Log.Error("Main TMP font is invalid.");
                return;
            }

            _mainTMPFont = mainTMPFont;
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            _cachedCanvas = gameObject.GetOrAddComponent<Canvas>();
            _cachedCanvas.overrideSorting = true;
            OriginalDepth = _cachedCanvas.sortingOrder;

            _canvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();

            var rectTransform = GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;

            gameObject.GetOrAddComponent<GraphicRaycaster>();

            var texts = GetComponentsInChildren<Text>(true);
            foreach (var text in texts)
            {
                text.font = _mainFont;
                if (!string.IsNullOrEmpty(text.text))
                {
                    text.text = GameEntry.Localization.GetString(text.text);
                }
            }

            var tmpTexts = GetComponentsInChildren<TMP_Text>(true);
            foreach (var tmpText in tmpTexts)
            {
                if (_mainTMPFont != null)
                {
                    tmpText.font = _mainTMPFont;
                }

                if (!string.IsNullOrEmpty(tmpText.text))
                {
                    tmpText.text = GameEntry.Localization.GetString(tmpText.text);
                }
            }
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            _canvasGroup.alpha = 0f;
            StopAllCoroutines();
            StartCoroutine(_canvasGroup.FadeToAlpha(1f, FadeTime));
        }

        protected override void OnResume()
        {
            base.OnResume();

            _canvasGroup.alpha = 0f;
            StopAllCoroutines();
            StartCoroutine(_canvasGroup.FadeToAlpha(1f, FadeTime));
        }

        protected override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
        {
            var oldDepth = Depth;
            base.OnDepthChanged(uiGroupDepth, depthInUIGroup);
            var deltaDepth = UGuiGroupHelper.DepthFactor * uiGroupDepth + DepthFactor * depthInUIGroup - oldDepth + OriginalDepth;
            GetComponentsInChildren(true, _cachedCanvasContainer);
            foreach (var container in _cachedCanvasContainer)
            {
                container.sortingOrder += deltaDepth;
            }

            _cachedCanvasContainer.Clear();
        }

        private IEnumerator CloseCo(float duration)
        {
            yield return _canvasGroup.FadeToAlpha(0f, duration);
            GameEntry.UI.CloseUIForm(this);
        }

        protected ProcedureBase GetCurrentProcedure()
        {
            return GameEntry.Procedure.CurrentProcedure;
        }
    }
}