using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
   public void SetBGMVolume(float volume) {
        audioMixer.SetFloat("bgm", volume);
   }
   public void SetSFXVolume(float volume) {
        audioMixer.SetFloat("sfx", volume);
   }
}
