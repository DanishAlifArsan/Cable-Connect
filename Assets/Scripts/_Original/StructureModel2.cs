using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureModel2 : MonoBehaviour
{
    float yHeight = 0;

    public void CreateModel(GameObject model) {     // membuat bangunan baru
        var structure = Instantiate(model, transform);
        yHeight = structure.transform.position.y;
    } 

    public void SwapModel(GameObject model, Quaternion rotation, int colorCode) {      // mengganti gambar bangunan
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        var structure = Instantiate(model, transform);
        structure.transform.localPosition = new Vector3(0,yHeight, 0);
        structure.transform.localRotation = rotation; 
        structure.GetComponent<MeshRenderer>().material.color = GetColor(colorCode);
    }

    public void RemoveModel() {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    private Color GetColor(int colorCode) {
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
 }
