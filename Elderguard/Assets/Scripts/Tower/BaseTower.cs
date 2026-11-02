using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTower : MonoBehaviour
{
    [Header("Base Tower Settings")]
    [SerializeField] protected float range = 5f;
    [SerializeField] protected float fireRate = 1f; 
    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] protected Transform rotationPart; 
    [SerializeField] protected float rotationSpeed = 360f;
    [SerializeField] protected int cost = 100;

    public int Cost => cost;

    public static readonly List<BaseTower> All = new List<BaseTower>();

    public bool IsDisabled { get; private set; }

    public void SetDisabled(bool value) => IsDisabled = value;

    protected virtual void OnEnable() => All.Add(this);
    protected virtual void OnDisable() => All.Remove(this);

    protected Transform targetEnemy;
    private float fireCountdown = 0f;

    protected virtual void Start()
    {
        InvokeRepeating(nameof(UpdateTarget), 0f, 0.2f);
    }

    protected virtual void Update()
    {
        Debug.Log($"{name} disabled={IsDisabled} target={targetEnemy}");

        if (IsDisabled) return;

        fireCountdown -= Time.deltaTime;

        if (targetEnemy == null) return;

        if (rotationPart != null)
        {
            RotateTowardsTarget();
        }

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }
    }

    protected virtual void UpdateTarget()
    {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, range, enemyLayer);
        Debug.Log($"{name} found {enemiesInRange.Length} in range, mask={enemyLayer.value}");   // temporary
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

        targetEnemy = nearestEnemy;
    }

    protected virtual void RotateTowardsTarget()
    {
        Vector2 direction = targetEnemy.position - rotationPart.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f; 
        Quaternion targetRot = Quaternion.Euler(0f, 0f, angle);
        rotationPart.rotation = Quaternion.RotateTowards(rotationPart.rotation, targetRot, rotationSpeed * Time.deltaTime);
    }

    protected abstract void Shoot();

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}