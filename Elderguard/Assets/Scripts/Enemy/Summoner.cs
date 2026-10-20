using UnityEngine;

public class Summoner : Enemy
{
    [SerializeField] private GameObject _enemyToSummon;
    private Transform _spawnPoint;

    private void Start()
    {
        SetStats(300f, 3f, 15f);
        _spawnPoint = transform; 
    }
    public void Summon()
    {
        _speed = 0;
        Instantiate(_enemyToSummon, _spawnPoint.position, _spawnPoint.rotation);
      }
    }

