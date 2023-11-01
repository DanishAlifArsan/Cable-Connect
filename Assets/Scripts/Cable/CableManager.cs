using System;
using System.Collections;
using System.Collections.Generic;
using SVS;
using UnityEngine;

public class CableManager : MonoBehaviour
{
    public PlacementManager2 placementManager;
    public List<Vector3Int> tempPlacement = new List<Vector3Int>(); // list yang berisi jalan
    public List<Vector3Int> cablePositionToCheck = new List<Vector3Int>();   // list yang berisi jalan untuk dicek arahnya
    public List<Vector3Int> tempRemove = new List<Vector3Int>();

    private Vector3Int startPosition;   // posisi ujung awal jalan
    private bool placementMode = false; // variabel yang ngecek apakah sekarang lagi naruh jalan atau enggak
    private bool removeMode = false; 

    public CableFixer cableFixer;

    private int cableColor = 0; //warna dari kabel

    private void Start() {
        cableFixer = GetComponent<CableFixer>();
    }

    public void PlaceCable(Vector3Int position) {    //fungsi pas road dipasang
    // cek cek dulu

        if (!CheckPositionBeforePlacement(position))
        {
            return;
        }

        if (!placementMode) // kalau lagi gak naruh, mulai menaruh jalan
        {
            cableColor = GetCableColor(position);
            // reset isi dari list
            tempPlacement.Clear();
            cablePositionToCheck.Clear();

            placementMode = true; 
            startPosition = position;   // mendapatkan posisi ujung dari jalan

            tempPlacement.Add(position);    
            placementManager.PlaceTemporaryStructure(position, cableFixer.straight, CellType2.Road, cableColor);  // generate preview jalan
            AudioPlayer.instance.PlaySound(0, true); 
            
        }  
        else {  // proses ketika preview sudah fix
            AudioPlayer.instance.PlaySound(0, true); 
            placementManager.RemoveAllTempStructures(); // menghapus preview
            tempPlacement.Clear();

            foreach (var positionToFix in cablePositionToCheck)  // cek arah dari jalan buat diperbaiki
            {
                cableFixer.FixCableAtPosition(placementManager, positionToFix, placementManager.grid[positionToFix.x, positionToFix.z].Item2);
            }

            cablePositionToCheck.Clear();    // reset list jalan yang buat diperbaiki

            tempPlacement = placementManager.GetPathBetween(startPosition, position);   // mendapatkan jarak dari ujung ke ujung jarak

            foreach (var tempPos in tempPlacement)  // cek apakah di grid preview kosong
            {
                if (!placementManager.CheckIfPositionIsFree(tempPos))
                {
                    continue;
                }   
                placementManager.PlaceTemporaryStructure(tempPos, cableFixer.straight, CellType2.Road, cableColor);   // kalau kosong, generate preview
            }
        }
        
        FixCablePrefabs(false);  // apply pergantian arah jalan
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

        if (GetCableColor(position) == 0)
        {
            return false;
        }

        return true;
    }

    private int[] CheckNeighborColor(Vector3Int position) {
        return placementManager.GetNeighborColorFor(position);
    } 

    private int GetCableColor(Vector3Int position) {    // generate warna dari kabel
        var colorsToCheck = CheckNeighborColor(position);
        int color = 0;;
            
        foreach (var c in colorsToCheck)
        {
            if (c >= color)
            {
                color = c;
            }
        }

        return color;
    }  

    private void FixCablePrefabs(bool isRemove)   // fungsi buat ganti arah road
    {
        List<Vector3Int> cableToFix = new List<Vector3Int>();

        if (!isRemove)
        {
            cableToFix = tempPlacement;
        } else {
            cableToFix = tempRemove;
        }
        
        foreach (var tempPos in cableToFix)
        {
            cableFixer.FixCableAtPosition(placementManager, tempPos, cableColor);
            var neighbors = placementManager.GetNeighborTypeFor(tempPos, CellType2.Road);
            foreach (var cablePos in neighbors)
            {
                if (!cablePositionToCheck.Contains(cablePos)) 
                {
                    cablePositionToCheck.Add(cablePos);
                } 
            }
        }
        foreach (var positionToFix in cablePositionToCheck)
        {
            cableFixer.FixCableAtPosition(placementManager, positionToFix, placementManager.grid[positionToFix.x, positionToFix.z].Item2);
        }
    }

    public void FinishPlacing() {   // kalau sudah selesai menaruh jalan
        placementMode = false;  // set placement mode jadi false
        placementManager.AddTempStructureToDictionary();    // memasukkan jalan ke list
        tempPlacement.Clear();
        startPosition = Vector3Int.zero; // reset posisi ujung jalan
    }

    internal void RemoveCable(Vector3Int position)
    {
        if (!placementManager.CheckIfPositionOfType(position, CellType2.Road))
        {
            return;
        }

        if (!removeMode)
        {
            // reset isi dari list
            tempRemove.Clear();

            foreach (var positionToFix in cablePositionToCheck)  // cek arah dari jalan buat diperbaiki
            {
                cableFixer.FixCableAtPosition(placementManager, positionToFix, placementManager.grid[positionToFix.x, positionToFix.z].Item2);
            }

            cablePositionToCheck.Clear();

            removeMode = true; 
            startPosition = position;   // mendapatkan posisi ujung dari jalan

            tempRemove.Add(position);    
            placementManager.RemoveTemporaryStructure(position);  // generate preview jalan
            AudioPlayer.instance.PlaySound(2, true); 
        } else {
            AudioPlayer.instance.PlaySound(2, true); 
            tempRemove.Clear();

            cablePositionToCheck.Clear();    // reset list jalan yang buat diperbaiki

            tempRemove = placementManager.GetPathBetween(startPosition, position);   // mendapatkan jarak dari ujung ke ujung jarak

            foreach (var tempPos in tempRemove)  // cek apakah di grid preview kosong
            { 
                placementManager.RemoveTemporaryStructure(tempPos);   // kalau kosong, generate preview
            }
            
        }
        
        FixCablePrefabs(true);  // apply pergantian arah jalan
    }

    internal void FinishRemove()
    {
        removeMode = false;  // set placement mode jadi false
        placementManager.RemoveTempStructureFromDictionary();    // memasukkan jalan ke list
        tempRemove.Clear();
        startPosition = Vector3Int.zero; // reset posisi ujung jalan
    }
}
