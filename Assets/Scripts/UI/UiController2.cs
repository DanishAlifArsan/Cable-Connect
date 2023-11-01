using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiController2 : MonoBehaviour
{
    public Action OnCablePlacement, onCableRemove;
    public Button placeCableButton, RemoveCableButton;

    public Color SelectedColor;
    List<Button> buttonList; 
    [SerializeField] GameObject[] menus;

    private void Start() {
        Time.timeScale = 1;
        if (placeCableButton == null)
        {
            return;
        }
        buttonList = new List<Button>{placeCableButton, RemoveCableButton};

        placeCableButton.onClick.AddListener(()=> {
            ResetButtonColor();
            ModifyOutline(placeCableButton);
            OnCablePlacement?.Invoke();
        });

        RemoveCableButton.onClick.AddListener(()=> {
            ResetButtonColor();
            ModifyOutline(RemoveCableButton);
            onCableRemove?.Invoke();
        });

        placeCableButton.onClick.Invoke(); 
    }

    private void ModifyOutline(Button button)
    {
        var image = button.GetComponent<Image>();
        image.color = SelectedColor;
    }

    private void ResetButtonColor()
    {
        foreach (Button b in buttonList)
        {
            b.GetComponent<Image>().color = Color.white;
        }
    }

    public void ShowMenu(int i) {
        menus[i].SetActive(true);
    }
    public void HideMenu(int i) {
        menus[i].SetActive(false);
    }

    public void PauseGame() {
        if (!menus[0].activeInHierarchy)
        {
            Time.timeScale = 0;
            menus[0].SetActive(true);
        } else {
            Time.timeScale = 1;
            menus[0].SetActive(false);
        }   
    }

    public void ChangeScene(int scene) {
        SceneManager.LoadScene(scene);
    }

    public void ExitGame() {
        Debug.Log("Exit Game");
        Application.Quit();
    }

}
