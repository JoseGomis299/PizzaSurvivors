using UnityEngine;

public class Coin : MonoBehaviour, ICollectable
{
    public void Collect(GameObject collector)
    {
        collector.GetComponent<CharacterCurrency>().AddValue(Random.Range(1,10));
    }
}