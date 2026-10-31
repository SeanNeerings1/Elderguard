using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("Bomb Settings")]
    [SerializeField] private float speed = 15f;
    [SerializeField] private int damage = 15;
    [SerializeField] private float hitDistanceThreshold = 0.2f;
    private float explosionRadius = 2.5f;
    [SerializeField] private LayerMask enemyLayer;

    private Transform target;

    public void Seek(Transform _target)
    {
        target = _target;
    }

    public void SetExplosionRadius(float radius)
    {
        explosionRadius = radius;
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame || dir.magnitude <= hitDistanceThreshold)
        {
            Explode();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void Explode()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayer);

        foreach (Collider2D enemyCollider in hitEnemies)
        {
            Enemy enemyHealth = enemyCollider.GetComponent<Enemy>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }
}