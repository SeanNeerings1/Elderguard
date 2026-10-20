using UnityEngine;

public class TowerPlacementManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Grid grid;
    [SerializeField] private LayerMask obstacleLayer; // Selecteer hier de 'Road' of 'Unwalkable' layer
    [SerializeField] private LayerMask towerLayer;    // Selecteer hier de layer waar reeds geplaatste towers op staan

    private GameObject towerToBuildPrefab;
    private GameObject currentPreviewObject;
    private SpriteRenderer previewRenderer;

    private void Update()
    {
        if (towerToBuildPrefab == null) return;

        // 1. Muispositie omzetten naar Wereld- en Grid-coördinaten
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector3Int cellPosition = grid.WorldToCell(mouseWorldPos);
        Vector3 snappedWorldPos = grid.GetCellCenterWorld(cellPosition);

        // 2. Toon en verplaats de preview
        if (currentPreviewObject != null)
        {
            currentPreviewObject.transform.position = snappedWorldPos;

            // Controleer of de tile geldig is en pas de kleur aan (Groen = ok, Rood = bezet)
            bool isValid = CanPlaceTowerAt(cellPosition, snappedWorldPos);
            if (previewRenderer != null)
            {
                previewRenderer.color = isValid ? new Color(0f, 1f, 0f, 0.5f) : new Color(1f, 0f, 0f, 0.5f);
            }

            // 3. Klikken om te plaatsen
            if (Input.GetMouseButtonDown(0) && isValid)
            {
                PlaceTower(snappedWorldPos);
            }

            // Annuleren met rechtermuisknop
            if (Input.GetMouseButtonDown(1))
            {
                CancelPlacement();
            }
        }
    }

    // Wordt aangeroepen vanuit de UI knoppen
    public void SelectTowerToPlace(GameObject towerPrefab)
    {
        // Ruim eventuele oude preview op
        if (currentPreviewObject != null)
        {
            Destroy(currentPreviewObject);
        }

        towerToBuildPrefab = towerPrefab;

        // Maak een tijdelijke visuele preview
        currentPreviewObject = new GameObject("TowerPreview");
        previewRenderer = currentPreviewObject.AddComponent<SpriteRenderer>();

        // Haal de sprite van de te bouwen tower op
        SpriteRenderer prefabRenderer = towerPrefab.GetComponentInChildren<SpriteRenderer>();
        if (prefabRenderer != null)
        {
            previewRenderer.sprite = prefabRenderer.sprite;
            previewRenderer.sortingOrder = 10; // Zorg dat de preview boven op de map ligt
        }
    }

    private bool CanPlaceTowerAt(Vector3Int cellPosition, Vector3 cellWorldPos)
    {
        // A. Controleer of er een weg/obstakel op de tile staat met Physics2D
        Collider2D obstacleCheck = Physics2D.OverlapBox(cellWorldPos, new Vector2(0.8f, 0.8f), 0f, obstacleLayer);
        if (obstacleCheck != null)
        {
            return false; // Er ligt een weg of obstakel!
        }

        // B. Controleer of er al een andere tower staat
        Collider2D towerCheck = Physics2D.OverlapBox(cellWorldPos, new Vector2(0.8f, 0.8f), 0f, towerLayer);
        if (towerCheck != null)
        {
            return false; // Er staat al een tower!
        }

        return true;
    }

    private void PlaceTower(Vector3 position)
    {
        Instantiate(towerToBuildPrefab, position, Quaternion.identity);

        // Optioneel: Stoppen met plaatsen na 1 tower, of ingedrukt houden om meerdere te plaatsen
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