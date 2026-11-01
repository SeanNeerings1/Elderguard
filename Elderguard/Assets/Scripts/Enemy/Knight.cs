using UnityEngine;

public class Knight : Enemy
{
    [Header("Knight Stats")]
    [SerializeField] private float _health = 150f;
    [SerializeField] private float _defence = 10f;
    private void Start()
    {
        SetStats(_health, _defence);
    }
}