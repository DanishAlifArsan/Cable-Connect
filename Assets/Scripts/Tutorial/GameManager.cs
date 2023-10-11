using System;
using System.Collections;
using System.Collections.Generic;
using SVS;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CameraMovement cameraMovement;
    public InputManager inputManager;
    public RoadManager roadManager;
    public UIController uIController;
    public StructureManager structureManager;

    private void Start() {
        uIController.OnRoadPlacement += RoadPlacementHandler;
        uIController.onHousePlacement += HousePlacementHandler;
        uIController.OnSpecialPlacement += SpecialPlacementHolder;
    }

    private void HousePlacementHandler()    // handler untuk menaruh bangunan
    {
        ClearInputAction();
        inputManager.OnMouseClick += structureManager.PlaceHouse;
    }

    private void SpecialPlacementHolder()   // handler untuk menaruh bangunan
    {
        ClearInputAction();
        inputManager.OnMouseClick += structureManager.PlaceSpecial;
    }

    private void RoadPlacementHandler() //hanlder untuk menaruh jalan
    {
        ClearInputAction();
        inputManager.OnMouseClick += roadManager.PlaceRoad; //kalau mouse diklik, jalannya dipasang
        inputManager.OnMouseHold += roadManager.PlaceRoad;  //kalau mouse dihold, munculin preview jalan
        inputManager.OnMouseUp += roadManager.FinishPlacing;    //kalau mouse dilepas, preview jalan hilang
    }

    private void ClearInputAction() // sebelum menaruh bangunan, input mouse direset
    {
        inputManager.OnMouseClick = null;
        inputManager.OnMouseHold = null;
        inputManager.OnMouseUp = null;
    }

    private void Update() {
        cameraMovement.MoveCamera(new Vector3(inputManager.CameraMovement.x,0,inputManager.CameraMovement.y));  //gerakin kamera pakai arah panah
    }
}
