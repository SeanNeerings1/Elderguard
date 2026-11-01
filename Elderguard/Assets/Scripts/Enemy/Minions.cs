using UnityEngine;

public class Minion : Enemy
{
    [Header("Minion Stats")]
    [SerializeField] private float _health = 200f;
    [SerializeField] private float _defence = 4f;
    private void Start()
    {
        SetStats(_health, _defence);
    }
}