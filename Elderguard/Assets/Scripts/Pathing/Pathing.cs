using UnityEngine;
using UnityEngine.Tilemaps;

public class WaypointPath : MonoBehaviour
{
    public static WaypointPath Instance { get; private set; }

    public Tilemap pathTilemap;
    public Transform[] waypoints;

    public int Count => waypoints.Length;
    public Vector3 Get(int i) => waypoints[i].position;

    void Awake()
    {
        Instance = this;
        if (waypoints == null || waypoints.Length == 0) CollectChildren();
    }

    [ContextMenu("Collect child waypoints")]
    void CollectChildren()
    {
        waypoints = new Transform[transform.childCount];
        for (int i = 0; i < waypoints.Length; i++) waypoints[i] = transform.GetChild(i);
    }

    [ContextMenu("Snap to cell centers")]
    void Snap()
    {
        if (pathTilemap == null) return;
        foreach (var w in waypoints)
            w.position = pathTilemap.GetCellCenterWorld(pathTilemap.WorldToCell(w.position));
    }

    void OnDrawGizmos()
    {
        if (waypoints == null) return;
        Gizmos.color = Color.yellow;
        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i] == null) continue;
            Gizmos.DrawWireSphere(waypoints[i].position, 0.15f);
            if (i > 0 && waypoints[i - 1] != null)
                Gizmos.DrawLine(waypoints[i - 1].position, waypoints[i].position);
        }
    }
}