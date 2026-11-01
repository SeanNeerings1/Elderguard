using UnityEngine;

public class TowerPlacementManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Grid grid;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private LayerMask towerLayer;

    private GameObject towerToBuildPrefab;
    private GameObject currentPreviewObject;
    private SpriteRenderer previewRenderer;
    private BaseTower towerScriptToBuild;

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

            bool isValidTile = CanPlaceTowerAt(cellPosition, snappedWorldPos);
            bool canAfford = HasEnoughGold();
            bool canPlace = isValidTile && canAfford;


            if (previewRenderer != null)
            {
                previewRenderer.color = canPlace ? new Color(0f, 1f, 0f, 0.5f) : new Color(1f, 0f, 0f, 0.5f);
            }

            if (Input.GetMouseButtonDown(0) && canPlace)
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

        // Haal het BaseTower script op van de prefab om de prijs te weten
        if (towerToBuildPrefab != null)
        {
            towerScriptToBuild = towerToBuildPrefab.GetComponent<BaseTower>();
        }

        currentPreviewObject = new GameObject("TowerPreview");
        previewRenderer = currentPreviewObject.AddComponent<SpriteRenderer>();

        SpriteRenderer prefabRenderer = towerPrefab.GetComponentInChildren<SpriteRenderer>();
        if (prefabRenderer != null)
        {
            previewRenderer.sprite = prefabRenderer.sprite;
            previewRenderer.sortingOrder = 10;
        }
    }

    private bool HasEnoughGold()
    {
        if (towerScriptToBuild == null || EconomyManager.Instance == null) return false;

        return EconomyManager.Instance.CanAfford(towerScriptToBuild.Cost);
    }

    private bool CanPlaceTowerAt(Vector3Int cellPosition, Vector3 cellWorldPos)
    {
        Collider2D obstacleCheck = Physics2D.OverlapBox(cellWorldPos, new Vector2(0.8f, 0.8f), 0f, obstacleLayer);
        if (obstacleCheck != null) return false;

        Collider2D towerCheck = Physics2D.OverlapBox(cellWorldPos, new Vector2(0.8f, 0.8f), 0f, towerLayer);
        if (towerCheck != null) return false;

        return true;
    }

    private void PlaceTower(Vector3 position)
    {
        if (towerScriptToBuild == null || EconomyManager.Instance == null) return;

        if (EconomyManager.Instance.TrySpend(towerScriptToBuild.Cost))
        {
            Instantiate(towerToBuildPrefab, position, Quaternion.identity);
            CancelPlacement();
        }
        else
        {
            Debug.Log("Niet genoeg geld om deze toren te bouwen!");
        }
    }

    public void CancelPlacement()
    {
        if (currentPreviewObject != null)
        {
            Destroy(currentPreviewObject);
        }
    }
}