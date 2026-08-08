using Game.Scripts.Hotfix.HotfixBusiness.Tools.Button;
using Game.Scripts.Main.Runtime.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Hotfix.HotfixBusiness.UI.Common
{
    public class VolumeControl : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI labelText;
        [SerializeField] private Toggle muteToggle;
        [SerializeField] private SliderControl volumeSlider;

        [SerializeField] [UISoundId] private int switchSoundId;

        private bool _isReady;

        public TextMeshProUGUI LabelText => labelText;
        public Toggle MuteToggle => muteToggle;
        public Slider VolumeSlider => volumeSlider != null ? volumeSlider.Slider : null;

        private void Update()
        {
            _isReady = true;
        }

        public void PlaySwitchSound(bool value)
        {
            if (!_isReady)
            {
                return;
            }

            GameEntry.Sound.PlayUISound(switchSoundId);
        }
    }
}