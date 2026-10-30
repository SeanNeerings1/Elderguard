using System.Collections.Generic;
using UnityEngine;

/// Placeholder enemy: walks the waypoints, can be damaged, pays gold when killed,
/// and hurts the village if it reaches the end. Replace or extend freely.
public class Enemy : MonoBehaviour
{
    // Every living enemy. WaveManager and towers use this.
    public static readonly List<Enemy> All = new List<Enemy>();

    [Header("Stats")]
    public float speed = 1f;            // world units (tiles) per second
    public float maxHealth = 40f;
    public float healthPerWave = 5f;    // extra health per wave number
    public int reward = 8;              // gold when killed
    public int baseDamage = 1;          // damage to the village if it reaches the end
    public bool isBoss;                 // killing a boss wins the level

    float health;
    int next = 1;                       // index of the waypoint we're walking toward
    bool dead;
    WaypointPath path;

    public int NextIndex => next;

    /// Bigger = further along the road. Towers use this to target the leader.
    public float Progress =>
        next * 1000f - Vector3.Distance(transform.position, path.Get(Mathf.Min(next, path.Count - 1)));

    void Awake()
    {
        path = WaypointPath.Instance;
        health = maxHealth;
    }

    void OnEnable() => All.Add(this);
    void OnDisable() => All.Remove(this);

    /// Called by the spawner right after creating the enemy.
    public void Init(int wave)
    {
        maxHealth += wave * healthPerWave;
        health = maxHealth;
    }

    /// For summoned minions: continue from a given waypoint instead of the start.
    public void StartAt(int nextIndex) { next = Mathf.Clamp(nextIndex, 1, path.Count - 1); }

    void Update()
    {
        Vector3 target = path.Get(next);
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if ((transform.position - target).sqrMagnitude < 0.0001f && ++next >= path.Count)
        {
            dead = true;
            GameManage.Instance.DamageBase(baseDamage);
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float amount)
    {
        if (dead) return;
        health -= amount;
        if (health > 0f) return;

        dead = true;
        EconomyManager.Instance.AddGold(reward);
        if (isBoss) GameManage.Instance.Win();
        Destroy(gameObject);
    }
}