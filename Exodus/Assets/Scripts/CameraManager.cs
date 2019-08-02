using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraManager : MonoBehaviour
{
    public Vector3 outZoomPosition;
    public Vector3 inZoomPosition;
    public float outZoomSize;
    public float inZoomSize;

    Camera cam;
    //0 = zoomed in, 1 = zoomed out
    float zoomPos = 0;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        SetZoom();
    }

    private void Update()
    {
        float newZoom = -Input.GetAxisRaw("Vertical");

        if (newZoom != 0)
        {
            zoomPos = Mathf.Clamp01(zoomPos + newZoom * Time.deltaTime);
            SetZoom();
        }
    }

    public void SetZoom()
    {
        transform.position = Vector3.Lerp(inZoomPosition, outZoomPosition, zoomPos);
        cam.orthographicSize = Mathf.Lerp(inZoomSize, outZoomSize, zoomPos);
    }
}
