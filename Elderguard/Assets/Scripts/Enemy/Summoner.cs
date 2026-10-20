using UnityEngine;
using System.Collections;


public class Summoner : Enemy
{
    [SerializeField] private GameObject _enemyToSummon;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _summonDuration = 2f;
    [SerializeField] private int _enemiesToSummon = 4;
    [SerializeField] private float _summonCooldown = 10f;

    private float _originalSpeed;
    private bool _summoning;
    private bool _ableToSummon = true;

    void Start()
    {
        _originalSpeed = _speed; // remember the normal speed
    }

    public void Summon()
    {
        if (!_ableToSummon) return; // safety so it can't be triggered twice
        StartCoroutine(SummonRoutine());
    }

    private IEnumerator SummonRoutine()
    {
        _summoning = true;
        _ableToSummon = false;
        _speed = 0;

        // spread the 4 spawns evenly over the 2 seconds
        float delay = _summonDuration / _enemiesToSummon;
        for (int i = 0; i < _enemiesToSummon; i++)
        {
            Instantiate(_enemyToSummon, _spawnPoint.position, _spawnPoint.rotation);
            yield return new WaitForSeconds(delay);
        }

       
        _speed = _originalSpeed;
        _summoning = false;

        // cooldown starts after the summon finishes
        yield return new WaitForSeconds(_summonCooldown);
        _ableToSummon = true;
    }
}