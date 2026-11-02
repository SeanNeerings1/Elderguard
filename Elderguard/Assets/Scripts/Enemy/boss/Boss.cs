using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Boss enemy that periodically disables some of the player's towers.
// Needs the small additions to BaseTower: All, IsDisabled, SetDisabled().
public class Boss : Enemy
{
    [Header("Boss Stats")]
    [SerializeField] private float _health = 500f;
    [SerializeField] private float _defence = 6f;

    [Header("Tower Disabling")]
    [SerializeField] private float _timeBetweenDisables = 6f;  // pause between each disable wave
    [SerializeField] private int _towersToDisable = 2;         // how many towers per wave
    [SerializeField] private float _disableDuration = 4f;      // seconds each tower stays off
    [SerializeField] private float _range = 0f;                // 0 = any tower on the map

    private readonly List<BaseTower> _disabled = new List<BaseTower>();

    protected override void Awake()
    {
        base.Awake();
        isBoss = true;
    }

    void Start()
    {
        SetStats(_health, _defence);
        StartCoroutine(DisableLoop());
    }

    private IEnumerator DisableLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(_timeBetweenDisables);
            DisableTowers();
        }
    }

    private void DisableTowers()
    {
        var candidates = new List<BaseTower>();
        foreach (var t in BaseTower.All)
        {
            if (t == null || t.IsDisabled) continue;
            if (_range > 0f && Vector2.Distance(transform.position, t.transform.position) > _range) continue;
            candidates.Add(t);
        }

        for (int i = candidates.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (candidates[i], candidates[j]) = (candidates[j], candidates[i]);
        }

        int count = Mathf.Min(_towersToDisable, candidates.Count);
        for (int i = 0; i < count; i++)
        {
            BaseTower t = candidates[i];
            t.SetDisabled(true);
            _disabled.Add(t);
            StartCoroutine(ReenableAfterDelay(t));
        }
    }

    private IEnumerator ReenableAfterDelay(BaseTower t)
    {
        yield return new WaitForSeconds(_disableDuration);
        if (t != null) t.SetDisabled(false);
        _disabled.Remove(t);
    }

    void OnDestroy()
    {
        foreach (var t in _disabled)
            if (t != null) t.SetDisabled(false);
        _disabled.Clear();
    }
}