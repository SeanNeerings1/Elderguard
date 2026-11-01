using UnityEngine;
using System.Collections;

public class Summoner : Enemy
{

    [Header("Summoner Stats")]
    [SerializeField] private float _health = 200f;
    [SerializeField] private float _defence = 4f;

    [Header("Summoning")]
    [SerializeField] private GameObject _enemyToSummon;
    [SerializeField] private Transform _minionSpawnPos;
    [SerializeField] private float _summonDuration = 2f;
    [SerializeField] private float _timeBetweenSummons = 3f;
    [SerializeField] private int _enemiesToSummon = 4;
    private bool _summoning;

    public bool IsSummoning => _summoning;

    void Start()
    {
        SetStats(_health, _defence); 
        StartCoroutine(SummonLoop());
    }
    private IEnumerator SummonLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(_timeBetweenSummons);
            yield return StartCoroutine(SummonRoutine());
        }
    }

    private IEnumerator SummonRoutine()
    {
        _summoning = true;

        float delay = _summonDuration / _enemiesToSummon;
        for (int i = 0; i < _enemiesToSummon; i++)
        {
            Instantiate(_enemyToSummon, _minionSpawnPos.position, Quaternion.identity);
            yield return new WaitForSeconds(delay);
        }
        _summoning = false;
    }
}