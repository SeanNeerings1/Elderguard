using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerPlacementManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Grid grid;
    [SerializeField] private LayerMask obstacleLayer;  
    [SerializeField] private LayerMask towerLayer;     

    private GameObject towerToBuildPrefab;
    private GameObject currentPreviewObject;
    private SpriteRenderer previewRenderer;

    private void Update()
    {
        if (towerToBuildPrefab == null) return;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector3Int cellPosition = grid.WorldToCell(mouseWorldPos);
        Vector3 snappedWorldPos = grid.GetCellCenterWorld(cellPosition);

        if (currentPreviewObject != null)
        {
            currentPreviewObject.transform.position = snappedWorldPos;

            bool isValid = CanPlaceTowerAt(cellPosition, snappedWorldPos);
            if (previewRenderer != null)
            {
                previewRenderer.color = isValid ? new Color(0f, 1f, 0f, 0.5f) : new Color(1f, 0f, 0f, 0.5f);
            }

            if (Input.GetMouseButtonDown(0) && isValid)
            {
                PlaceTower(snappedWorldPos);
            }

            if (Input.GetMouseButtonDown(1))
            {
                CancelPlacement();
            }
        }
    }
    public void SelectTowerToPlace(GameObject towerPrefab)
    {
        if (currentPreviewObject != null)
        {
            Destroy(currentPreviewObject);
        }

        towerToBuildPrefab = towerPrefab;

        currentPreviewObject = new GameObject("TowerPreview");
        previewRenderer = currentPreviewObject.AddComponent<SpriteRenderer>();

        SpriteRenderer prefabRenderer = towerPrefab.GetComponentInChildren<SpriteRenderer>();
        if (prefabRenderer != null)
        {
            previewRenderer.sprite = prefabRenderer.sprite;
            previewRenderer.sortingOrder = 10; 
        }
    }

    private bool CanPlaceTowerAt(Vector3Int cellPosition, Vector3 cellWorldPos)
    {
        Vector2 boxSize = (Vector2)grid.cellSize * 0.8f;

        if (obstacleLayer.value != 0 &&
            Physics2D.OverlapBox(cellWorldPos, boxSize, 0f, obstacleLayer) != null)
        {
            return false;
        }

        if (Physics2D.OverlapBox(cellWorldPos, boxSize, 0f, towerLayer) != null)
        {
            return false;
        }

        return true;
    }

    private void PlaceTower(Vector3 position)
    {
        Instantiate(towerToBuildPrefab, position, Quaternion.identity);

        CancelPlacement();
    }

    public void CancelPlacement()
    {
        towerToBuildPrefab = null;
        if (currentPreviewObject != null)
        {
            Destroy(currentPreviewObject);
        }
    }
}