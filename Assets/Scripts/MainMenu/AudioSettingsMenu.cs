using UnityEngine;
using UnityEngine.UI;
using AudioSettings;

namespace GameSettingsUI
{
    public class AudioSettingsMenu : MonoBehaviour, IOnMuteCallback
    {
        [SerializeField]
        AudioData audioController;
        [SerializeField]
        Toggle muteToggle;
        [SerializeField]
        Slider masterSlider;
        [SerializeField]
        Slider musicSlider;
        [SerializeField]
        Slider sfxSlider;

        public void UpdateSettings()
        {
            audioController.LoadAudioSettings();
            muteToggle.isOn = audioController.Muted;
            masterSlider.value = audioController.MasterVolume;
            musicSlider.value = audioController.MusicVolume;
            sfxSlider.value = audioController.SFXVolume;
            audioController.RegisterCallBack(this);
        }

        private void OnEnable()
        {
            UpdateSettings();   
        }
        private void OnDisable()
        {
            audioController.DeRegisterCallback(this);
        }
        public void AudioMuted(bool muted)
        {
            muteToggle.isOn = muted;
        }
    }


}