using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private int currentScene;
    [SerializeField] private TextMeshProUGUI sceneCounterText;
    [SerializeField] private GameObject[] scenes;
    
    private void Awake()
    {
        SwitchScene(0);
    }

    private void SelectScene(int index) {
        if(index < 0) {
            index = scenes.Length - 1;
            currentScene = scenes.Length - 1;
        }
        if (index > scenes.Length  -1)
        {
            index = 0;
            currentScene = 0;
        }
        for (int i = 0; i < scenes.Length; i++)
        {
            scenes[i].SetActive(i==index);
        } 
    }

    public void SwitchScene(int change) {
        currentScene += change;
        SelectScene(currentScene);
        sceneCounterText.text = (currentScene + 1).ToString() + "/" + scenes.Length.ToString();
    }
}
