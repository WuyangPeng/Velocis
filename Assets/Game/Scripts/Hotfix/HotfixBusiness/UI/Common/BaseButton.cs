using System.Collections;
using Game.Scripts.Main.Runtime.Sound;
using Game.Scripts.Main.Runtime.UI.UICommon;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Hotfix.HotfixBusiness.UI.Common
{
    public class BaseButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        private const float DefaultFadeTime = 0.3f;
        private const float DefaultHoverAlpha = 0.7f;
        private const float DefaultClickAlpha = 0.6f;

        [Header("基础设置")] [SerializeField] private UnityEvent onHover;

        [SerializeField] private UnityEvent onClick;

        [Header("点击防连点")] [SerializeField] private float clickInterval = 0.3f;

        [Header("音效设置")] [SerializeField] [UISoundId]
        private int clickSoundId;

        [SerializeField] [UISoundId] private int hoverSoundId;

        [Header("快捷键设置")] [SerializeField] private KeyCode shortcutKey = KeyCode.None;

        [Header("缩放微动画设置")] [SerializeField] private bool useScaleAnimation = true;

        [SerializeField] private float hoverScale = 1.05f;

        [SerializeField] private float pressScale = 0.95f;

        [SerializeField] private float scaleDuration = 0.1f;

        private Coroutine _fadeCoroutine;
        private float _lastClickTime;
        private Coroutine _scaleCoroutine;

        public UnityEvent OnClick => onClick;

        private CanvasGroup CanvasGroup { get; set; }

        protected Vector3 OriginalScale { get; private set; }

        protected float HoverScale => hoverScale;
        protected float PressScale => pressScale;
        protected float ScaleDuration => scaleDuration;

        protected virtual void Awake()
        {
            CanvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();
            OriginalScale = transform.localScale;
        }

        private void OnDisable()
        {
            ResetVisualState();
        }

        private void Update()
        {
            if (shortcutKey == KeyCode.None || !Input.GetKeyDown(shortcutKey))
            {
                return;
            }

            if (!isActiveAndEnabled || !gameObject.activeInHierarchy)
            {
                return;
            }

            InvokeClick();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            ApplyPressVisual();

            InvokeClick();
        }

        private void InvokeClick()
        {
            if (!(Time.time - _lastClickTime >= clickInterval))
            {
                return;
            }

            _lastClickTime = Time.time;

            if (clickSoundId > 0 && GameEntry.Sound != null)
            {
                GameEntry.Sound.PlayUISound(clickSoundId);
            }

            onClick.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            ApplyHoverVisual();

            if (hoverSoundId > 0 && GameEntry.Sound != null)
            {
                GameEntry.Sound.PlayUISound(hoverSoundId);
            }

            onHover.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            ApplyExitVisual();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            ApplyReleaseVisual();
        }

        protected virtual void ApplyHoverVisual()
        {
            StartFadeTo(DefaultHoverAlpha, DefaultFadeTime);

            if (useScaleAnimation)
            {
                StartScaleTo(OriginalScale * hoverScale, scaleDuration);
            }
        }

        protected virtual void ApplyExitVisual()
        {
            StartFadeTo(1f, DefaultFadeTime);

            if (useScaleAnimation)
            {
                StartScaleTo(OriginalScale, scaleDuration);
            }
        }

        protected virtual void ApplyPressVisual()
        {
            CanvasGroup.alpha = DefaultClickAlpha;

            if (useScaleAnimation)
            {
                StartScaleTo(OriginalScale * pressScale, scaleDuration);
            }
        }

        protected virtual void ApplyReleaseVisual()
        {
            CanvasGroup.alpha = DefaultHoverAlpha;

            if (useScaleAnimation)
            {
                StartScaleTo(OriginalScale * hoverScale, scaleDuration);
            }
        }

        protected virtual void ResetVisualState()
        {
            StopFadeCoroutine();
            StopScaleCoroutine();
            CanvasGroup.alpha = 1f;
            transform.localScale = OriginalScale;
        }

        private void StartFadeTo(float alpha, float duration)
        {
            StopFadeCoroutine();
            _fadeCoroutine = StartCoroutine(CanvasGroup.FadeToAlpha(alpha, duration));
        }

        protected void StartScaleTo(Vector3 targetScale, float duration)
        {
            StopScaleCoroutine();
            _scaleCoroutine = StartCoroutine(ScaleTo(targetScale, duration));
        }

        private void StopFadeCoroutine()
        {
            if (_fadeCoroutine == null)
            {
                return;
            }

            StopCoroutine(_fadeCoroutine);
            _fadeCoroutine = null;
        }

        private void StopScaleCoroutine()
        {
            if (_scaleCoroutine == null)
            {
                return;
            }

            StopCoroutine(_scaleCoroutine);
            _scaleCoroutine = null;
        }

        private IEnumerator ScaleTo(Vector3 targetScale, float duration)
        {
            var time = 0f;
            var startScale = transform.localScale;
            while (time < duration)
            {
                time += Time.deltaTime;
                transform.localScale = Vector3.Lerp(startScale, targetScale, time / duration);
                yield return null;
            }

            transform.localScale = targetScale;
        }
    }
}