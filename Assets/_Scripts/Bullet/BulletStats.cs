using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Bullet Stats", fileName = "Bullet Stats", order = 0)]
public class BulletStats : ScriptableObject
{
    [field: Header("Base Stats")]
    [field: SerializeField] public float BaseDamage { get; set; } = 20f;
    [field: SerializeField] public float BaseSpeed { get; set; } = 10f;
    [field: SerializeField] public float BaseSize { get; set; } = 1f;
    
    [Header("Attack Multipliers")]
    [SerializeField] private List<ElementalMultiplier> attackMultipliers = new List<ElementalMultiplier>();
    private Dictionary<Element, float> _attackMultipliers = new Dictionary<Element, float>();

    public float GetAttack(Element element, float attack)
    {
        return attack * GetAttackMultiplier(element);
    }
    
    public void SetValues(BulletStats bulletStats)
    {
        BaseDamage = bulletStats.BaseDamage;
        BaseSpeed = bulletStats.BaseSpeed;
        BaseSize = bulletStats.BaseSize;
    }
    
    public void SetValues(Stats characterStats, BulletStats bulletStats)
    {
        BaseDamage = bulletStats.BaseDamage;
        BaseSpeed = bulletStats.BaseSpeed;
        BaseSize = bulletStats.BaseSize;
        
        List<ElementalMultiplier> multipliers = new List<ElementalMultiplier>(characterStats.AttackMultipliers);
        
        _attackMultipliers = new Dictionary<Element, float>();
        foreach (var multiplier in multipliers)
            _attackMultipliers.Add(multiplier.Element, multiplier.Multiplier);

        if(attackMultipliers == null) return;
        
        foreach (var multiplier in attackMultipliers)
        {
            if (_attackMultipliers.ContainsKey(multiplier.Element))
                _attackMultipliers[multiplier.Element] = multiplier.Multiplier;
            else
                _attackMultipliers.Add(multiplier.Element, multiplier.Multiplier);
        }
    }
    
    public void AddAttackMultiplier(Element element, float multiplier)
    {
        if(_attackMultipliers.ContainsKey(element))
            _attackMultipliers[element] += multiplier;
    }
    
    private float GetAttackMultiplier(Element element)
    {
        if(_attackMultipliers.ContainsKey(element))
            return _attackMultipliers[element];
        return 1;
    }
}