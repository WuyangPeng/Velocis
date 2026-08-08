using System;
using Game.Scripts.Hotfix.HotfixBusiness.Tools.Button;
using Game.Scripts.Main.Runtime.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Hotfix.HotfixBusiness.UI.Common
{
    public class DropdownControl : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI labelText;
        [SerializeField] private TMP_Dropdown dropdown;

        [Header("音效设置")] [SerializeField] [UISoundId]
        private int clickSoundId;

        [SerializeField] [UISoundId] private int selectSoundId;

        [Header("悬停金光粒子")] [SerializeField] private GameObject sparkleTemplate;

        [SerializeField] private int sparkleParticleCount = 28;

        private Image[] _hoverParticles;
        private Coroutine _particleCoroutine;

        public TextMeshProUGUI LabelText => labelText;
        public TMP_Dropdown Dropdown => dropdown;

        private void Awake()
        {
            if (dropdown != null && sparkleTemplate != null)
            {
                _hoverParticles = ButtonHoverParticleUtility.EnsureHoverParticles(dropdown.transform, sparkleTemplate, sparkleParticleCount);
                ButtonHoverParticleUtility.Reset(this, ref _particleCoroutine, _hoverParticles);
            }
        }

        private void Start()
        {
            if (dropdown != null)
            {
                dropdown.onValueChanged.AddListener(OnValueChanged);

                // 动态给 dropdown 物体挂载按下事件监听组件以播放点击音效
                var clickHandler = dropdown.gameObject.GetOrAddComponent<DropdownPointerDownHandler>();
                clickHandler.onDown = PlayClickSound;

                // 动态给 dropdown 物体挂载悬停监听组件以播放金光粒子
                var hoverHandler = dropdown.gameObject.GetOrAddComponent<DropdownHoverHandler>();
                hoverHandler.onEnter = PlayHoverVisual;
                hoverHandler.onExit = StopHoverVisual;
            }
        }

        private void OnDisable()
        {
            StopHoverVisual();
        }

        private void OnDestroy()
        {
            if (dropdown != null)
            {
                dropdown.onValueChanged.RemoveListener(OnValueChanged);
            }
        }

        private void PlayClickSound()
        {
            if (clickSoundId > 0 && GameEntry.Sound != null)
            {
                GameEntry.Sound.PlayUISound(clickSoundId);
            }
        }

        private void OnValueChanged(int value)
        {
            if (selectSoundId > 0 && GameEntry.Sound != null)
            {
                GameEntry.Sound.PlayUISound(selectSoundId);
            }
        }

        private void PlayHoverVisual()
        {
            if (_hoverParticles is { Length: > 0 })
            {
                ButtonHoverParticleUtility.PlayHover(this, ref _particleCoroutine, _hoverParticles, 0.95f);
            }
        }

        private void StopHoverVisual()
        {
            if (_hoverParticles is { Length: > 0 })
            {
                ButtonHoverParticleUtility.StopHover(this, ref _particleCoroutine, _hoverParticles);
            }
        }

        private class DropdownPointerDownHandler : MonoBehaviour, IPointerDownHandler
        {
            public Action onDown;

            public void OnPointerDown(PointerEventData eventData)
            {
                if (eventData.button == PointerEventData.InputButton.Left)
                {
                    onDown?.Invoke();
                }
            }
        }

        private class DropdownHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
        {
            public Action onEnter;
            public Action onExit;

            public void OnPointerEnter(PointerEventData eventData)
            {
                onEnter?.Invoke();
            }

            public void OnPointerExit(PointerEventData eventData)
            {
                onExit?.Invoke();
            }
        }
    }
}