using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public int width, height;   //ukuran dari fieldnya
    Grid grid;  // struktur data graf

    private Dictionary<Vector3Int, StructureModel> tempRoad = new Dictionary<Vector3Int, StructureModel>(); //array yg nyimpan daftar preview bangunan
    private Dictionary<Vector3Int, StructureModel> structureDictionary = new Dictionary<Vector3Int, StructureModel>(); //array yg nyimpan daftar bangunan

    private void Start() {
        grid = new Grid(width,height);  // inisialisasi graf
    }

    internal bool CheckIfPositionInBound(Vector3Int position)   // cek kalau bangunan yg mau ditaruh ada di area field
    {
        if (position.x >= 0 && position.x < width && position.z >= 0 && position.z < height)
        {
            return true;
        }

        return false;
    }

    internal bool CheckIfPositionIsFree(Vector3Int position)    // check apakah grid kosong
    {
        return CheckIfPositionOfType(position, CellType.Empty); // tipe empty adalah ketika grid kosong
    }

    private bool CheckIfPositionOfType(Vector3Int position, CellType type)  // check tipe dari grid 
    {
       return grid[position.x, position.z] == type;
    }

    internal void PlaceTemporaryStructure(Vector3Int position, GameObject structurePrefab, CellType type)   // generate preview bangunan di grid
    {
        grid[position.x, position.z] = type;    // membuat grid yang dituju bertipe bangunan
        StructureModel structureModel = CreateANewStructureModel(position, structurePrefab, type);  // membuat bangunan
        tempRoad.Add(position,structureModel); // memasukkan bangunan yang dibuat ke array preview
    }

    private StructureModel CreateANewStructureModel(Vector3Int position, GameObject structurePrefab, CellType type) // membuat bangunan yg akan ditaruh 
    {
        GameObject structure = new GameObject(type.ToString());
        structure.transform.SetParent(transform);
        structure.transform.localPosition = position;
        var structureModel = structure.AddComponent<StructureModel>();
        structureModel.CreateModel(structurePrefab);
        return structureModel;
    }

    public void ModifyStructureModel(Vector3Int position, GameObject newModel, Quaternion rotation) {   // mengganti arah jalan menghadap
        if (tempRoad.ContainsKey(position))
        {
            tempRoad[position].SwapModel(newModel,rotation); // mengganti arah preview jalan
        } else if (structureDictionary.ContainsKey(position))
        {
            structureDictionary[position].SwapModel(newModel,rotation); // mengganti arah jalan yang sebenarnya
        }
    }

    internal CellType[] GetNeighborTypeFor(Vector3Int position) //mendapatkan tipe grid di sekitar
    {
        return grid.GetAllAdjacentCellTypes(position.x, position.z);
    }
    internal List<Vector3Int> GetNeighborTypeFor(Vector3Int tempPos, CellType type) //mendapatkan tipe grid di sekitar
    {
        var neighborVerticles = grid.GetAdjacentCellsOfType(tempPos.x , tempPos.z, type);  //mendapatkan list grid dengan tipe tertentu
        List<Vector3Int> neighbors = new List<Vector3Int>(); // membuat array
        foreach (var point in neighborVerticles)
        {
            neighbors.Add(new Vector3Int(point.X, 0, point.Y)); // memasukkan list grid tersebut ke array
        }
        return neighbors; // mengembalikan array
    }

    internal void RemoveAllTempStructures() // menghapus preview
    {
        foreach (var structure in tempRoad.Values) 
        {
            var position = Vector3Int.RoundToInt(structure.transform.position);
            grid[position.x, position.z] = CellType.Empty;  // untuk setiap preview, gridnya dikosongkan
            Destroy(structure.gameObject);
        }

        tempRoad.Clear();
    }

    internal List<Vector3Int> GetPathBetween(Vector3Int startPosition, Vector3Int endPosition)  // untuk menggambar jalan
    {
        var resultPath = GridSearch.AStarSearch(grid,new Point(startPosition.x, startPosition.z), new Point(endPosition.x, endPosition.z)); // menggunakan astar untuk mendapatkan jarak dari ujung ke ujung jalan
        List<Vector3Int> path = new List<Vector3Int>(); // membuat array path
        foreach (Point p in resultPath) 
        {
            path.Add(new Vector3Int(p.X, 0, p.Y)); // memasukkan path yang dihasilkan astar ke dalam array
        }
        return path;
    }

    internal void AddTempStructureToDictionary()    // generate preview bangunan
    {
        foreach (var structure in tempRoad)
        {
            structureDictionary.Add(structure.Key, structure.Value);
            DestroyNatureAt(structure.Key); // menghancurkan pohon ketika preview dibuat
        }
        tempRoad.Clear();
    }

    internal void PlaceObjectOnTheMap(Vector3Int position, GameObject structurePrefab, CellType type)   // generate bangunan yang asli
    {
        grid[position.x, position.z] = type; // set tipe grid yang dituju
        StructureModel structureModel = CreateANewStructureModel(position, structurePrefab, type);  // membuat bangunan
        structureDictionary.Add(position,structureModel);   // menambahkan bangunan ke dictionary
        DestroyNatureAt(position); // menghancurkan pohon ketika bangunan dibuat
    }

    private void DestroyNatureAt(Vector3Int position)   // buat menghancurkan tanaman
    {
        RaycastHit[] hits = Physics.BoxCastAll(position+new Vector3(0,0.5f,0), new Vector3(0.5f,0.5f,0.5f),transform.up,Quaternion.identity, 1f, 1 << LayerMask.NameToLayer("Nature")); // mendeteksi tanaman
        foreach (var item in hits)
        {
            Destroy(item.collider.gameObject); // menghancurkan tanaman
        }
    }
}
