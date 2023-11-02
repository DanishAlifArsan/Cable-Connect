using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeSetting : MonoBehaviour
{
    public SettingMenu settingMenu;

    public float sfx, bgm;

    private void Start() {
        LoadVolume();
    }

    // private void SaveVolume() {
    //     bgm = settingMenu.bgmSlider.value;
    //     sfx = settingMenu.sfxSlider.value;
    //     PlayerPrefs.SetFloat("bgm", bgm);
    //     PlayerPrefs.SetFloat("sfx", sfx);
    // }

    private void LoadVolume() {
        bgm = PlayerPrefs.GetFloat("bgm", 0.75f);
        sfx = PlayerPrefs.GetFloat("sfx", 0.75f);
        // settingMenu.SetBGMVolume(bgm);
        // settingMenu.SetSFXVolume(sfx);
        settingMenu.sfxSlider.value = sfx;
        settingMenu.bgmSlider.value = bgm;
    }

    // private void SaveGame() {
    //     SettingSystem.SaveGame(this);
    // }

    // private void LoadGame() {
    //     SettingModel saveData = SettingSystem.LoadGame();
    //     if (saveData != null)
    //     {
    //         sfx = saveData.sfx;
    //         bgm = saveData.bgm;
    //         settingMenu.SetBGMVolume(bgm);
    //         settingMenu.SetSFXVolume(sfx);
    //         settingMenu.sfxSlider.value = sfx;
    //         settingMenu.bgmSlider.value = bgm;
    //     }   
    // }

    // private void OnDisable() {
    //     SaveGame();
    // }
}
