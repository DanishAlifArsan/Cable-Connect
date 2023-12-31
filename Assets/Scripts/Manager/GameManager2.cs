using System;
using System.Collections;
using System.Collections.Generic;
using SVS;
using TMPro;
using UnityEngine;

public class GameManager2 : MonoBehaviour
{
    public InputManager2 inputManager;
    public CableManager cableManager;
    public UiController2 uIController;
    public StructureManager2 structureManager;
    
    [SerializeField] private TextMeshProUGUI teksLangkah, teksKoneksi;
    [SerializeField] private UnityEngine.UI.Image LifeBar;
    // [SerializeField] private int maxConnection;
    [SerializeField] private float maxLife;
    [SerializeField] private float lifeDecreaseRate;
    private float currentLife;
    private float timer = 0;
    public int totalScore {get; private set;}
    public int timeScore {get; private set;}
    public int connectedScore {get; private set;}
    private bool isGameOver = false;

    private void Start() {
        uIController.OnCablePlacement += CablePlacementHandler;
        uIController.onCableRemove += cableRemoveHandler;
        CablePlacementHandler();
        currentLife = maxLife;
    }

    private void Update() {
        if (isGameOver)
        {
            return;
        }
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
    }

    private void CablePlacementHandler() //hanlder untuk menaruh jalan
    {
        ClearInputAction();
        inputManager.OnMouseClick += cableManager.PlaceCable; //kalau mouse diklik, jalannya dipasang
        inputManager.OnMouseHold += cableManager.PlaceCable;  //kalau mouse dihold, munculin preview jalan
        inputManager.OnMouseUp += cableManager.FinishPlacing;    //kalau mouse dilepas, preview jalan hilang
    }

    private void ClearInputAction() // sebelum menaruh bangunan, input mouse direset
    {
        inputManager.OnMouseClick = null;
        inputManager.OnMouseHold = null;
        inputManager.OnMouseUp = null;
    }

    private void GameOver() {
        isGameOver = true;
        Time.timeScale = 0;
        timeScore = (int) timer * 9;
        connectedScore = structureManager.GetNumberOfConnections() * 500;
        //(waktu(detik) x 9)+(connected x 500)
        totalScore = timeScore + connectedScore;
        uIController.ShowMenu(1);
    }
}
