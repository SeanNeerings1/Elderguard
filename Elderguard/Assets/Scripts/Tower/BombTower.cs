using UnityEngine;

public class BombTower : BaseTower
{
    [Header("Bomb Tower Settings")]
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] public float explosionRadius = 2f;
    protected override void Shoot()
    {
        if (bombPrefab == null || firePoint == null) return;

        GameObject bomb = Instantiate(bombPrefab, firePoint.position, firePoint.rotation);
        Bomb Bomb = bomb.GetComponent<Bomb>();

        if (Bomb != null)
        {
            Bomb.SetExplosionRadius(explosionRadius);
            Bomb.Seek(targetEnemy);
        }
    }
}
