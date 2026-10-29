using UnityEngine;

public class SingleTargetTower : BaseTower
{
    [Header("Single Target Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    protected override void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet Bullet = bullet.GetComponent<Bullet>();

        if (Bullet != null)
        {
            Bullet.Seek(targetEnemy);
        }
    }
}