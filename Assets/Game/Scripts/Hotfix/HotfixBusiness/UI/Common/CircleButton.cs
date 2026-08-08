using Game.Scripts.Hotfix.HotfixBusiness.Tools.Button;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Hotfix.HotfixBusiness.UI.Common
{
    public class CircleButton : BaseButton
    {
        private const float HoverScaleMultiplier = 1.05f;
        private const float PressScaleMultiplier = 0.95f;
        private const float GlowHoverAlpha = 0.85f;
        private const float GlowPressAlpha = 0.55f;
        private const float GlowPulseMinScale = 1f;
        private const float GlowPulseMaxScale = 1.15f;
        private const float GlowPulsePeriod = 0.9f;
        [SerializeField] private TMP_Text btnText;
        [SerializeField] private Image glowImage;
        private Coroutine _glowFadeCoroutine;

        private Coroutine _glowPulseCoroutine;
        private RectTransform _glowRect;

        protected override void Awake()
        {
            base.Awake();
            if (glowImage == null)
            {
                return;
            }

            _glowRect = glowImage.rectTransform;
            ButtonHoverGlowUtility.ResetGlow(glowImage, _glowRect);
        }

        public void SetText(string text)
        {
            if (btnText != null)
            {
                btnText.text = text;
            }
        }

        public void SetTextActive(bool active)
        {
            if (btnText != null)
            {
                btnText.gameObject.SetActive(active);
            }
        }

        protected override void ApplyHoverVisual()
        {
            StartScaleTo(OriginalScale * HoverScaleMultiplier, ScaleDuration);
            ButtonHoverGlowUtility.FadeAlpha(this, ref _glowFadeCoroutine, glowImage, GlowHoverAlpha, ScaleDuration);
            ButtonHoverGlowUtility.PulseScale(this, ref _glowPulseCoroutine, _glowRect, GlowPulseMinScale, GlowPulseMaxScale, GlowPulsePeriod);
        }

        protected override void ApplyExitVisual()
        {
            StartScaleTo(OriginalScale, ScaleDuration);
            StopGlowEffects();
            ButtonHoverGlowUtility.FadeAlpha(this, ref _glowFadeCoroutine, glowImage, 0f, ScaleDuration);
        }

        protected override void ApplyPressVisual()
        {
            StartScaleTo(OriginalScale * PressScaleMultiplier, ScaleDuration);
            ButtonHoverGlowUtility.Stop(this, ref _glowPulseCoroutine);
            ButtonHoverGlowUtility.FadeAlpha(this, ref _glowFadeCoroutine, glowImage, GlowPressAlpha, ScaleDuration);
        }

        protected override void ApplyReleaseVisual()
        {
            ApplyHoverVisual();
        }

        protected override void ResetVisualState()
        {
            base.ResetVisualState();
            StopGlowEffects();
            ButtonHoverGlowUtility.ResetGlow(glowImage, _glowRect);
        }

        private void StopGlowEffects()
        {
            ButtonHoverGlowUtility.Stop(this, ref _glowFadeCoroutine);
            ButtonHoverGlowUtility.Stop(this, ref _glowPulseCoroutine);
        }
    }
}