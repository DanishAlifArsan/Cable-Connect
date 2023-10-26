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
        structure.GetComponentInChildren<MeshRenderer>().material.color = GetColor(colorCode);
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
                return new Color(0.8235294117647058f,0.1803921568627451f,0.15294117647058825f);
            case 2:
                return new Color(0.8392156862745098f,0.5450980392156862f,0.18823529411764706f);
            case 3:
                return new Color(0.9803921568627451f,0.8745098039215686f,0.5686274509803921f);
            case 4:
                return new Color(0.2235294117647059f,0.6901960784313725f,0.47058823529411764f);
            case 5:
                return new Color(0.6627450980392157f,0.8705882352941177f,0.9411764705882353f);
            case 6:
                return new Color(0.6823529411764706f,0.2784313725490196f,0.6039215686274509f);
            case 7:
                return new Color(0.15294117647058825f,0.3764705882352941f,0.8235294117647058f);
            case 8:
                return new Color(0.23529411764705882f,0.7372549019607844f,0.7294117647058823f);
            case 9:
                return new Color(0.3176470588235294f,0.34509803921568627f,0.3843137254901961f);
            default:
                return Color.white;
        }
    }
 }
