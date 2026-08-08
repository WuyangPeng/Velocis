using Game.Scripts.Main.Runtime.Sound;
using GameEntry = Game.Scripts.Main.Runtime.Base.GameEntry;

namespace Game.Scripts.Hotfix.HotfixBusiness.UI.Common
{
    public class VolumeBinder
    {
        private readonly string _soundGroupName;
        private readonly VolumeControl _volumeControl;
        private bool _initMuted;
        private float _initVolume;

        public VolumeBinder(string soundGroupName, VolumeControl volumeControl)
        {
            _soundGroupName = soundGroupName;
            _volumeControl = volumeControl;
        }

        public void Initialize()
        {
            _initMuted = GameEntry.Sound.IsMuted(_soundGroupName);
            _initVolume = GameEntry.Sound.GetVolume(_soundGroupName);

            _volumeControl.MuteToggle.isOn = !_initMuted;
            _volumeControl.VolumeSlider.value = _initVolume;
            _volumeControl.VolumeSlider.gameObject.SetActive(!_initMuted);
        }

        public void OnMuteChanged(bool isOn)
        {
            GameEntry.Sound.Mute(_soundGroupName, !isOn);
            _volumeControl.VolumeSlider.gameObject.SetActive(isOn);
        }

        public void OnVolumeChanged(float volume)
        {
            GameEntry.Sound.SetVolume(_soundGroupName, volume);
        }

        public void Rollback()
        {
            GameEntry.Sound.Mute(_soundGroupName, _initMuted);
            GameEntry.Sound.SetVolume(_soundGroupName, _initVolume);
        }
    }
}