using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    //Get sliders from Canvas object
    [SerializeField] private AudioMixer masterMixer;
    [SerializeField] private Slider master;
    [SerializeField] private Slider music;
    [SerializeField] private Slider sfx;
    [SerializeField] private Slider voice;

    //Centralized functoin for volume changing
    void SetBusVolume(Slider bus, string exposedVolume)
    {
        float volume = bus.value;

        //Set the volume parameter to -80, which is mute, because Mathf.Log10(0) is undefined
        //Without this conditional, if slider was set to 0, the volume would reset
        //Using Mathf.Log10(volume) because Unity's audio mixer is using the Decibel scale
        if (volume <= 0)
        {
            masterMixer.SetFloat(exposedVolume, -80);
        }
        else
        {
            masterMixer.SetFloat(exposedVolume, Mathf.Log10(volume) * 20);
        }        
    }

    public void SetMasterVolume()
    {
        SetBusVolume(master, "masterVolume");
    }

    public void SetMusicVolume()
    {
        SetBusVolume(music, "musicVolume");
    }

    public void SetSfxVolume()
    {
        SetBusVolume(sfx, "sfxVolume");
    }

    public void SetVoiceVolume()
    {
        SetBusVolume(voice, "voiceVolume");
    }
}
