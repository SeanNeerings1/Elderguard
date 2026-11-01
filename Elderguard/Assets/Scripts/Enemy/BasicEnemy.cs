using UnityEngine;

public class BasicEnemy : Enemy
{

    [Header("Stats")]
    [SerializeField] private float _health = 600f;
    [SerializeField] private float _defence = 1f;
    private void Start()
    {
        SetStats(_health, _defence);

    }
}