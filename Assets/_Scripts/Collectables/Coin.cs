using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Coin : MonoBehaviour, ICollectable
{
    private int _value; 
    [SerializeField] private int maxCoinValue = 5;
    private void OnEnable()
    {
        _value = Random.Range(1, maxCoinValue);
        transform.localScale = new Vector3(0.3f+_value*0.03f, 0.3f+_value*0.03f, 1);
    }

    public void Collect(GameObject collector)
    {
        CurrencyManager.Instance.AddValue(_value);
    }
}