using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public struct BuffData 
{
    [SerializeField] private BuffType buffType;
    public BuffType Type => buffType;
    
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
            BuffType.BulletMaxRange => new BulletMaxRangeBuff(target, 1, 1, TimerType.Infinite, multiplier, incrementType),
            _ => null
        };
    }

    public override string ToString()
    {
        string op = "";
        string color = "";
        
        switch (incrementType)
        {
            case IncrementType.Additive:
                switch (multiplier)
                {
                    case > 0:
                        op = "+= ";
                        color = "<color=green>";
                        break;
                    case < 0:
                        op = "-= ";
                        color = "<color=red>";
                        break;
                    default:
                        op = "= ";
                        color = "<color=white>";
                        break;
                }
                break;
            case IncrementType.Exponential:
                op = "*= ";
                color = multiplier switch
                {
                    > 1 => "<color=green>",
                    < 1 => "<color=red>",
                    _ => "<color=white>"
                };
                break;
            default:
                op = "= ";
                color = "<color=white>";
                break;
        }

        return $"{color}{op}{multiplier}</color>";
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
    BulletBounce,
    BulletMaxRange
}