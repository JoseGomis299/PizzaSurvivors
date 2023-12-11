using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public float Health;

    private void Start()
    {
        Health = GetComponent<StatsManager>().Stats.BaseHealth;
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
    }

}
