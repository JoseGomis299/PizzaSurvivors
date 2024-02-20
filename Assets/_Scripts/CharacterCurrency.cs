using UnityEngine;

public class CharacterCurrency : MonoBehaviour
{
    private int _money;
    void Start()
    {
        _money = 0;
    }
    
    public void AddValue(int amount)
    {
        _money += amount;
    }
}