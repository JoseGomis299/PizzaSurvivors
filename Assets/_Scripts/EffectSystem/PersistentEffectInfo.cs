using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Persistent Effect", fileName = "Persistent Effect")]
public class PersistentEffectInfo : ScriptableObject
{
    [Header("Effect Type")]
    public PersistentEffectType effectType;
    
    [Header("Effect Settings")]
    public float duration;
    public float damage;
    public DamageType damageType;
    public float interval;
    
    public GameObject visualEffect;
    
    public BaseEffect GetEffect(StatsManager target)
    {
        return effectType switch
        {
            PersistentEffectType.Burn => new PersistentDamage(target, duration, damage, Element.Fire, damageType, interval, visualEffect),
            _ => null
        };
    }
}

public enum PersistentEffectType
{
    Burn,
}