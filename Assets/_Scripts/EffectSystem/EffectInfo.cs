using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Stats Effect", fileName = "Stats Effect")]
public class EffectInfo : ScriptableObject
{
    [Header("Effect Type")]
    public EffectType effectType;
    
    [Header("Effect Settings")]
    public float multiplier;
    public int maxStacks;
    public float duration;
    
    [Space(10)]
    public IncrementType incrementType;
    public TimerType timerType;
    
    public BaseEffect GetEffect(StatsManager target)
    {
        return effectType switch
        {
            EffectType.FreezingDebuff => new FreezingDebuff(target, duration, maxStacks, timerType, multiplier, incrementType),
            _ => null
        };
    }
}

public enum EffectType
{
    AttackBuff,
    DefenseBuff,
    SpeedBuff,
    HealthBuff,
    FreezingDebuff
}

