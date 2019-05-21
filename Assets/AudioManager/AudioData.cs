using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu()]
public class AudioData : ScriptableObject
{
    [SerializeField]
    AudioMixer mixer;
    [SerializeField]
    AudioMixerGroup masterGroup;
    [SerializeField]
    string exposedMasterVolumeName = "MasterVolume";
    [SerializeField]
    string exposedMusicVolumeName = "MusicVolume";
    [SerializeField]
    string exposedSFXVolumeName = "SFXVolume";

    [SerializeField]
    bool muted;
    [SerializeField]
    float musicVolume;
    [SerializeField]
    float sfxVolume;
    [SerializeField]
    AnimationCurve volumeCurve;
    public bool Muted { get => muted; set {MuteAudio(value); } }
    public float MusicVolume { get => musicVolume; set { SetMusicVolume(value); } }
    public float SFXVolume { get => sfxVolume; set { SetSFXVolume(value); } }

    List<IOnMuteCallback> callbacks=new List<IOnMuteCallback>();

    public void MuteAudio(bool muted)
    {
        this.muted = muted;
        mixer.SetFloat(exposedMasterVolumeName, muted? -80: GetActualValue(MusicVolume));
        foreach (var reciever in callbacks)
        {
            reciever.AudioMuted(muted);
        }
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = value;

        mixer.SetFloat(exposedMusicVolumeName, GetActualValue(value));
        if (value == 0)
            MuteAudio(true);
        if (Muted && value > 0)
            MuteAudio(false);
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
        mixer.SetFloat(exposedSFXVolumeName, GetActualValue(value));
    }


    float GetActualValue(float input)
    {
        return volumeCurve.Evaluate(input);
    }

    public void RegisterCallBack(IOnMuteCallback reciever)
    {
        callbacks.Add(reciever);
    }
    public void DeRegisterCallback(IOnMuteCallback reciever)
    {
        callbacks.Remove(reciever);
    }
}
