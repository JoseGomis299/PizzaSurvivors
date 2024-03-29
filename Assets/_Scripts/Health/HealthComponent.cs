using System;
using System.Runtime.CompilerServices;
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
    }

    private void Start()
    {
        _statsManager.Stats.OnMaxHealthChanged += SetMaxHealth;
        SetInitialHealth();
    }

    private void OnDestroy()
    {
        _statsManager.Stats.OnMaxHealthChanged -= SetMaxHealth;
    }

    private void OnEnable()
    {
        SetInitialHealth();
    }

    private void SetInitialHealth()
    {
        if (_statsManager.Stats == null) return;

        _health = _statsManager.Stats.MaxHealth;
        OnMaxHealthUpdate?.Invoke(_health);
        OnHealthUpdate?.Invoke(_health);
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
        OnHealthUpdate?.Invoke(_health);

        if (_characterMovement != null) _characterMovement.ApplyKnockBack(damage.GetKnockBack(transform.position));
        if (_health <= 0 && TryGetComponent(out IKillable killable)) killable.OnDeath();
    }

    public void SetMaxHealth(float previousMaxHealth, float maxHealth)
    {
        _health = Mathf.Min(_health, maxHealth);
        
        if(maxHealth - previousMaxHealth > 0) Heal(maxHealth - previousMaxHealth );
        if(_health > maxHealth) _health = maxHealth;
        
        OnMaxHealthUpdate?.Invoke(maxHealth);
        OnHealthUpdate?.Invoke(_health);
    }
}