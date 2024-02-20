using System;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static event Action<int> OnMoneyChanged;
    
    public static CurrencyManager Instance {get; private set;}
    private int _money;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _money = 0;
    }
    
    public void AddValue(int amount)
    {
        _money += amount;
        OnMoneyChanged?.Invoke(_money);
    }
    
    public void RemoveValue(int amount)
    {
        _money -= amount;
        OnMoneyChanged?.Invoke(_money);
    }
}