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
    
    public IncrementalEffect GetEffect(StatsManager target)
    {
        return new IncrementalEffect(target, 1, 1, TimerType.Infinite, multiplier, incrementType, buffType);
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
    BulletMaxRange,
    BulletKnockBack
}