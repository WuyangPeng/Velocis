using System;
using Game.Scripts.Hotfix.HotfixBusiness.Tools.Button;
using Game.Scripts.Main.Runtime.Sound;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Hotfix.HotfixBusiness.UI.Common
{
    public class SliderControl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Slider slider;
        [SerializeField] [UISoundId] private int dragSoundId;
        [Header("悬停金光粒子")] [SerializeField] private GameObject sparkleTemplate;
        [SerializeField] private int sparkleParticleCount = 28;

        private Image[] _hoverParticles;
        private Coroutine _particleCoroutine;
        private bool _isReady;
        private float _lastDragSoundTime;

        public Slider Slider => slider;

        private void Awake()
        {
            var particleParent = slider != null ? slider.transform : transform;
            _hoverParticles = ButtonHoverParticleUtility.EnsureHoverParticles(particleParent, sparkleTemplate, sparkleParticleCount);
            ButtonHoverParticleUtility.Reset(this, ref _particleCoroutine, _hoverParticles);
        }

        private void Start()
        {
            if (slider != null)
            {
                slider.onValueChanged.AddListener(PlayDragSound);
            }
        }

        private void Update()
        {
            _isReady = true;
        }

        private void OnDisable()
        {
            ButtonHoverParticleUtility.StopHover(this, ref _particleCoroutine, _hoverParticles);
        }

        private void OnDestroy()
        {
            if (slider != null)
            {
                slider.onValueChanged.RemoveListener(PlayDragSound);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.pointerEnter == null || slider == null)
            {
                return;
            }

            if (eventData.pointerEnter == slider.gameObject || eventData.pointerEnter.transform.IsChildOf(slider.transform))
            {
                ButtonHoverParticleUtility.PlayHover(this, ref _particleCoroutine, _hoverParticles, 0.95f);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ButtonHoverParticleUtility.StopHover(this, ref _particleCoroutine, _hoverParticles);
        }

        public void PlayDragSound(float value)
        {
            if (!_isReady)
            {
                return;
            }

            if (!(Time.time - _lastDragSoundTime >= 0.15f))
            {
                return;
            }

            _lastDragSoundTime = Time.time;
            GameEntry.Sound.PlayUISound(dragSoundId);
        }
    }
}
