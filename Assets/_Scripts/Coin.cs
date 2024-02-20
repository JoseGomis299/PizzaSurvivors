using UnityEngine;

public class Coin : MonoBehaviour, ICollectable
{
    public void Collect(GameObject collector)
    {
        CurrencyManager.Instance.AddValue(Random.Range(1,10));
    }
}