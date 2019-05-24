using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour, IOnMuteCallback
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

    private void Start()
    {
        audioController.LoadAudioSettings();
    }

    private void OnEnable()
    {
        muteToggle.isOn = audioController.Muted;
        masterSlider.value = audioController.MasterVolume;
        musicSlider.value = audioController.MusicVolume;
        sfxSlider.value = audioController.SFXVolume;
        audioController.RegisterCallBack(this);
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


public  interface IOnMuteCallback
{
    void AudioMuted(bool muted);
}