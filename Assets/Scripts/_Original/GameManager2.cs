using System;
using System.Collections;
using System.Collections.Generic;
using SVS;
using UnityEngine;

public class GameManager2 : MonoBehaviour
{
    public InputManager2 inputManager;
    public CableManager cableManager;
    // public UIController uIController;
    public StructureManager2 structureManager;

    private void Start() {
        // uIController.OnRoadPlacement += RoadPlacementHandler;
        // uIController.onHousePlacement += HousePlacementHandler;
        // uIController.OnSpecialPlacement += SpecialPlacementHolder;
        CablePlacementHandler();
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
}
