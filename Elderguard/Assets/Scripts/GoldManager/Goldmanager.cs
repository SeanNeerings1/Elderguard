using System;
using TMPro;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance { get; private set; }

    [SerializeField] int startingGold = 150;
    [SerializeField] TMP_Text goldText;          

    public int Gold { get; private set; }

    public event Action<int> OnGoldChanged;

    public event Action<int> OnInsufficientFunds;

    void Awake()
    {
        Instance = this;
        Gold = startingGold;
        UpdateText();
    }

    void Start()
    {
        OnGoldChanged?.Invoke(Gold);
    }

    void OnDestroy() { if (Instance == this) Instance = null; }

    public bool CanAfford(int cost) => Gold >= cost;

    public bool TrySpend(int cost)
    {
        if (cost < 0) return false;
        if (Gold < cost)
        {
            OnInsufficientFunds?.Invoke(cost);
            return false;
        }
        Gold -= cost;
        Changed();
        return true;
    }

    public void AddGold(int amount)
    {
        if (amount <= 0) return;
        Gold += amount;
        Changed();
    }

    void Changed()
    {
        UpdateText();
        OnGoldChanged?.Invoke(Gold);
    }

    void UpdateText() { if (goldText) goldText.text = " " + Gold; }
}