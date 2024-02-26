using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Character Stats", fileName = "Character Stats")]
public class Stats : ScriptableObject
{
    public event Action<float, float> OnMaxHealthChanged;
    [field: Header("Base Stats")]
    [field: SerializeField] public float MaxHealth {get; private set;} = 100f;
    [SerializeField, Range(0,100), InspectorName("Defense")] private float defense = 10f;
    public float Defense
    {
        get => defense;
        set
        {
            if(value > 100) value = 100;
            defense = value;
        }
    } 

    [SerializeField] private AnimationCurve defenseCurve;
    
    public float AttackSpeed { 
        get => _attackSpeed;
        set => _attackSpeed = Mathf.Clamp(value, 0.01f, 2.5f);
    }
    [Space(10)]
    [SerializeField] private float _attackSpeed = 0.1f;
    public float AttackCooldown => 1/AttackSpeed;

    public float Attack
    {
        get =>_attack;
        set => _attack = Mathf.Clamp(value, 1f, 1000000f);
    }
    [SerializeField] private float _attack = 20f;
    
    public float Speed
    {
        get => _speed;
        set => _speed = Mathf.Clamp(value, 1f, 100f);
    }
    [SerializeField] private float _speed = 10f;
    
    public float Luck
    {
        get => _luck;
        set => _luck = Mathf.Clamp(value, 0f, 100f);
    }
    [SerializeField] private float _luck = 7f;

    [field: Header("Bullets Stats")]
    [field: SerializeField] public float BulletsMaxRange { get; set; } = 10f;
    [field: SerializeField] public float AdditionalBulletsSpeed { get; set; } = 0f;
    [field: SerializeField] public int AdditionalBulletsPierce { get; set; } = 0;
    [field: SerializeField] public int AdditionalBulletsBounce { get; set; } = 0;
    [field: SerializeField] public float AdditionalBulletsSize { get; set; } = 0f;
    [field: SerializeField] public float BulletKnockBack { get; set; } = 0f;
    

    [field:Header("Damage Multipliers")]
    [field: SerializeField] public List<ElementalMultiplier> DamageMultipliers { get; private set; }
    private Dictionary<Element, float> _damageMultipliers;

    [field:Header("Attack Multipliers")]
    [field: SerializeField] public List<ElementalMultiplier> AttackMultipliers { get; private set; }
    private Dictionary<Element, float> _attackMultipliers;
    

    public float GetReceivedDamage(Element element, float damage)
    {
        return damage * GetDamageMultiplier(element) * (0.5f*defenseCurve.Evaluate(defense/100f) + 0.5f);
    }
    
    public float GetAttack(Element element, float attack)
    {
        return attack * GetAttackMultiplier(element);
    }
    
    public void AddDamageMultiplier(Element element, float multiplier)
    {
        if(_damageMultipliers.ContainsKey(element))
            _damageMultipliers[element] += multiplier;
    }
    
    public void AddAttackMultiplier(Element element, float multiplier)
    {
        if(_attackMultipliers.ContainsKey(element))
            _attackMultipliers[element] += multiplier;
    }
    
    public void SetMaxHealth(float value)
    {
        if(value <= 0) value = 1;
        float oldValue = MaxHealth;
        MaxHealth = value;
        OnMaxHealthChanged?.Invoke(oldValue, MaxHealth);
    }

    public void SetValues(Stats otherStats)
    {
        MaxHealth = otherStats.MaxHealth;
        Defense = otherStats.Defense;
        AttackSpeed = otherStats.AttackSpeed;
        Attack = otherStats.Attack;
        Speed = otherStats.Speed;
        Luck = otherStats.Luck;
        AdditionalBulletsPierce = otherStats.AdditionalBulletsPierce;
        AdditionalBulletsSpeed = otherStats.AdditionalBulletsSpeed;
        AdditionalBulletsBounce = otherStats.AdditionalBulletsBounce;
        AdditionalBulletsSize = otherStats.AdditionalBulletsSize;
        BulletKnockBack = otherStats.BulletKnockBack;
        BulletsMaxRange = otherStats.BulletsMaxRange;
        
        defenseCurve = otherStats.defenseCurve;
        
        DamageMultipliers = new List<ElementalMultiplier>(otherStats.DamageMultipliers);
        AttackMultipliers = new List<ElementalMultiplier>(otherStats.AttackMultipliers);
        
        _damageMultipliers = new Dictionary<Element, float>();
        foreach (var multiplier in DamageMultipliers)
         _damageMultipliers.Add(multiplier.Element, multiplier.Multiplier);
        
        _attackMultipliers = new Dictionary<Element, float>();
        foreach (var multiplier in AttackMultipliers)
            _attackMultipliers.Add(multiplier.Element, multiplier.Multiplier);
    }
    
    private float GetAttackMultiplier(Element element)
    {
       if(_attackMultipliers.ContainsKey(element))
           return _attackMultipliers[element];
       return 1;
    }

    private float GetDamageMultiplier(Element element)
    {
        if(_damageMultipliers.ContainsKey(element))
            return _damageMultipliers[element];
        return 1;
    }
    
    public void MultiplyBasicStats(float value)
    {
        MaxHealth *= value;
        Defense *= value;
        AttackSpeed *= value;
        Attack *= value;
        Speed *= value;
        Luck *= value;
    }
}