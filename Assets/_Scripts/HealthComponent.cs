using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour, IDamageable
{
    private StatsManager statsManager;
    public float Health => statsManager.Stats.BaseHealth;

    public event Action<float> onHealthUpdate;

    private void Start()
    {
        statsManager = GetComponent<StatsManager>();
    }

    public void TakeDamage(float damage)
    {
        statsManager.Stats.BaseHealth -= statsManager.Stats.GetReceivedDamage(Element.None, damage);
        if (Health <= 0)
        {
            gameObject.SetActive(false);
        }
        //Debug.Log(statsManager.Stats.BaseHealth);
        onHealthUpdate?.Invoke(Health);
    }

}