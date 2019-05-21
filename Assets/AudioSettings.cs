using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour, IOnMuteCallback
{
    [SerializeField]
    AudioData audioController;
    [SerializeField]
    Toggle muteToggle;

    private void OnEnable()
    {
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