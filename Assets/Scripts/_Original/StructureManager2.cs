using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SVS;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public class StructureManager2 : MonoBehaviour
{
    public GameObject housePrefab, specialPrefab; // prefab dari bangunan
    public PlacementManager2 placementManager;
    private float[] houseWeights, specialWeights; //weight dari bangunan

    [SerializeField] private float structureSpawnTimer;
    [SerializeField] private int maxStructureCount;
    private int structureCount = 0;
    private float structureSpawnCooldown;

    int randomColor = 0;

    private void Start() {
        PlaceStructure();
    }

    private void Update() {
        structureSpawnCooldown += Time.deltaTime;
         if (structureSpawnCooldown >= structureSpawnTimer)
         {
            PlaceStructure();
         }
    }

    private void PlaceStructure() {
        Vector3Int housePos = placementManager.GetRandomGridPosition();
        Vector3Int specialPos = placementManager.GetRandomGridPosition();
        if (CheckPositionBeforePlacement(housePos) && CheckPositionBeforePlacement(specialPos) && structureCount < maxStructureCount )
        {
            structureCount++;
            randomColor = GetRandomColor();
            structureSpawnCooldown = 0;
            PlaceHouse(housePos);
            PlaceSpecial(specialPos);
        }
    }
    public void PlaceHouse(Vector3Int position) {   // buat naruh bangunan
       
            placementManager.PlaceObjectOnTheMap(position, housePrefab, CellType2.Structure, randomColor);
            // housePrefab.GetComponent<MeshRenderer>().material.SetColor("Color",ChangeColor(randomColor));
            AudioPlayer.instance.PlayPlacementSound();
        
    }

    public void PlaceSpecial(Vector3Int position) { // buat naruh bangunan khusus
        
            placementManager.PlaceObjectOnTheMap(position, specialPrefab, CellType2.Structure, randomColor);
            //  specialPrefab.GetComponent<MeshRenderer>().material.SetColor("Color",ChangeColor(randomColor));
            AudioPlayer.instance.PlayPlacementSound();
        
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
        if (placementManager.GetNeighborTypeFor(position, CellType2.Structure).Count > 0 || placementManager.GetNeighborTypeFor(position, CellType2.Road).Count > 0)
        {
            return false;
        }

        return true;
    }
}

// [Serializable]
// public struct StructurePrefabWeight // stats dari prefab. ada prefab dan weight
// {
//     public GameObject prefab;
//     [Range(0,1)] public float weight;
// }
