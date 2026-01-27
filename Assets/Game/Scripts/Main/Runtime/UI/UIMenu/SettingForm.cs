using Game.Scripts.Main.Runtime.Definition.Constant;
using Game.Scripts.Main.Runtime.Sound;
using Game.Scripts.Main.Runtime.UI.UICommon;
using GameFramework.Localization;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Main.Runtime.UI.UIMenu
{
    public class SettingForm : UGuiForm
    {
        [SerializeField] private Toggle musicMuteToggle;

        [SerializeField] private Slider musicVolumeSlider;

        [SerializeField] private Toggle soundMuteToggle;

        [SerializeField] private Slider soundVolumeSlider;

        [SerializeField] private Toggle uiSoundMuteToggle;

        [SerializeField] private Slider uiSoundVolumeSlider;

        [SerializeField] private CanvasGroup languageTipsCanvasGroup;

        [SerializeField] private Toggle englishToggle;

        [SerializeField] private Toggle chineseSimplifiedToggle;

        [SerializeField] private Toggle chineseTraditionalToggle;

        [SerializeField] private Toggle koreanToggle;

        private Language selectedLanguage = Language.Unspecified;

        public void OnMusicMuteChanged(bool isOn)
        {
            GameEntry.Sound.Mute("Music", !isOn);
            musicVolumeSlider.gameObject.SetActive(isOn);
        }

        public void OnMusicVolumeChanged(float volume)
        {
            GameEntry.Sound.SetVolume("Music", volume);
        }

        public void OnSoundMuteChanged(bool isOn)
        {
            GameEntry.Sound.Mute("Sound", !isOn);
            soundVolumeSlider.gameObject.SetActive(isOn);
        }

        public void OnSoundVolumeChanged(float volume)
        {
            GameEntry.Sound.SetVolume("Sound", volume);
        }

        public void OnUISoundMuteChanged(bool isOn)
        {
            GameEntry.Sound.Mute("UISound", !isOn);
            uiSoundVolumeSlider.gameObject.SetActive(isOn);
        }

        public void OnUISoundVolumeChanged(float volume)
        {
            GameEntry.Sound.SetVolume("UISound", volume);
        }

        public void OnEnglishSelected(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            selectedLanguage = Language.English;
            RefreshLanguageTips();
        }

        public void OnChineseSimplifiedSelected(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            selectedLanguage = Language.ChineseSimplified;
            RefreshLanguageTips();
        }

        public void OnChineseTraditionalSelected(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            selectedLanguage = Language.ChineseTraditional;
            RefreshLanguageTips();
        }

        public void OnKoreanSelected(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            selectedLanguage = Language.Korean;
            RefreshLanguageTips();
        }

        public void OnSubmitButtonClick()
        {
            if (selectedLanguage == GameEntry.Localization.Language)
            {
                Close();
                return;
            }

            GameEntry.Setting.SetString(Constant.Setting.Language, selectedLanguage.ToString());
            GameEntry.Setting.Save();

            GameEntry.Sound.StopMusic();
            UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Restart);
        }


        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            musicMuteToggle.isOn = !GameEntry.Sound.IsMuted("Music");
            musicVolumeSlider.value = GameEntry.Sound.GetVolume("Music");

            soundMuteToggle.isOn = !GameEntry.Sound.IsMuted("Sound");
            soundVolumeSlider.value = GameEntry.Sound.GetVolume("Sound");

            uiSoundMuteToggle.isOn = !GameEntry.Sound.IsMuted("UISound");
            uiSoundVolumeSlider.value = GameEntry.Sound.GetVolume("UISound");

            selectedLanguage = GameEntry.Localization.Language;
            switch (selectedLanguage)
            {
                case Language.English:
                    englishToggle.isOn = true;
                    break;

                case Language.ChineseSimplified:
                    chineseSimplifiedToggle.isOn = true;
                    break;

                case Language.ChineseTraditional:
                    chineseTraditionalToggle.isOn = true;
                    break;

                case Language.Korean:
                    koreanToggle.isOn = true;
                    break;
            }
        }


        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (languageTipsCanvasGroup.gameObject.activeSelf)
            {
                languageTipsCanvasGroup.alpha = 0.5f + 0.5f * Mathf.Sin(Mathf.PI * Time.time);
            }
        }

        private void RefreshLanguageTips()
        {
            languageTipsCanvasGroup.gameObject.SetActive(selectedLanguage != GameEntry.Localization.Language);
        }
    }
}