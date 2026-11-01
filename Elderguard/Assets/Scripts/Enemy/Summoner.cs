using UnityEngine;
using System.Collections;

public class Summoner : Enemy
{
    [SerializeField] private GameObject _enemyToSummon;
    [SerializeField] private Transform _minionSpawnPos;
    [SerializeField] private float _summonDuration = 2f;
    [SerializeField] private float _timeBetweenSummons = 3f;
    [SerializeField] private int _enemiesToSummon = 4;

    private float _originalSpeed;
    private bool _summoning;

    public bool IsSummoning => _summoning;

    void Start() // if Enemy has a Start(), make it protected virtual there and use "protected override void Start()" + base.Start()
    {
        _originalSpeed = _speed;
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

        _speed = _originalSpeed;
        _summoning = false;
    }
}