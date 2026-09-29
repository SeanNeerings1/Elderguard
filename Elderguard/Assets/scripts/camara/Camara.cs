using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Camera))]
public class CameraFit : MonoBehaviour
{
    public Tilemap map;             
    public float padding = 0.5f;   
    public float bottomUiUnits = 0f; 

    Camera cam;
    int lastW, lastH;

    void Awake() { cam = GetComponent<Camera>(); cam.orthographic = true; }

    void Update()
    {
        if (Screen.width == lastW && Screen.height == lastH) return;
        lastW = Screen.width; lastH = Screen.height;
        Fit();
    }

    void Fit()
    {
        map.CompressBounds();
        Bounds b = map.localBounds;
        Vector3 center = map.transform.TransformPoint(b.center);
        float aspect = (float)Screen.width / Screen.height;

        float sizeH = b.extents.y + padding + bottomUiUnits * 0.5f;
        float sizeW = (b.extents.x + padding) / aspect;
        cam.orthographicSize = Mathf.Max(sizeH, sizeW);
        cam.transform.position = new Vector3(center.x, center.y - bottomUiUnits * 0.5f, -10f);
    }
}