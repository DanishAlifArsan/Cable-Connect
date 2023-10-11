using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoadFixer : MonoBehaviour
{
    public GameObject roadStraight, corner, threeWay, fourWay;

    public void FixRoadAtPosition(PlacementManager placementManager, Vector3Int tempPos) { 
        var result = placementManager.GetNeighborTypeFor(tempPos); 
        int roadCount = 0;
        roadCount = result.Where(x => x == CellType.Road).Count(); 

        //ditambah fungsi buat cek kalau diagonal

        if (roadCount == 0 || roadCount == 1)
        {
            CreateDeadEnd(placementManager, result, tempPos);
        } else if (roadCount == 2) 
        {
            if (CreateStraightRoad(placementManager, result, tempPos))  
            {
              return;   
            }
            CreateCorner(placementManager, result, tempPos);
        } else if (roadCount == 3) {
            Create3Way(placementManager, result, tempPos);
        } else {
            Create4Way(placementManager, result, tempPos);
        }
    }

    private void CreateDeadEnd(PlacementManager placementManager, CellType[] result, Vector3Int tempPos) //untuk membuat jalan buntu
    {
        if (result[0] == CellType.Road)
        {
            placementManager.ModifyStructureModel(tempPos, roadStraight, Quaternion.identity);           

        } else if (result[1] == CellType.Road)
        {
            placementManager.ModifyStructureModel(tempPos, roadStraight, Quaternion.Euler(0,90,0));           

        } else if (result[2] == CellType.Road)
        {
            placementManager.ModifyStructureModel(tempPos, roadStraight, Quaternion.identity);           

        } else if (result[3] == CellType.Road)
        {
            placementManager.ModifyStructureModel(tempPos, roadStraight, Quaternion.Euler(0,90,0));           

        }  
    }

    private void Create4Way(PlacementManager placementManager, CellType[] result, Vector3Int tempPos)   //untuk membuat perempatan
    {
        placementManager.ModifyStructureModel(tempPos, fourWay, Quaternion.identity);
    }

    private void Create3Way(PlacementManager placementManager, CellType[] result, Vector3Int tempPos)   //untuk membuat pertigaan
    {
        if (result[1] == CellType.Road && result[2] == CellType.Road && result[3] == CellType.Road)
        {
            placementManager.ModifyStructureModel(tempPos, threeWay, Quaternion.identity);            
        } else if (result[2] == CellType.Road && result[3] == CellType.Road && result[0] == CellType.Road)
        {
            placementManager.ModifyStructureModel(tempPos, threeWay, Quaternion.Euler(0,90,0));            
        } else if (result[3] == CellType.Road && result[0] == CellType.Road && result[1] == CellType.Road)
        {
            placementManager.ModifyStructureModel(tempPos, threeWay, Quaternion.Euler(0,180,0));            
        } else if (result[0] == CellType.Road && result[1] == CellType.Road && result[2] == CellType.Road)
        {
            placementManager.ModifyStructureModel(tempPos, threeWay, Quaternion.Euler(0,270,0));            
        }
    }

    private void CreateCorner(PlacementManager placementManager, CellType[] result, Vector3Int tempPos) //untuk membuat belokan
    {
        if (result[1] == CellType.Road && result[2] == CellType.Road)
        {
            placementManager.ModifyStructureModel(tempPos, corner, Quaternion.Euler(0,90,0));           
        } else if (result[2] == CellType.Road && result[3] == CellType.Road)
        {
            placementManager.ModifyStructureModel(tempPos, corner, Quaternion.Euler(0,180,0));            
        } else if (result[3] == CellType.Road && result[0] == CellType.Road)
        {
            placementManager.ModifyStructureModel(tempPos, corner, Quaternion.Euler(0,270,0));            
        } else if (result[0] == CellType.Road && result[1] == CellType.Road)
        {
            placementManager.ModifyStructureModel(tempPos, corner, Quaternion.Euler(0,0,0));            
        }
    }

     private bool CreateStraightRoad(PlacementManager placementManager, CellType[] result, Vector3Int tempPos)   // untuk cek apakah jalannya lurus
    {
        if (result[0] == CellType.Road && result[2] == CellType.Road)
        {
            placementManager.ModifyStructureModel(tempPos, roadStraight, Quaternion.identity);           
            return true;
        } else if (result[1] == CellType.Road && result[3] == CellType.Road)
        {
            placementManager.ModifyStructureModel(tempPos, roadStraight, Quaternion.Euler(0,90,0));           
            return true;
        }
        return false;
    }

     private bool CreateDiagonalRoad(PlacementManager placementManager, CellType[] result, Vector3Int tempPos)   // untuk cek apakah jalannya diagonal
    {
        throw new NotImplementedException();
    }
}
