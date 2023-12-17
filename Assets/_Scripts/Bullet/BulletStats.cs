using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Bullet Stats", fileName = "Bullet Stats", order = 0)]
public class BulletStats : ScriptableObject
{
    [field: Header("Base Stats")]
    [field: SerializeField] public float Damage { get; set; } = 20f;
    [field: SerializeField] public float Speed { get; set; } = 10f;
    [field: SerializeField] public float Size { get; set; } = 1f;
    
    [field: SerializeField] public int Pierce { get; set; } = 0;
    [field: SerializeField] public int Bounce { get; set; } = 0;
    
    [field: SerializeField] public Element Element { get; set; } = 0;

    private Dictionary<Element, float> _attackMultipliers = new Dictionary<Element, float>();

    public float GetAttack(Element element, float attack)
    {
        return attack * GetAttackMultiplier(element);
    }
    
    public void SetValues(Stats characterStats, BulletStats bulletStats)
    {
        Damage = bulletStats.Damage + characterStats.Attack;
        Speed = bulletStats.Speed + characterStats.AdditionalBulletsSpeed;
        Size = bulletStats.Size + characterStats.AdditionalBulletsSize;
        Pierce = bulletStats.Pierce + characterStats.AdditionalBulletsPierce;
        Bounce = bulletStats.Bounce + characterStats.AdditionalBulletsBounce;
        
        List<ElementalMultiplier> multipliers = new List<ElementalMultiplier>(characterStats.AttackMultipliers);
        
        _attackMultipliers = new Dictionary<Element, float>();
        foreach (var multiplier in multipliers)
            _attackMultipliers.Add(multiplier.Element, multiplier.Multiplier);
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