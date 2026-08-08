using Game.Scripts.Hotfix.HotfixBusiness.Tools.Button;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Hotfix.HotfixBusiness.UI.Common
{
    public class RectangleButton : BaseButton
    {
        private const float HoverScaleMultiplier = 1.03f;
        private const float PressScaleMultiplier = 0.97f;
        private const float HoverParticleIntensity = 0.95f;
        private const float PressParticleIntensity = 0.55f;

        [Header("悬停金光粒子")] [SerializeField] private GameObject sparkleTemplate;

        [SerializeField] private int sparkleParticleCount = 28;

        private Image[] _hoverParticles;
        private Coroutine _particleCoroutine;

        protected override void Awake()
        {
            base.Awake();

            _hoverParticles = ButtonHoverParticleUtility.EnsureHoverParticles(transform, sparkleTemplate, sparkleParticleCount);
            ButtonHoverParticleUtility.Reset(this, ref _particleCoroutine, _hoverParticles);
        }

        protected override void ApplyHoverVisual()
        {
            StartScaleTo(OriginalScale * HoverScaleMultiplier, ScaleDuration);
            ButtonHoverParticleUtility.PlayHover(this, ref _particleCoroutine, _hoverParticles, HoverParticleIntensity);
        }

        protected override void ApplyExitVisual()
        {
            StartScaleTo(OriginalScale, ScaleDuration);
            ButtonHoverParticleUtility.StopHover(this, ref _particleCoroutine, _hoverParticles);
        }

        protected override void ApplyPressVisual()
        {
            StartScaleTo(OriginalScale * PressScaleMultiplier, ScaleDuration);
            ButtonHoverParticleUtility.PlayHover(this, ref _particleCoroutine, _hoverParticles, PressParticleIntensity);
        }

        protected override void ApplyReleaseVisual()
        {
            ApplyHoverVisual();
        }

        protected override void ResetVisualState()
        {
            base.ResetVisualState();
            ButtonHoverParticleUtility.Reset(this, ref _particleCoroutine, _hoverParticles);
        }
    }
}