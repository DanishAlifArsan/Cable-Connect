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
    private float structureSpawnCooldown;

    int randomColor = 0;

    private void Start() {
        randomColor = GetRandomColor();
        structureSpawnCooldown = 0;
        PlaceHouse(placementManager.getRandomGridPosition());
        PlaceSpecial(placementManager.getRandomGridPosition());
    }

    private void Update() {
        structureSpawnCooldown += Time.deltaTime;
         if (structureSpawnCooldown >= structureSpawnTimer)
         {
            randomColor = GetRandomColor();
            structureSpawnCooldown = 0;
            PlaceHouse(placementManager.getRandomGridPosition());
            PlaceSpecial(placementManager.getRandomGridPosition());
         }
    }

    public void PlaceHouse(Vector3Int position) {   // buat naruh bangunan
        if (CheckPositionBeforePlacement(position))
        {
            placementManager.PlaceObjectOnTheMap(position, housePrefab, CellType2.Structure, randomColor);
            // housePrefab.GetComponent<MeshRenderer>().material.SetColor("Color",ChangeColor(randomColor));
            AudioPlayer.instance.PlayPlacementSound();
        }
    }

    public void PlaceSpecial(Vector3Int position) { // buat naruh bangunan khusus
        if (CheckPositionBeforePlacement(position))
        {
            placementManager.PlaceObjectOnTheMap(position, specialPrefab, CellType2.Structure, randomColor);
            //  specialPrefab.GetComponent<MeshRenderer>().material.SetColor("Color",ChangeColor(randomColor));
            AudioPlayer.instance.PlayPlacementSound();
        }
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

    //fungsi buat ngubah warna bangunan
    private Color ChangeColor(int colorCode) {
        switch (colorCode)
        {
            case 1:
                return Color.red;
            case 2:
                return Color.blue;
            case 3:
                return Color.yellow;
            case 4:
                return Color.green;
            case 5:
                return Color.black;
            case 6:
                return Color.cyan;
            case 7:
                return Color.gray;
            case 8:
                return Color.green;
            case 9:
                return Color.magenta;
            default:
                return Color.white;
        }
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
