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
        string percent = "";
        
        switch (incrementType)
        {
            case IncrementType.Additive:
                switch (multiplier)
                {
                    case > 0:
                        op = "+ ";
                        color = "<color=green>";
                        break;
                    case < 0:
                        multiplier *= -1;
                        op = "- ";
                        color = "<color=red>";
                        break;
                    default:
                        op = "= ";
                        color = "<color=white>";
                        break;
                }
                break;
            case IncrementType.Exponential:
                switch (multiplier)
                {
                    case > 1:
                        op = "+ ";
                        multiplier -= 1;
                        color = "<color=green>";
                        break;
                    case < 1:
                        op = "- ";
                        multiplier = 1 - multiplier;
                        color = "<color=red>";
                        break;
                    default:
                        op = "= ";
                        color = "<color=white>";
                        break;
                }
                percent = "%";
                multiplier *= 100;
                break;
            default:
                op = "= ";
                color = "<color=white>";
                break;
        }

        return $"{color}{op}{multiplier}{percent}</color>";
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