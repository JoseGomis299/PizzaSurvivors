using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Character Stats", fileName = "Character Stats")]
public class Stats : ScriptableObject
{
    [field: Header("Base Stats")]
    [field: SerializeField] public float BaseHealth {get; set;} = 100f;
    [field: SerializeField] public float BaseDefense {get; set;} = 10f;
    [field: SerializeField] public float BaseAttackSpeed { get; set; } = 100f;
    [field: SerializeField] public float BaseAttack { get; set; } = 20f;
    [field: SerializeField] public float BaseSpeed { get; set; } = 10f;
    [field: SerializeField] public float BaseLuck { get; set; } = 7f;

    [field: Header("Bullets Stats")]
    [field: SerializeField] public float AdditionalBulletsSpeed { get; set; } = 0f;
    [field: SerializeField] public int AdditionalBulletsPierce { get; set; } = 0;
    [field: SerializeField] public int AdditionalBulletsBounce { get; set; } = 0;
    [field: SerializeField] public float AdditionalBulletsSize { get; set; } = 0f;
    
    
    
    [field:Header("Damage Multipliers")]
    [field: SerializeField] public List<ElementalMultiplier> DamageMultipliers { get; private set; }
    private Dictionary<Element, float> _damageMultipliers;

    [field:Header("Attack Multipliers")]
    [field: SerializeField] public List<ElementalMultiplier> AttackMultipliers { get; private set; }
    private Dictionary<Element, float> _attackMultipliers;

    public float GetReceivedDamage(Element element, float damage)
    {
        return damage * GetDamageMultiplier(element) * (0.4f/(BaseDefense/10f) + 0.6f);
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

    public void SetValues(Stats otherStats)
    {
        BaseHealth = otherStats.BaseHealth;
        BaseDefense = otherStats.BaseDefense;
        BaseAttackSpeed = otherStats.BaseAttackSpeed;
        BaseAttack = otherStats.BaseAttack;
        BaseSpeed = otherStats.BaseSpeed;
        BaseLuck = otherStats.BaseLuck;
        AdditionalBulletsPierce = otherStats.AdditionalBulletsPierce;
        AdditionalBulletsSpeed = otherStats.AdditionalBulletsSpeed;
        
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
}