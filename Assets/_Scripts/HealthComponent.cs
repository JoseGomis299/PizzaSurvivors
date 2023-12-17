using System;
using UnityEngine;

[RequireComponent(typeof(StatsManager))]
public class HealthComponent : MonoBehaviour, IDamageable
{
    private StatsManager _statsManager;
    private float _health;
    
    public float Health => _health;
    public float MaxHealth => _statsManager.Stats.MaxHealth;

    public event Action<float> OnHealthUpdate;
    public event Action<float> OnMaxHealthUpdate;

    private void Awake()
    {
        _statsManager = GetComponent<StatsManager>();
        _health = _statsManager.Stats.MaxHealth;
    }
    
    public void Heal(float amount)
    {
        _health += amount;
        
        if(_health > MaxHealth) _health = MaxHealth;
        OnHealthUpdate?.Invoke(Health);
    }

    public void TakeDamage(float damage, Element element)
    {
        _health -= _statsManager.Stats.GetReceivedDamage(element, damage);
        if (_health <= 0)
        {
            gameObject.SetActive(false);
        }
        //Debug.Log(statsManager.Stats.BaseHealth);
        OnHealthUpdate?.Invoke(_health);
    }

    public void SetMaxHealth(float maxHealth)
    {
        _statsManager.Stats.MaxHealth = maxHealth;
        OnMaxHealthUpdate?.Invoke(maxHealth);
    }
}