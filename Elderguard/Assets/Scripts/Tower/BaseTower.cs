using UnityEngine;

public abstract class BaseTower : MonoBehaviour
{
    [Header("Base Tower Settings")]
    [SerializeField] protected float range = 5f;
    [SerializeField] protected float fireRate = 1f; // Schoten per seconde
    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] protected Transform rotationPart; // Het deel van de tower dat meedraait (optioneel)

    protected Transform targetEnemy;
    private float fireCountdown = 0f;

    protected virtual void Start()
    {
        // Zoek elke 0.2 seconden naar een target in plaats van elke frame (beter voor performance)
        InvokeRepeating(nameof(UpdateTarget), 0f, 0.2f);
    }

    protected virtual void Update()
    {
        if (targetEnemy == null) return;

        // Draai naar de vijand toe
        if (rotationPart != null)
        {
            RotateTowardsTarget();
        }

        // Schiet logica
        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    // Zoekt naar de dichtstbijzijnde vijand
    protected virtual void UpdateTarget()
    {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, range, enemyLayer);
        float shortestDistance = Mathf.Infinity;
        Transform nearestEnemy = null;

        foreach (Collider2D enemy in enemiesInRange)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy.transform;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            targetEnemy = nearestEnemy;
        }
        else
        {
            targetEnemy = null;
        }
    }

    // Draait de tower naar het target (2D)
    protected virtual void RotateTowardsTarget()
    {
        Vector2 direction = targetEnemy.position - rotationPart.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f; // -90 afhankelijk van sprite orientatie
        rotationPart.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    // Abstracte of virtual methode die specifieke towers moeten implementeren
    protected abstract void Shoot();

    // Visualiseer de range in de Unity Editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}

