using Game.Scripts.Main.Runtime.Sound;
using Game.Scripts.Main.Runtime.UI.UICommon;
using GameFramework.Localization;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Main.Runtime.UI.UIMenu
{
    public class SettingForm : UGuiForm
    {
        [SerializeField]
        private Toggle musicMuteToggle;

        [SerializeField]
        private Slider musicVolumeSlider;

        [SerializeField]
        private Toggle soundMuteToggle;

        [SerializeField]
        private Slider soundVolumeSlider;

        [SerializeField]
        private Toggle uiSoundMuteToggle;

        [SerializeField]
        private Slider uiSoundVolumeSlider;

        [SerializeField]
        private CanvasGroup languageTipsCanvasGroup;

        [SerializeField]
        private Toggle englishToggle;

        [SerializeField]
        private Toggle chineseSimplifiedToggle;

        [SerializeField]
        private Toggle chineseTraditionalToggle;

        [SerializeField]
        private Toggle koreanToggle;

        private Language selectedLanguage = Language.Unspecified;

        public void OnMusicMuteChanged(bool isOn)
        {
            Base.GameEntry.Sound.Mute("Music", !isOn);
            musicVolumeSlider.gameObject.SetActive(isOn);
        }

        public void OnMusicVolumeChanged(float volume)
        {
            Base.GameEntry.Sound.SetVolume("Music", volume);
        }

        public void OnSoundMuteChanged(bool isOn)
        {
            Base.GameEntry.Sound.Mute("Sound", !isOn);
            soundVolumeSlider.gameObject.SetActive(isOn);
        }

        public void OnSoundVolumeChanged(float volume)
        {
            Base.GameEntry.Sound.SetVolume("Sound", volume);
        }

        public void OnUISoundMuteChanged(bool isOn)
        {
            Base.GameEntry.Sound.Mute("UISound", !isOn);
            uiSoundVolumeSlider.gameObject.SetActive(isOn);
        }

        public void OnUISoundVolumeChanged(float volume)
        {
            Base.GameEntry.Sound.SetVolume("UISound", volume);
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
            if (selectedLanguage == Base.GameEntry.Localization.Language)
            {
                Close();
                return;
            }

            Base.GameEntry.Setting.SetString(Definition.Constant.Constant.Setting.Language, selectedLanguage.ToString());
            Base.GameEntry.Setting.Save();

            Base.GameEntry.Sound.StopMusic();
            GameEntry.Shutdown(ShutdownType.Restart);
        }


        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            musicMuteToggle.isOn = !Base.GameEntry.Sound.IsMuted("Music");
            musicVolumeSlider.value = Base.GameEntry.Sound.GetVolume("Music");

            soundMuteToggle.isOn = !Base.GameEntry.Sound.IsMuted("Sound");
            soundVolumeSlider.value = Base.GameEntry.Sound.GetVolume("Sound");

            uiSoundMuteToggle.isOn = !Base.GameEntry.Sound.IsMuted("UISound");
            uiSoundVolumeSlider.value = Base.GameEntry.Sound.GetVolume("UISound");

            selectedLanguage = Base.GameEntry.Localization.Language;
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

                default:
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
            languageTipsCanvasGroup.gameObject.SetActive(selectedLanguage != Base.GameEntry.Localization.Language);
        }
    }
}
