using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using SVS;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager2 : MonoBehaviour
{
    public InputManager2 inputManager;
    public CableManager cableManager;
    public UiController2 uIController;
    public StructureManager2 structureManager;
    
    [SerializeField] private TextMeshProUGUI teksLangkah, teksKoneksi;
    [SerializeField] private UnityEngine.UI.Image LifeBar;
    [SerializeField] private GameObject leaderboardUI;
    // [SerializeField] private int maxConnection;
    [SerializeField] private float maxLife;
    [SerializeField] private float lifeDecreaseRate;
    private float currentLife;
    private float timer = 0;
    public int totalScore {get; private set;}

    private void Start() {
        uIController.OnCablePlacement += CablePlacementHandler;
        uIController.onCableRemove += cableRemoveHandler;
        CablePlacementHandler();
        currentLife = maxLife;
        totalScore = 0;
        leaderboardUI.SetActive(false);
    }

    private void Update() {
        teksLangkah.text = "Moves : " + inputManager.GetNumberOfMoves(); // menampilkan jumlah langkah
        teksKoneksi.text = structureManager.GetNumberOfConnections().ToString(); // menampilkan jumlah koneksi
        CheckLife();
        timer += Time.deltaTime;
        LifeBar.fillAmount = currentLife / maxLife;
    }

    private void CheckLife() {
        currentLife -= Time.deltaTime * lifeDecreaseRate;
        if (currentLife <= 0)
        {
            ClearInputAction();
            GameOver();
        }
        if (structureManager.isConnect)
        {
            currentLife = maxLife;
            structureManager.isConnect = false;
        }
    }
 
    private void cableRemoveHandler()   // handler untuk hapus kabel
    {
        ClearInputAction();
        inputManager.OnMouseClick += cableManager.RemoveCable; //kalau mouse diklik, jalannya dipasang
        inputManager.OnMouseHold += cableManager.RemoveCable;  //kalau mouse dihold, munculin preview jalan
        inputManager.OnMouseUp += cableManager.FinishRemove;    //kalau mouse dilepas, preview jalan hilang
        structureManager.SetIsRemove(true);
    }

    private void CablePlacementHandler() //hanlder untuk menaruh jalan
    {
        ClearInputAction();
        inputManager.OnMouseClick += cableManager.PlaceCable; //kalau mouse diklik, jalannya dipasang
        inputManager.OnMouseHold += cableManager.PlaceCable;  //kalau mouse dihold, munculin preview jalan
        inputManager.OnMouseUp += cableManager.FinishPlacing;    //kalau mouse dilepas, preview jalan hilang
        structureManager.SetIsRemove(false);
    }

    private void ClearInputAction() // sebelum menaruh bangunan, input mouse direset
    {
        inputManager.OnMouseClick = null;
        inputManager.OnMouseHold = null;
        inputManager.OnMouseUp = null;
    }

    private void GameOver() {
        Time.timeScale = 0;
        leaderboardUI.SetActive(true);
        timer = 0;
        totalScore = structureManager.GetNumberOfConnections() + (int) timer;
        Debug.Log("Game Over, score:" + totalScore);
    }
}
