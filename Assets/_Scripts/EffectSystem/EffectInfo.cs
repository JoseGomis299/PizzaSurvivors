using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Stats Effect", fileName = "Stats Effect", order = 0)]
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
        switch (effectType)
        {
            case EffectType.AttackBuff:
                return new AttackBuff(target, duration, maxStacks, timerType, multiplier, incrementType);
            case EffectType.DefenseBuff:
                return new DefenseBuff(target, duration, maxStacks, timerType, multiplier, incrementType);
            case EffectType.SpeedBuff:
                return new SpeedBuff(target, duration, maxStacks, timerType, multiplier, incrementType);
            case EffectType.HealthBuff:
                return new HealthBuff(target, duration, maxStacks, timerType, multiplier, incrementType);
            case EffectType.FreezingDebuff:
                return new FreezingDebuff(target, duration, maxStacks, timerType, multiplier, incrementType);
        }

        return null;
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