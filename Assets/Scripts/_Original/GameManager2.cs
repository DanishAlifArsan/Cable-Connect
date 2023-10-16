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
    [SerializeField] private int maxConnection;

    private void Start() {
        uIController.OnCablePlacement += CablePlacementHandler;
        uIController.onCableRemove += cableRemoveHandler;
        CablePlacementHandler();
    }

    private void Update() {
        teksLangkah.text = "Moves : " + inputManager.GetNumberOfMoves(); // menampilkan jumlah langkah
        teksKoneksi.text = structureManager.GetNumberOfConnections() + " / " + maxConnection; // menampilkan jumlah koneksi
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
}
