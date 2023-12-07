using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create Stats", fileName = "Stats")]
public class Stats : ScriptableObject
{
    [Serializable]
    class ElementalMultiplier
    {
        public Element Element;
        public float Multiplier;
    }
    
    [field: Header("Base Stats")]
    [field: SerializeField] public float BaseHealth {get; set;} = 100f;
    [field: SerializeField] public float BaseDefense {get; set;} = 10f;
    [field: SerializeField] public float BaseMana { get; set; } = 100f;
    [field: SerializeField] public float BaseAttack { get; set; } = 20f;
    [field: SerializeField] public float BaseSpeed { get; set; } = 10f;
    [field: SerializeField] public float BaseLuck { get; set; } = 7f;
    
    [Header("Damage Multipliers")]
    [SerializeField] private List<ElementalMultiplier> damageMultipliers;
    private Dictionary<Element, float> _damageMultipliers;

    [Header("Attack Multipliers")]
    [SerializeField] private List<ElementalMultiplier> attackMultipliers;
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
        BaseMana = otherStats.BaseMana;
        BaseAttack = otherStats.BaseAttack;
        BaseSpeed = otherStats.BaseSpeed;
        BaseLuck = otherStats.BaseLuck;
        
        damageMultipliers = new List<ElementalMultiplier>(otherStats.damageMultipliers);
        attackMultipliers = new List<ElementalMultiplier>(otherStats.attackMultipliers);
        
        _damageMultipliers = new Dictionary<Element, float>();
        foreach (var multiplier in damageMultipliers)
         _damageMultipliers.Add(multiplier.Element, multiplier.Multiplier);
        
        _attackMultipliers = new Dictionary<Element, float>();
        foreach (var multiplier in attackMultipliers)
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