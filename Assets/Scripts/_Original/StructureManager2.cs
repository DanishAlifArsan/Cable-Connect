using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SVS;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public class StructureManager2 : MonoBehaviour
{
    public GameObject[] housePrefab, specialPrefab; // prefab dari bangunan
    public PlacementManager2 placementManager;
    private float[] houseWeights, specialWeights; //weight dari bangunan

    [SerializeField] private float structureSpawnTimer;
    // [SerializeField] private int maxStructureCount;
    private int structureCount = 0;
    private float structureSpawnCooldown;
    private bool isRemove = false;

    public bool isConnect;

    private Dictionary<Vector3Int,Vector3Int> structureDictionary = new Dictionary<Vector3Int, Vector3Int>();
    private List<Vector3Int> checkedStructureToConnect = new List<Vector3Int>();

    private int connectionCount = 0;
    int randomColor = 0;

    private void Start() {
        PlaceStructure();
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
            CheckConnection(d, isRemove);
        }
    }

    private GameObject GetRandomPrefab(GameObject[] structurePrefab) {
        var randomPrefab = structurePrefab[UnityEngine.Random.Range(0, structurePrefab.Length - 1)];
        return randomPrefab;
    }

    private void PlaceStructure() {
        Vector3Int housePos = placementManager.GetRandomGridPosition();
        Vector3Int specialPos = placementManager.GetRandomAdjectionGridPosition(housePos, 10);
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

    public int CheckConnection(Vector3Int position, bool isRemove) {
         if (isRemove && checkedStructureToConnect.Contains(position))
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
            // housePrefab.GetComponent<MeshRenderer>().material.SetColor("Color",ChangeColor(randomColor));
    }

    public void PlaceSpecial(Vector3Int position) { // buat naruh bangunan khusus
        
            placementManager.PlaceObjectOnTheMap(position, GetRandomPrefab(specialPrefab), CellType2.Structure, randomColor);
            //  specialPrefab.GetComponent<MeshRenderer>().material.SetColor("Color",ChangeColor(randomColor));
    }

    private int GetRandomWeight(float[] weights)    // generate weight random (gak paham juga maksudnya apa)
    {
        float sum = 0f;
        for (int i = 0; i < weights.Length; i++)
        {
            sum += weights[i];
        }
        float random = UnityEngine.Random.Range(0, sum);
        float tempSum = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            if (random >= tempSum && random < tempSum + weights[i])
            {
                return i;
            }
            tempSum += weights[i];
        }
        return 0;
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

    public void SetIsRemove(bool isRemove) {
        this.isRemove = isRemove;
    }

     public int GetNumberOfConnections() { //buat menghitung langkah
        return connectionCount;
    }
}

// [Serializable]
// public struct StructurePrefabWeight // stats dari prefab. ada prefab dan weight
// {
//     public GameObject prefab;
//     [Range(0,1)] public float weight;
// }
