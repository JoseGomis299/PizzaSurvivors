using System;
using UnityEngine;

[RequireComponent(typeof(StatsManager))]
public class HealthComponent : MonoBehaviour, IDamageable
{
    private StatsManager _statsManager;
    private float _health;

    private CharacterMovement _characterMovement;
    
    public float Health => _health;
    public float MaxHealth => _statsManager.Stats.MaxHealth;

    public event Action<float> OnHealthUpdate;
    public event Action<float> OnMaxHealthUpdate;

    private void Awake()
    {
        _statsManager = GetComponent<StatsManager>();
        _characterMovement = GetComponent<CharacterMovement>();
        _health = _statsManager.Stats.MaxHealth;
    }
    
    public void Heal(float amount)
    {
        _health += amount;
        
        if(_health > MaxHealth) _health = MaxHealth;
        OnHealthUpdate?.Invoke(Health);
    }

    public void TakeDamage(Damage damage)
    {
        _health -= _statsManager.Stats.GetReceivedDamage(damage.element, damage.value);
        if (_characterMovement != null) _characterMovement.ApplyKnockBack(damage.KnockBack);
        if (_health <= 0)
        {
            gameObject.SetActive(false);
        }
//        Debug.Log(_health);
        OnHealthUpdate?.Invoke(_health);
    }

    public void SetMaxHealth(float maxHealth)
    {
        _statsManager.Stats.MaxHealth = maxHealth;
        OnMaxHealthUpdate?.Invoke(maxHealth);
    }
}