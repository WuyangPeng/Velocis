using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Main.Runtime.Sound;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.UI.UICommon
{
    public abstract class UGuiForm : UIFormLogic
    {
        public const int DepthFactor = 100;
        private const float FadeTime = 0.3f;

        private static Font s_MainFont;
        private Canvas cachedCanvas;
        private CanvasGroup canvasGroup;
        private readonly List<Canvas> cachedCanvasContainer = new();

        public int OriginalDepth
        {
            get;
            private set;
        }

        public int Depth => cachedCanvas.sortingOrder;

        public void Close()
        {
            Close(false);
        }

        public void Close(bool ignoreFade)
        {
            StopAllCoroutines();

            if (ignoreFade)
            {
                Base.GameEntry.UI.CloseUIForm(this);
            }
            else
            {
                StartCoroutine(CloseCo(FadeTime));
            }
        }

        public void PlayUISound(int uiSoundId)
        {
            Base.GameEntry.Sound.PlayUISound(uiSoundId);
        }

        public static void SetMainFont(Font mainFont)
        {
            if (mainFont == null)
            {
                Log.Error("Main font is invalid.");
                return;
            }

            UGuiForm.s_MainFont = mainFont;
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            cachedCanvas = gameObject.GetOrAddComponent<Canvas>();
            cachedCanvas.overrideSorting = true;
            OriginalDepth = cachedCanvas.sortingOrder;

            canvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();

            var rectTransform = GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;

            gameObject.GetOrAddComponent<GraphicRaycaster>();

            var texts = GetComponentsInChildren<Text>(true);
            foreach (var text in texts)
            {
                text.font = s_MainFont;
                if (!string.IsNullOrEmpty(text.text))
                {
                    text.text = Base.GameEntry.Localization.GetString(text.text);
                }
            }
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            canvasGroup.alpha = 0f;
            StopAllCoroutines();
            StartCoroutine(canvasGroup.FadeToAlpha(1f, FadeTime));
        }
        protected override void OnResume()
        {
            base.OnResume();

            canvasGroup.alpha = 0f;
            StopAllCoroutines();
            StartCoroutine(canvasGroup.FadeToAlpha(1f, FadeTime));
        }
        protected override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
        {
            var oldDepth = Depth;
            base.OnDepthChanged(uiGroupDepth, depthInUIGroup);
            var deltaDepth = UGuiGroupHelper.DepthFactor * uiGroupDepth + DepthFactor * depthInUIGroup - oldDepth + OriginalDepth;
            GetComponentsInChildren(true, cachedCanvasContainer);
            foreach (var container in cachedCanvasContainer)
            {
                container.sortingOrder += deltaDepth;
            }

            cachedCanvasContainer.Clear();
        }

        private IEnumerator CloseCo(float duration)
        {
            yield return canvasGroup.FadeToAlpha(0f, duration);
            Base.GameEntry.UI.CloseUIForm(this);
        }

        protected GameFramework.Procedure.ProcedureBase GetCurrentProcedure()
        {
            return Base.GameEntry.Procedure.CurrentProcedure;
        }
    }
}
