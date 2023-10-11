using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraMovement2 : MonoBehaviour
{
    public float panSpeed = 5f;
    public float panBorderThickness = 10f;

    void Update()
    {
        // Camera panning
        Vector3 pos = transform.position;

        if (Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            pos.z += panSpeed * Time.deltaTime;
        }
        if ( Input.mousePosition.y <= panBorderThickness)
        {
            pos.z -= panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x <= panBorderThickness)
        {
            pos.x -= panSpeed * Time.deltaTime;
        }

        transform.position = pos;
    }
}
