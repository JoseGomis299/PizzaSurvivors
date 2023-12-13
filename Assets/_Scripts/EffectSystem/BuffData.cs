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
        switch (buffType)
        {
            case BuffType.Attack:
                return new AttackBuff(target, 1, 1, TimerType.Infinite, multiplier, incrementType);
            case BuffType.Defense:
                return new DefenseBuff(target, 1, 1, TimerType.Infinite, multiplier, incrementType);
            case BuffType.Speed:
                return new SpeedBuff(target, 1, 1, TimerType.Infinite, multiplier, incrementType);
            case BuffType.Health:
                return new HealthBuff(target, 1, 1, TimerType.Infinite, multiplier, incrementType);
            case BuffType.AttackSpeed:
                return new AttackSpeedBuff(target, 1, 1, TimerType.Infinite, multiplier, incrementType);
            case BuffType.Luck:
                return new LuckBuff(target, 1, 1, TimerType.Infinite, multiplier, incrementType);
            case BuffType.BulletSpeed:
                return new BulletSpeedBuff(target, 1, 1, TimerType.Infinite, multiplier, incrementType);
            case BuffType.BulletPierce:
                return new BulletPierceBuff(target, 1, 1, TimerType.Infinite, multiplier, incrementType);
            case BuffType.BulletSize:
                return new BulletSizeBuff(target, 1, 1, TimerType.Infinite, multiplier, incrementType);
        }

        return null;
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
    BulletSize
}