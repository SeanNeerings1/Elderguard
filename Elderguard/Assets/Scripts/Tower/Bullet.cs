using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private float speed = 15f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float hitDistanceThreshold = 0.2f;

    private Transform target;

    // Wordt aangeroepen door de Tower om het doelwit in te stellen
    public void Seek(Transform _target)
    {
        target = _target;
    }

    private void Update()
    {
        // Als het doelwit is vernietigd voordat de kogel er is, ruim de kogel op
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Richting en beweging bepalen
        Vector2 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        // Controleer of de kogel dit frame bij het doel aankomt
        if (dir.magnitude <= distanceThisFrame || dir.magnitude <= hitDistanceThreshold)
        {
            HitTarget();
            return;
        }

        // Beweeg de kogel richting het doel
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);

        // Optioneel: Draai de kogel in de vliegrichting
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    // Deze methode handelt de impact af en verwijdert de kogel
    private void HitTarget()
    {
        // Schade toebrengen als de vijand een EnemyHealth script heeft
        //EnemyHealth enemy = target.GetComponent<EnemyHealth>();
        //if (enemy != null)
        //{
          //  enemy.TakeDamage(damage);
        //}

        // Verwijder de kogel uit het spel
        Destroy(gameObject);
    }
}