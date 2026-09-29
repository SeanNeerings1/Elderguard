using UnityEngine;

using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected float _health;
    protected float _speed;
    protected float _defence;

    public void SetStats(float health, float speed, float defence)
    {
        _health = health;
        _speed = speed;
        _defence = defence;
    }
}