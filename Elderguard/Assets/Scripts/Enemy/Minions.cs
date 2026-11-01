using UnityEngine;

public class Minion : Enemy
{
    [Header("Minion Stats")]
    [SerializeField] private float _health = 50f;
    [SerializeField] private float _defence = 0f;
    private void Start()
    {
        SetStats(_health, _defence);
    }
}