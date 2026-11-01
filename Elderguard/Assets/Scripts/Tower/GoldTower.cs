using UnityEngine;

public class GoldTower : BaseTower
{
    [Header("Gold Generation Settings")]
    [SerializeField] private int goldAmount = 10;


    protected override void Shoot()
    {
        GenerateGold();
    }

    private void GenerateGold()
    {
        if (EconomyManager.Instance != null)
        {
            EconomyManager.Instance.AddGold(goldAmount);
        }
        else
        {
            return;
        }
    }
}