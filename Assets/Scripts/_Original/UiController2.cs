using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController2 : MonoBehaviour
{
    public Action OnCablePlacement, onCableRemove;
    public Button placeCableButton, RemoveCableButton;

    public Color outlineColor;
    List<Button> buttonList; 

    private void Start() {
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
        var outline = button.GetComponent<Outline>();
        outline.effectColor = outlineColor;
        outline.enabled = true;
    }

    private void ResetButtonColor()
    {
        foreach (Button b in buttonList)
        {
            b.GetComponent<Outline>().enabled = false;
        }
    }
}
