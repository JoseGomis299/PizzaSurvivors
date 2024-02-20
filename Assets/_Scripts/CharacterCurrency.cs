using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCurrency : MonoBehaviour
{
    private int money;
    void Start()
    {
        money = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Your money: " + money);
    }

    public void AddValue(int amount)
    {
        money += amount;
    }
}
