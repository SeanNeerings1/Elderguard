using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Camera))]
public class CameraFit : MonoBehaviour
{
    public Tilemap map;
    public Color backgroundColor = new Color(0.60f, 0.90f, 0.31f);

    Camera cam;
    int lastW, lastH;

    void Awake()
    {
        cam = GetComponent<Camera>();
        cam.orthographic = true;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = backgroundColor;
    }

    void Update()
    {
        if (Screen.width == lastW && Screen.height == lastH) return;
        lastW = Screen.width; lastH = Screen.height;
        Fit();
    }

    void Fit()
    {
        if (map == null) return;

        map.CompressBounds();
        Bounds b = map.localBounds;
        Vector3 center = map.transform.TransformPoint(b.center);
        float aspect = (float)Screen.width / Screen.height;

        cam.orthographicSize = Mathf.Max(b.extents.y, b.extents.x / aspect);
        cam.transform.position = new Vector3(center.x, center.y, -10f);
    }
}