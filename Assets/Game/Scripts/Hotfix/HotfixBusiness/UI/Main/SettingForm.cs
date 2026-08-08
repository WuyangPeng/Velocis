using Game.Scripts.Hotfix.HotfixBusiness.UI.Common;
using Game.Scripts.Hotfix.HotfixCommon.Platform;
using Game.Scripts.Main.Runtime.Definition.Constant;
using Game.Scripts.Main.Runtime.Sound;
using Game.Scripts.Main.Runtime.UI.UICommon;
using GameFramework.Localization;
using TMPro;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Hotfix.HotfixBusiness.UI.Main
{
    public class SettingForm : UGuiForm
    {
        [SerializeField] private VolumeControl musicVolumeControl;
        [SerializeField] private VolumeControl soundVolumeControl;
        [SerializeField] private VolumeControl uiSoundVolumeControl;

        [SerializeField] private DropdownControl languageDropdown;
        [SerializeField] private DropdownControl graphicQualityDropdown;
        [SerializeField] private ToggleControl vibrationToggleControl;

        [SerializeField] private BaseButton confirmButton;
        [SerializeField] private BaseButton cancelButton;

        [SerializeField] [UISoundId] private int optionSwitchSoundId;
        private bool _initFullscreenEnabled;

        private Language _initLanguage;
        private int _initQualityLevel;
        private bool _initVibrationEnabled;

        private bool _isInitializing;

        private VolumeBinder _musicBinder;
        private VolumeBinder _soundBinder;
        private VolumeBinder _uiSoundBinder;
        private bool _useFullscreenToggle;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            _isInitializing = true;

            InitVolumeControls();
            InitLanguageDropdown();
            InitGraphicQualityDropdown();
            InitVibrationOrFullscreenToggle();

            _isInitializing = false;
        }

        private void InitVolumeControls()
        {
            _musicBinder = new VolumeBinder("Music", musicVolumeControl);
            _soundBinder = new VolumeBinder("Sound", soundVolumeControl);
            _uiSoundBinder = new VolumeBinder("UISound", uiSoundVolumeControl);

            _musicBinder.Initialize();
            _soundBinder.Initialize();
            _uiSoundBinder.Initialize();
        }

        private void InitLanguageDropdown()
        {
            if (languageDropdown != null && languageDropdown.Dropdown != null)
            {
                languageDropdown.Dropdown.ClearOptions();
                languageDropdown.Dropdown.options.Add(new TMP_Dropdown.OptionData(GameEntry.Localization.GetString("SystemSetting.Language.ChineseSimplified")));
                languageDropdown.Dropdown.options.Add(new TMP_Dropdown.OptionData(GameEntry.Localization.GetString("SystemSetting.Language.ChineseTraditional")));
                languageDropdown.Dropdown.options.Add(new TMP_Dropdown.OptionData(GameEntry.Localization.GetString("SystemSetting.Language.English")));
                languageDropdown.Dropdown.options.Add(new TMP_Dropdown.OptionData(GameEntry.Localization.GetString("SystemSetting.Language.Korean")));
                languageDropdown.Dropdown.options.Add(new TMP_Dropdown.OptionData(GameEntry.Localization.GetString("SystemSetting.Language.Japanese")));
            }

            _initLanguage = GameEntry.Localization.Language;
            var langIndex = LanguageToDropdownIndex(_initLanguage);
            if (languageDropdown == null || languageDropdown.Dropdown == null)
            {
                return;
            }

            languageDropdown.Dropdown.value = langIndex;
            languageDropdown.Dropdown.RefreshShownValue();
        }

        private void InitGraphicQualityDropdown()
        {
            if (graphicQualityDropdown != null && graphicQualityDropdown.Dropdown != null)
            {
                graphicQualityDropdown.Dropdown.ClearOptions();
                graphicQualityDropdown.Dropdown.options.Add(new TMP_Dropdown.OptionData(GameEntry.Localization.GetString("SystemSetting.GraphicQuality.High")));
                graphicQualityDropdown.Dropdown.options.Add(new TMP_Dropdown.OptionData(GameEntry.Localization.GetString("SystemSetting.GraphicQuality.Medium")));
                graphicQualityDropdown.Dropdown.options.Add(new TMP_Dropdown.OptionData(GameEntry.Localization.GetString("SystemSetting.GraphicQuality.Low")));
            }

            _initQualityLevel = GameEntry.Setting.GetInt(Constant.Setting.GraphicQuality, 2); // 默认高品质
            var qualityIndex = QualityLevelToDropdownIndex(_initQualityLevel);
            if (graphicQualityDropdown == null || graphicQualityDropdown.Dropdown == null)
            {
                return;
            }

            graphicQualityDropdown.Dropdown.value = qualityIndex;
            graphicQualityDropdown.Dropdown.RefreshShownValue();
        }

        private void InitVibrationOrFullscreenToggle()
        {
            if (vibrationToggleControl == null)
            {
                return;
            }

            var supportsVibration = PlatformUtility.SupportsVibration;
            _useFullscreenToggle = PlatformUtility.SupportsFullscreen;

            vibrationToggleControl.gameObject.SetActive(supportsVibration || _useFullscreenToggle);

            if (!supportsVibration && !_useFullscreenToggle)
            {
                _initVibrationEnabled = false;
                _initFullscreenEnabled = false;
                return;
            }

            if (_useFullscreenToggle)
            {
                if (vibrationToggleControl.LabelText != null)
                {
                    vibrationToggleControl.LabelText.text = GameEntry.Localization.GetString("SystemSetting.Fullscreen");
                }

                _initFullscreenEnabled = GameEntry.Setting.GetBool(Constant.Setting.FullscreenEnabled, true);
                if (vibrationToggleControl.Toggle != null)
                {
                    vibrationToggleControl.Toggle.isOn = _initFullscreenEnabled;
                }

                PlatformUtility.IsFullscreen = _initFullscreenEnabled;
                return;
            }

            if (vibrationToggleControl.LabelText != null)
            {
                vibrationToggleControl.LabelText.text = GameEntry.Localization.GetString("SystemSetting.Vibration");
            }

            _initVibrationEnabled = GameEntry.Setting.GetBool(Constant.Setting.VibrationEnabled, true); // 默认启用震动
            if (vibrationToggleControl.Toggle != null)
            {
                vibrationToggleControl.Toggle.isOn = _initVibrationEnabled;
            }
        }

        // 音量与静音逻辑的实时调节
        public void OnMusicMuteChanged(bool isOn)
        {
            _musicBinder.OnMuteChanged(isOn);
        }

        public void OnMusicVolumeChanged(float volume)
        {
            _musicBinder.OnVolumeChanged(volume);
        }

        public void OnSoundMuteChanged(bool isOn)
        {
            _soundBinder.OnMuteChanged(isOn);
        }

        public void OnSoundVolumeChanged(float volume)
        {
            _soundBinder.OnVolumeChanged(volume);
        }

        public void OnUISoundMuteChanged(bool isOn)
        {
            _uiSoundBinder.OnMuteChanged(isOn);
        }

        public void OnUISoundVolumeChanged(float volume)
        {
            _uiSoundBinder.OnVolumeChanged(volume);
        }

        // 下拉框与勾选框事件
        public void OnLanguageChanged(int index)
        {
            PlayOptionSwitchSound();
        }

        public void OnGraphicQualityChanged(int index)
        {
            PlayOptionSwitchSound();
        }

        public void OnVibrationChanged(bool isOn)
        {
            PlayOptionSwitchSound();
            if (_isInitializing)
            {
                return;
            }

            if (_useFullscreenToggle)
            {
                PlatformUtility.IsFullscreen = isOn;
                return;
            }

            if (isOn)
            {
                // 如果开启震动，给予一个短暂的物理震动反馈预览
                PlatformUtility.Vibrate();
            }
        }

        // 音效控制
        private void PlayOptionSwitchSound()
        {
            if (_isInitializing)
            {
                return;
            }

            GameEntry.Sound.PlayUISound(optionSwitchSoundId);
        }

        // 确定与取消逻辑
        public void OnConfirmButtonClick()
        {
            if (SaveSettings())
            {
                Close();
            }
        }

        private bool SaveSettings()
        {
            var selectedLang = DropdownIndexToLanguage(languageDropdown.Dropdown.value);
            var selectedQuality = DropdownIndexToQualityLevel(graphicQualityDropdown.Dropdown.value);
            var selectedVibration = PlatformUtility.SupportsVibration && vibrationToggleControl != null && vibrationToggleControl.Toggle != null && vibrationToggleControl.Toggle.isOn;
            var selectedFullscreen = _useFullscreenToggle && vibrationToggleControl != null && vibrationToggleControl.Toggle != null && vibrationToggleControl.Toggle.isOn;

            // 写入本地 preferences
            GameEntry.Setting.SetString(Constant.Setting.Language, selectedLang.ToString());
            GameEntry.Setting.SetInt(Constant.Setting.GraphicQuality, selectedQuality);
            GameEntry.Setting.SetBool(Constant.Setting.VibrationEnabled, selectedVibration);
            if (_useFullscreenToggle)
            {
                GameEntry.Setting.SetBool(Constant.Setting.FullscreenEnabled, selectedFullscreen);
                PlatformUtility.IsFullscreen = selectedFullscreen;
            }

            GameEntry.Setting.Save();

            // 应用画质特效等级
            QualitySettings.SetQualityLevel(selectedQuality, true);

            // 如果语言发生了更改，则重启游戏以应用新语言字典
            if (selectedLang == _initLanguage)
            {
                return true;
            }

            GameEntry.Sound.StopMusic();
            UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Restart);
            return false;
        }

        public void OnCancelButtonClick()
        {
            _musicBinder.Rollback();
            _soundBinder.Rollback();
            _uiSoundBinder.Rollback();

            if (_useFullscreenToggle)
            {
                PlatformUtility.IsFullscreen = _initFullscreenEnabled;
            }

            Close();
        }

        // 映射辅助方法
        private static int LanguageToDropdownIndex(Language lang)
        {
            return lang switch
            {
                Language.ChineseSimplified => 0,
                Language.ChineseTraditional => 1,
                Language.English => 2,
                Language.Korean => 3,
                Language.Japanese => 4,
                _ => 0
            };
        }

        private static Language DropdownIndexToLanguage(int index)
        {
            return index switch
            {
                0 => Language.ChineseSimplified,
                1 => Language.ChineseTraditional,
                2 => Language.English,
                3 => Language.Korean,
                4 => Language.Japanese,
                _ => Language.ChineseSimplified
            };
        }

        private static int QualityLevelToDropdownIndex(int level)
        {
            return level switch
            {
                2 => 0, // High Fidelity -> 高
                1 => 1, // Balanced -> 中
                0 => 2, // Performant -> 低
                _ => 0
            };
        }

        private static int DropdownIndexToQualityLevel(int index)
        {
            return index switch
            {
                0 => 2, // 高 -> High Fidelity (2)
                1 => 1, // 中 -> Balanced (1)
                2 => 0, // 低 -> Performant (0)
                _ => 2
            };
        }
    }
}