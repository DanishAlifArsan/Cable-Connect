using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public class CableFixer : MonoBehaviour
{
    public GameObject straight, corner, threeWay, fourWay;

    public void FixCableAtPosition(PlacementManager2 placementManager, Vector3Int tempPos, int color) { 
        // var result = placementManager.GetNeighborTypeFor(tempPos); 
        var result = placementManager.GetNeighborColorFor(tempPos); 
        int cableCount = 0;
        cableCount = result.Where(x => x == color).Count(); 
        // int cableCount = 0;
        // int structureCount = 0;
        // cableCount = result.Where(x => x == CellType2.Road).Count(); 
        // structureCount = result.Where(x => x == CellType2.Structure).Count();

        //ditambah fungsi buat cek kalau diagonal

        // if (cableCount == 0 || cableCount == 1)
        // {
        //     CreateDeadEnd(placementManager, result, tempPos);
        // } else if (cableCount == 2) 
        // {
        //     if (CreateStraightCable(placementManager, result, tempPos))  
        //     {
        //       return;   
        //     }
        //     CreateCorner(placementManager, result, tempPos);
        // } else if (cableCount == 3) {
        //     Create3Way(placementManager, result, tempPos);
        // } else {
        //     Create4Way(placementManager, result, tempPos); 
        // }

        if (cableCount == 0 || cableCount == 1)
        {
            CreateDeadEnd(placementManager, result, tempPos, color);
        } else if (cableCount == 2) 
        {
            if (CreateStraightCable(placementManager, result, tempPos, color))  
            {
              return;   
            }
            CreateCorner(placementManager, result, tempPos, color);
        }
    }

    private void CreateDeadEnd(PlacementManager2 placementManager, int[] result, Vector3Int tempPos, int color) //untuk membuat jalan buntu
    {
        if (result[0] == color)
        {
            placementManager.ModifyStructureModel(tempPos, straight, Quaternion.identity, color);           

        } else if (result[1] == color)
        {
            placementManager.ModifyStructureModel(tempPos, straight, Quaternion.Euler(0,90,0), color);           

        } else if (result[2] == color)
        {
            placementManager.ModifyStructureModel(tempPos, straight, Quaternion.identity, color);           

        } else if (result[3] == color)
        {
            placementManager.ModifyStructureModel(tempPos, straight, Quaternion.Euler(0,90,0), color);           

        }  
    }

    // private void Create4Way(PlacementManager placementManager, CellType[] result, Vector3Int tempPos)   //untuk membuat perempatan
    // {
    //     placementManager.ModifyStructureModel(tempPos, fourWay, Quaternion.identity);
    // }

    // private void Create3Way(PlacementManager placementManager, CellType[] result, Vector3Int tempPos)   //untuk membuat pertigaan
    // {
    //     if (result[1] == CellType.Road && result[2] == CellType.Road && result[3] == CellType.Road)
    //     {
    //         placementManager.ModifyStructureModel(tempPos, threeWay, Quaternion.identity);            
    //     } else if (result[2] == CellType.Road && result[3] == CellType.Road && result[0] == CellType.Road)
    //     {
    //         placementManager.ModifyStructureModel(tempPos, threeWay, Quaternion.Euler(0,90,0));            
    //     } else if (result[3] == CellType.Road && result[0] == CellType.Road && result[1] == CellType.Road)
    //     {
    //         placementManager.ModifyStructureModel(tempPos, threeWay, Quaternion.Euler(0,180,0));            
    //     } else if (result[0] == CellType.Road && result[1] == CellType.Road && result[2] == CellType.Road)
    //     {
    //         placementManager.ModifyStructureModel(tempPos, threeWay, Quaternion.Euler(0,270,0));            
    //     }
    // }

    private void CreateCorner(PlacementManager2 placementManager, int[] result, Vector3Int tempPos, int color) //untuk membuat belokan
    {
        if (result[1] == color && result[2] == color)
        {
            placementManager.ModifyStructureModel(tempPos, corner, Quaternion.Euler(0,90,0), color);           
        } else if (result[2] == color && result[3] == color)
        {
            placementManager.ModifyStructureModel(tempPos, corner, Quaternion.Euler(0,180,0), color);            
        } else if (result[3] == color && result[0] == color)
        {
            placementManager.ModifyStructureModel(tempPos, corner, Quaternion.Euler(0,270,0), color);            
        } else if (result[0] == color && result[1] == color)
        {
            placementManager.ModifyStructureModel(tempPos, corner, Quaternion.Euler(0,0,0), color);            
        }
    }

     private bool CreateStraightCable(PlacementManager2 placementManager, int[] result, Vector3Int tempPos, int color)   // untuk cek apakah jalannya lurus
    {
        if (result[0] == color && result[2] == color)
        {
            placementManager.ModifyStructureModel(tempPos, straight, Quaternion.identity, color);           
            return true;
        } else if (result[1] == color && result[3] == color)
        {
            placementManager.ModifyStructureModel(tempPos, straight, Quaternion.Euler(0,90,0), color);           
            return true;
        }
        return false;
    }

     private bool CreateDiagonalCable(PlacementManager2 placementManager, CellType2[] result, Vector3Int tempPos)   // untuk cek apakah jalannya diagonal
    {
        throw new NotImplementedException();
    }
}
