using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraMovement2 : MonoBehaviour
{
    
    private float panBorderThickness = 10f;
    [SerializeField] private float cameraSpeed = 5f;
    [SerializeField] private float minY, maxY;
    [SerializeField] Vector3 minValue, maxValue;

    private Camera cam;

    private void Start() {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        // Camera panning
        Vector3 pos = transform.position;

        if (Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            pos.z += cameraSpeed * Time.deltaTime;
        }
        if ( Input.mousePosition.y <= panBorderThickness)
        {
            pos.z -= cameraSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos.x += cameraSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x <= panBorderThickness)
        {
            pos.x -= cameraSpeed * Time.deltaTime;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        cam.orthographicSize -= scroll * 100f * cameraSpeed * Time.deltaTime;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minY, maxY);

        pos.x = Mathf.Clamp(pos.x, minValue.x, maxValue.x);
        pos.z = Mathf.Clamp(pos.z, minValue.z, maxValue.z);

        transform.position = pos;
    }
}
