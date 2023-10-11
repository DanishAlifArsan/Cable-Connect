using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PlacementManager2 : MonoBehaviour
{
    public int width, height;   //ukuran dari fieldnya
    Grid2 grid;  // struktur data graf

    private Dictionary<Vector3Int, StructureModel2> tempRoad = new Dictionary<Vector3Int, StructureModel2>(); //array yg nyimpan daftar preview bangunan
    private Dictionary<Vector3Int, StructureModel2> structureDictionary = new Dictionary<Vector3Int, StructureModel2>(); //array yg nyimpan daftar bangunan

    private void Start() {
        grid = new Grid2(width,height);  // inisialisasi graf
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
        return CheckIfPositionOfType(position, CellType2.Empty); // tipe empty adalah ketika grid kosong
    }

    private bool CheckIfPositionOfType(Vector3Int position, CellType2 type)  // check tipe dari grid 
    {
       return grid[position.x, position.z].Item1 == type;
    }

    internal void PlaceTemporaryStructure(Vector3Int position, GameObject structurePrefab, CellType2 type, int color)   // generate preview bangunan di grid
    {
        grid[position.x, position.z] = Tuple.Create(type,color);    // membuat grid yang dituju bertipe bangunan
        StructureModel2 structureModel = CreateANewStructureModel(position, structurePrefab, type);  // membuat bangunan
        tempRoad.Add(position,structureModel); // memasukkan bangunan yang dibuat ke array preview
        tempRoad[position].SwapModel(structurePrefab, Quaternion.identity, color);
    }

    private StructureModel2 CreateANewStructureModel(Vector3Int position, GameObject structurePrefab, CellType2 type) // membuat bangunan yg akan ditaruh 
    {
        GameObject structure = new GameObject(type.ToString());
        structure.transform.SetParent(transform);
        structure.transform.localPosition = position;
        var structureModel = structure.AddComponent<StructureModel2>();
        structureModel.CreateModel(structurePrefab);
        return structureModel;
    }

    public void ModifyStructureModel(Vector3Int position, GameObject newModel, Quaternion rotation, int randomColor) {   // mengganti arah jalan menghadap
        if (tempRoad.ContainsKey(position))
        {
            tempRoad[position].SwapModel(newModel,rotation,randomColor); // mengganti arah preview jalan
        } else if (structureDictionary.ContainsKey(position))
        {
            structureDictionary[position].SwapModel(newModel,rotation,randomColor); // mengganti arah jalan yang sebenarnya
        }
    }

    internal CellType2[] GetNeighborTypeFor(Vector3Int position) //mendapatkan tipe grid di sekitar
    {
        return grid.GetAllAdjacentCellTypes(position.x, position.z);
    }
    internal List<Vector3Int> GetNeighborTypeFor(Vector3Int tempPos, CellType2 type) //mendapatkan tipe grid di sekitar
    {
        var neighborVerticles = grid.GetAdjacentCellsOfType(tempPos.x , tempPos.z, type);  //mendapatkan list grid dengan tipe tertentu
        List<Vector3Int> neighbors = new List<Vector3Int>(); // membuat array
        foreach (var point in neighborVerticles)
        {
            neighbors.Add(new Vector3Int(point.X, 0, point.Y)); // memasukkan list grid tersebut ke array
        }
        return neighbors; // mengembalikan array
    }

    internal int[] GetNeighborColorFor(Vector3Int position) //mendapatkan warna grid di sekitar
    {
        return grid.GetAllAdjacentCellColor(position.x, position.z);
    }

     internal List<Vector3Int> GetNeighborColorFor(Vector3Int tempPos, int color) //mendapatkan tipe warna di sekitar
    {
        var neighborVerticles = grid.GetAdjacentCellsOfColor(tempPos.x , tempPos.z, color);  //mendapatkan list grid dengan tipe tertentu
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
            grid[position.x, position.z] = Tuple.Create(CellType2.Empty,0);  // untuk setiap preview, gridnya dikosongkan
            Destroy(structure.gameObject);
        }

        tempRoad.Clear();
    }

    internal List<Vector3Int> GetPathBetween(Vector3Int startPosition, Vector3Int endPosition)  // untuk menggambar jalan
    {
        var resultPath = GridSearch2.AStarSearch(grid,new Point(startPosition.x, startPosition.z), new Point(endPosition.x, endPosition.z)); // menggunakan astar untuk mendapatkan jarak dari ujung ke ujung jalan
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
            // DestroyNatureAt(structure.Key); // menghancurkan pohon ketika preview dibuat
        }
        tempRoad.Clear();
    }

    internal void PlaceObjectOnTheMap(Vector3Int position, GameObject structurePrefab, CellType2 type, int randomColor)   // generate bangunan yang asli
    {
        grid[position.x, position.z] = Tuple.Create(type,randomColor); // set tipe grid yang dituju
        StructureModel2 structureModel = CreateANewStructureModel(position, structurePrefab, type);  // membuat bangunan
        structureDictionary.Add(position,structureModel);  // menambahkan bangunan ke dictionary
        structureDictionary[position].SwapModel(structurePrefab, Quaternion.identity, randomColor);
        // DestroyNatureAt(position); // menghancurkan pohon ketika bangunan dibuat
    }

    // private void DestroyNatureAt(Vector3Int position)   // buat menghancurkan tanaman
    // {
    //     RaycastHit[] hits = Physics.BoxCastAll(position+new Vector3(0,0.5f,0), new Vector3(0.5f,0.5f,0.5f),transform.up,Quaternion.identity, 1f, 1 << LayerMask.NameToLayer("Nature")); // mendeteksi tanaman
    //     foreach (var item in hits)
    //     {
    //         Destroy(item.collider.gameObject); // menghancurkan tanaman
    //     }
    // }

    internal Vector3Int getRandomGridPosition() {   // buat cari grid random
    Vector3Int randomPos = new Vector3Int(UnityEngine.Random.Range(0, width - 1), 0, UnityEngine.Random.Range(0, height - 1));
     return randomPos;
    }
}
