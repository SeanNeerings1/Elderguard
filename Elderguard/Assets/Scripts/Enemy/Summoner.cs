using UnityEngine;

public class Summoner : Enemy
{
    [SerializeField] private GameObject _enemyToSummon;

    private void Start()
    {
        SetStats(300f, 3f, 15f);
    }
    public void Summon()
    {
      
    }

    }

