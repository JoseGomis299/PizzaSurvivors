using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public struct BuffData 
{
    [SerializeField] private BuffType buffType;
    
    public float multiplier;
    public IncrementType incrementType;
    
    public StatsManagerEffect GetEffect(StatsManager target)
    {
        return buffType switch
        {
            BuffType.Attack => new AttackBuff(target, 1, 1, TimerType.Infinite, multiplier, incrementType),
            BuffType.Defense => new DefenseBuff(target, 1, 1, TimerType.Infinite, multiplier, incrementType),
            BuffType.Speed => new SpeedBuff(target, 1, 1, TimerType.Infinite, multiplier, incrementType),
            BuffType.Health => new HealthBuff(target, 1, 1, TimerType.Infinite, multiplier, incrementType),
            BuffType.AttackSpeed => new AttackSpeedBuff(target, 1, 1, TimerType.Infinite, multiplier, incrementType),
            BuffType.Luck => new LuckBuff(target, 1, 1, TimerType.Infinite, multiplier, incrementType),
            BuffType.BulletSpeed => new BulletSpeedBuff(target, 1, 1, TimerType.Infinite, multiplier, incrementType),
            BuffType.BulletPierce => new BulletPierceBuff(target, 1, 1, TimerType.Infinite, multiplier, incrementType),
            BuffType.BulletSize => new BulletSizeBuff(target, 1, 1, TimerType.Infinite, multiplier, incrementType),
            BuffType.BulletBounce => new BulletBounceBuff(target, 1, 1, TimerType.Infinite, multiplier, incrementType),
            _ => null
        };
    }
}

public enum BuffType
{
    Attack,
    AttackSpeed,
    Defense,
    Speed,
    Health,
    Luck,
    BulletSpeed,
    BulletPierce,
    BulletSize,
    BulletBounce
}