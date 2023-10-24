using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SVS;
using UnityEngine;
using UnityEngine.Rendering;

public class StructureManager : MonoBehaviour
{
    public StructurePrefabWeighted[] housePrefabs, specialPrefabs; // prefab dari bangunan
    public PlacementManager placementManager;
    private float[] houseWeights, specialWeights; //weight dari bangunan

    private void Start() {
        houseWeights = housePrefabs.Select(prefabStats => prefabStats.weight).ToArray();
        specialWeights = specialPrefabs.Select(prefabStats => prefabStats.weight).ToArray();
    }

    public void PlaceHouse(Vector3Int position) {   // buat naruh bangunan
        if (CheckPositionBeforePlacement(position))
        {
            int randomIndex = GetRandomWeight(houseWeights);
            placementManager.PlaceObjectOnTheMap(position, housePrefabs[randomIndex].prefab, CellType.Structure);
            AudioPlayer.instance.PlaySound(1);
        }
    }

    public void PlaceSpecial(Vector3Int position) { // buat naruh bangunan khusus
        if (CheckPositionBeforePlacement(position))
        {
            int randomIndex = GetRandomWeight(specialWeights);
            placementManager.PlaceObjectOnTheMap(position, specialPrefabs[randomIndex].prefab, CellType.Structure);
            AudioPlayer.instance.PlaySound(1);
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
        if (placementManager.GetNeighborTypeFor(position, CellType.Road).Count <= 0) //check apakah disekitar bangunan ada jalan
        {
            return false;
        }

        return true;
    }
}

[Serializable]
public struct StructurePrefabWeighted // stats dari prefab. ada prefab dan weight
{
    public GameObject prefab;
    [Range(0,1)] public float weight;
}
