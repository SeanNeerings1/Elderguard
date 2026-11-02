using UnityEngine;
using System.Collections;

public class Summoner : Enemy
{

    [Header("Summoner Stats")]
    [SerializeField] private float _health = 150f;
    [SerializeField] private float _defence = 4f;

    [Header("Summoning")]
    [SerializeField] private GameObject _enemyToSummon;
    [SerializeField] private Transform _minionSpawnPos;
    [SerializeField] private float _summonDuration = 2f;
    [SerializeField] private float _timeBetweenSummons = 3f;
    [SerializeField] private int _enemiesToSummon = 4;

    private bool _summoning;
    private PathFollowing _path;

    public bool IsSummoning => _summoning;

    protected override void Awake()
    {
        base.Awake();
        _path = GetComponent<PathFollowing>();
    }

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
            GameObject go = Instantiate(_enemyToSummon, _minionSpawnPos.position, Quaternion.identity);
            go.GetComponent<Enemy>().Init(wave);                                   
            go.GetComponent<PathFollowing>().StartAt(_path.CurrentIndex, _minionSpawnPos.position);

            yield return new WaitForSeconds(delay);
        }
        _summoning = false;
    }
}