using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeSetting : MonoBehaviour
{
    public SettingMenu settingMenu;

    // private float sfx, bgm;

    private void Awake() {
        LoadVolume();
    }

    // private void SaveVolume() {
    //     bgm = settingMenu.bgmSlider.value;
    //     sfx = settingMenu.sfxSlider.value;
    //     PlayerPrefs.SetFloat("bgm", bgm);
    //     PlayerPrefs.SetFloat("sfx", sfx);
    // }

    private void LoadVolume() {
        // bgm = PlayerPrefs.GetFloat("bgm");
        // sfx = PlayerPrefs.GetFloat("sfx");
        // settingMenu.SetBGMVolume(bgm);
        // settingMenu.SetSFXVolume(sfx);
        settingMenu.sfxSlider.value = PlayerPrefs.GetFloat("sfx");
        settingMenu.bgmSlider.value = PlayerPrefs.GetFloat("bgm");
    }
}
