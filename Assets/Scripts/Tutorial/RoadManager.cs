using System;
using System.Collections;
using System.Collections.Generic;
using SVS;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public PlacementManager placementManager;
    public List<Vector3Int> tempPlacement = new List<Vector3Int>(); // list yang berisi jalan
    public List<Vector3Int> roadPositionToCheck = new List<Vector3Int>();   // list yang berisi jalan untuk dicek arahnya

    private Vector3Int startPosition;   // posisi ujung awal jalan
    private bool placementMode = false; // variabel yang ngecek apakah sekarang lagi naruh jalan atau enggak

    public RoadFixer roadFixer;

    private void Start() {
        roadFixer = GetComponent<RoadFixer>();
    }

    public void PlaceRoad(Vector3Int position) {    //fungsi pas road dipasang
    // cek cek dulu
        if (!placementManager.CheckIfPositionInBound(position))
        {
            return;
        }
        if (!placementManager.CheckIfPositionIsFree(position))
        {
            return;
        }
        if (!placementMode) // kalau lagi gak naruh, mulai menaruh jalan
        {
            // reset isi dari list
            tempPlacement.Clear();
            roadPositionToCheck.Clear();

            placementMode = true; 
            startPosition = position;   // mendapatkan posisi ujung dari jalan

            tempPlacement.Add(position);    
            placementManager.PlaceTemporaryStructure(position, roadFixer.roadStraight, CellType.Road);  // generate preview jalan
            
        }  
        else {  // proses ketika preview sudah fix
            placementManager.RemoveAllTempStructures(); // menghapus preview
            tempPlacement.Clear();

            foreach (var positionToFix in roadPositionToCheck)  // cek arah dari jalan buat diperbaiki
            {
                roadFixer.FixRoadAtPosition(placementManager, positionToFix);
            }

            roadPositionToCheck.Clear();    // reset list jalan yang buat diperbaiki

            tempPlacement = placementManager.GetPathBetween(startPosition, position);   // mendapatkan jarak dari ujung ke ujung jarak

            foreach (var tempPos in tempPlacement)  // cek apakah di grid preview kosong
            {
                if (!placementManager.CheckIfPositionIsFree(tempPos))
                {
                    continue;
                }   
                placementManager.PlaceTemporaryStructure(tempPos, roadFixer.roadStraight, CellType.Road);   // kalau kosong, generate preview
            }
        }

        FixRoadPrefabs();  // apply pergantian arah jalan
    }

    private void FixRoadPrefabs()   // fungsi buat ganti arah road
    {
        foreach (var tempPos in tempPlacement)
        {
            roadFixer.FixRoadAtPosition(placementManager, tempPos);
            var neighbors = placementManager.GetNeighborTypeFor(tempPos, CellType.Road);
            foreach (var roadPosition in neighbors)
            {
                if (!roadPositionToCheck.Contains(roadPosition)) 
                {
                    roadPositionToCheck.Add(roadPosition);
                }
            }
        }
        foreach (var positionToFix in roadPositionToCheck)
        {
            roadFixer.FixRoadAtPosition(placementManager, positionToFix);
        }
    }

    public void FinishPlacing() {   // kalau sudah selesai menaruh jalan
        placementMode = false;  // set placement mode jadi false
        placementManager.AddTempStructureToDictionary();    // memasukkan jalan ke list
        if (tempPlacement.Count > 0)
        {
            AudioPlayer.instance.PlaySound();   
        }
        tempPlacement.Clear();
        startPosition = Vector3Int.zero; // reset posisi ujung jalan
    }
}
