using System;
using System.Collections;
using System.Collections.Generic;
using SVS;
using UnityEngine;

public class StructureManager2 : MonoBehaviour
{
    public GameObject[] housePrefab, specialPrefab; // prefab dari bangunan
    public PlacementManager2 placementManager;

    [SerializeField] private float structureSpawnTimer;
    private int structureCount = 0;
    private float structureSpawnCooldown;

    public bool isConnect;

    private Dictionary<Vector3Int,Vector3Int> structureDictionary = new Dictionary<Vector3Int, Vector3Int>();
    private List<Vector3Int> checkedStructureToConnect = new List<Vector3Int>();

    private int connectionCount = 0;
    int randomColor = 0;

    private void Start() {
        PlaceStructure(true);
        isConnect = false;
    }

    private void Update() {
        structureSpawnCooldown += Time.deltaTime;
         if (structureSpawnCooldown >= structureSpawnTimer)
         {
            PlaceStructure();
         }

        foreach (var d in structureDictionary.Keys)
        {
            CheckConnection(d);
        }
    }

    private GameObject GetRandomPrefab(GameObject[] structurePrefab) {
        var randomPrefab = structurePrefab[UnityEngine.Random.Range(0, structurePrefab.Length - 1)];
        return randomPrefab;
    }

    private void PlaceStructure(bool initial = false) {
        Vector3Int housePos = placementManager.GetRandomGridPosition();
        Vector3Int specialPos = placementManager.GetRandomAdjectionGridPosition(housePos, 10);

        if (initial)
        {
            housePos = new Vector3Int(UnityEngine.Random.Range(5,10),0,UnityEngine.Random.Range(5,10));
            specialPos = new Vector3Int(UnityEngine.Random.Range(10,15),0,UnityEngine.Random.Range(10,15));
        }

        if (CheckPositionBeforePlacement(housePos) && CheckPositionBeforePlacement(specialPos) && housePos != specialPos)
        {
            structureCount++;
            randomColor = GetRandomColor();
            structureSpawnCooldown = 0;
            PlaceHouse(housePos);
            PlaceSpecial(specialPos);
            structureDictionary.Add(housePos,specialPos);
            AudioPlayer.instance.PlaySound(1);
        }
    }

    public int CheckConnection(Vector3Int position) {
         if (checkedStructureToConnect.Contains(position))
        {
             if (placementManager.GetPathBetween(position, structureDictionary[position], true).Count <= 0)
            {
                connectionCount--;
                checkedStructureToConnect.Remove(position);
            }
        }

        if (!checkedStructureToConnect.Contains(position))
        {
            Vector3Int specialPos  = structureDictionary[position];
            if (placementManager.GetPathBetween(position, specialPos, true).Count > 0)
            {
                AudioPlayer.instance.PlaySound(3);
                connectionCount++;
                checkedStructureToConnect.Add(position);
                isConnect = true;
            }
        }

        return connectionCount;
    }

    public void PlaceHouse(Vector3Int position) {   // buat naruh bangunan
       
            placementManager.PlaceObjectOnTheMap(position, GetRandomPrefab(housePrefab), CellType2.Structure, randomColor);
    }

    public void PlaceSpecial(Vector3Int position) { // buat naruh bangunan khusus
        
            placementManager.PlaceObjectOnTheMap(position, GetRandomPrefab(specialPrefab), CellType2.Structure, randomColor);
    }

    private int GetRandomColor() {
        return UnityEngine.Random.Range(1, 9);
    }

    private bool CheckPositionBeforePlacement(Vector3Int position)  // check grid yang akan ditaruh bangunan
    {
        if (!placementManager.CheckIfPositionInBound(position))
        {
            return false;
        }
        if (!placementManager.CheckIfPositionIsFree(position))
        {
            return false;
        }
        if (placementManager.GetNeighborTypeFor(position, CellType2.Structure).Count > 0 || placementManager.GetNeighborTypeFor(position, CellType2.Road).Count > 0 || placementManager.GetNeighborTypeFor(position, CellType2.SpecialStructure).Count > 0)
        {
            return false;
        }

        return true;
    }

     public int GetNumberOfConnections() { //buat menghitung langkah
        return connectionCount;
    }
}
