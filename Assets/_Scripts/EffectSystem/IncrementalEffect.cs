using UnityEngine;

public abstract class IncrementalEffect : BaseEffect {
    
    protected readonly float increment;
    protected readonly IncrementType incrementType;
    
    protected IncrementalEffect(IEffectTarget target, float duration, int maxStacks, TimerType timerType, float increment, IncrementType incrementType) : base(target, duration, maxStacks, timerType)
    {
        this.increment = increment;
        this.incrementType = incrementType;
    }
    
    protected float IncrementStat(float currentStat, float baseStat)
    {
        switch (incrementType)
        {
            case IncrementType.AddBase:
                currentStat += baseStat * increment;
                break;
            case IncrementType.Exponential:
                currentStat *= increment;
                break;
            case IncrementType.Additive:
                currentStat += increment;
                break;
            case IncrementType.KeepBest:
                currentStat = Mathf.Max(currentStat, baseStat * increment);
                break;
            case IncrementType.KeepBestAdditive:
                currentStat = Mathf.Max(currentStat, baseStat + increment);
                break;
            case IncrementType.Set:
                currentStat = increment;
                break;
        }

        if(this is not BulletPierceBuff && currentStat < 0) currentStat = 0;
        return currentStat;
    }
}

public enum IncrementType
{
    /// <summary>
    /// Stat = Stat + Increment
    /// </summary>
    [Tooltip("Stat = Stat + Increment")]
    Additive,
    /// <summary>
    /// Stat = Stat + (BaseStat * Increment)
    /// </summary>
    [Tooltip("Stat = Stat + (BaseStat * Increment)")]
    AddBase,
    /// <summary>
    /// Stat = Max(Stat, BaseStat * Increment)
    /// </summary>
    [Tooltip("Stat = Max(Stat, BaseStat * Increment)")]
    KeepBest,
    /// <summary>
    /// Stat = Max(Stat, BaseStat + Increment)
    /// </summary>
    [Tooltip("Stat = Max(Stat, BaseStat + Increment)")]
    KeepBestAdditive,
    /// <summary>
    /// Stat = Stat * Increment
    /// </summary>
    [Tooltip("Stat = Stat * Increment")]
    Exponential,
    Set
}