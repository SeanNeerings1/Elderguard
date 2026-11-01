using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    // Every living enemy. WaveManager uses this.
    public static readonly List<Enemy> All = new List<Enemy>();

    [Header("Stats")]
    public int reward = 8;
    public int baseDamage = 1;
    public float healthPerWave = 5f;   // extra health per wave number
    public bool isBoss;

    protected float maxHealth;
    protected float health;
    protected float defence;
    protected bool dead;
    int wave;

    protected virtual void Awake() { }

    void OnEnable() => All.Add(this);
    void OnDisable() => All.Remove(this);

    // Called by WaveManager right after spawning
    public void Init(int wave)
    {
        this.wave = wave;
    }

    // Your original method, now with wave scaling built in
    public void SetStats(float health, float defence)
    {
        maxHealth = health + wave * healthPerWave;
        this.health = maxHealth;
        this.defence = defence;
    }

    public void TakeDamage(float amount)
    {
        if (dead) return;

        health -= Mathf.Max(0f, amount - defence);
        OnDamaged();

        if (health <= 0f) Die();
    }

    void Die()
    {
        dead = true;
        EconomyManager.Instance.AddGold(reward);
        if (isBoss) GameManage.Instance.Win();
        OnDeath();
        Destroy(gameObject);
    }

    public void ReachedEnd()
    {
        if (dead) return;
        dead = true;
        GameManage.Instance.DamageBase(baseDamage);
        Destroy(gameObject);
    }

    protected virtual void OnDamaged() { }
    protected virtual void OnDeath() { }
}