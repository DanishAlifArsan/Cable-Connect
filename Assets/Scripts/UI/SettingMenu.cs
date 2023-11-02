using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
     public AudioMixer audioMixer;
     public float sfx, bgm;
     public Slider sfxSlider, bgmSlider;

     // private void Awake() {
     //      LoadVolume();
     // }

     public void SetBGMVolume(float volume) {
        audioMixer.SetFloat("bgm", volume);
     }
     public void SetSFXVolume(float volume) {
        audioMixer.SetFloat("sfx", volume);
     }

      private void OnDisable() {
        PlayerPrefs.SetFloat("bgm", bgmSlider.value);
        PlayerPrefs.SetFloat("sfx", sfxSlider.value);
    }

    private void SaveVolume()
    {
        throw new NotImplementedException();
    }

    //    private void SaveGame() {
    //         SettingSystem.SaveGame(this);
    //     }

    //     private void LoadGame() {
    //         SettingModel saveData = SettingSystem.LoadGame();
    //         if (saveData != null)
    //         {
    //             sfx = saveData.sfx;
    //             bgm = saveData.bgm;
    //             SetBGMVolume(bgm);
    //             SetSFXVolume(sfx);
    //             sfxSlider.value = sfx;
    //             bgmSlider.value = bgm;
    //         }   
    //     }

    //     private void OnDisable() {
    //      SaveVolume();
    //     }

    //     private void SaveVolume() {
    //      PlayerPrefs.SetFloat("bgm", bgm);
    //      PlayerPrefs.SetFloat("sfx", sfx);
    //     }

    //     private void LoadVolume() {
    //      bgm = PlayerPrefs.GetFloat("bgm");
    //      sfx = PlayerPrefs.GetFloat("sfx");
    //      SetBGMVolume(bgm);
    //      SetSFXVolume(sfx);
    //      sfxSlider.value = sfx;
    //      bgmSlider.value = bgm;
    //     }
}
