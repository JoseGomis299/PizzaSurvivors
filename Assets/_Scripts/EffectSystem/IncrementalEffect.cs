using UnityEngine;

public class IncrementalEffect : BaseEffect {
    
    public float Increment;
    public readonly IncrementType IncrementType;

    public readonly BuffType Type;

    public IncrementalEffect(StatsManager target, float duration, int maxStacks, TimerType timerType, float increment, IncrementType incrementType, BuffType type) : base(target, duration, maxStacks, timerType)
    {
        this.Increment = increment;
        this.IncrementType = incrementType;
        Type = type;
    }
    
    public override void Apply()
    {
        DeApplyOnTimerEnd();
        if(CurrentStacks >= MaxStacks) return;
        AddStack();

        IncrementStat();
    }

    public override void DeApply()
    {
        RemoveStack();
    }

    public override void ReApply()
    {
        for (int i = 0; i < CurrentStacks; i++)
        {
           IncrementStat();
        }
    }
    
    public bool IsEqual(IncrementalEffect other)
    {
        return other.Type == Type && other.IncrementType == IncrementType;
    }
    
    public bool IncreaseValue(float value)
    {
        switch (IncrementType)
        {
            case IncrementType.AddBase:
                if(value > 1) Increment += value - 1;
                else if(value <= 0) Increment = 0;
                else if(value < 1) Increment -= 1 - value;
                return true;
            case IncrementType.Multiplicative:
                if(value > 1) Increment += value - 1;
                else if(value <= 0) Increment = 0;
                else if(value < 1) Increment -= 1 - value;
                if(Increment < 0) Increment = 0;
                return true;
            case IncrementType.KeepBestAdditive:
            case IncrementType.KeepBest:
            case IncrementType.Additive:
                Increment += value; 
                return true;
        }
        return false;
    }
    
    protected void IncrementStat()
    {
        GetCurrentStat(out float currentStat, out float baseStat);

        switch (IncrementType)
        {
            case IncrementType.AddBase:
                currentStat += baseStat * Increment;
                break;
            case IncrementType.Exponential:
            case IncrementType.Multiplicative:
                currentStat *= Increment;
                break;
            case IncrementType.Additive:
                currentStat += Increment;
                break;
            case IncrementType.KeepBest:
                currentStat = Mathf.Max(currentStat, baseStat * Increment);
                break;
            case IncrementType.KeepBestAdditive:
                currentStat = Mathf.Max(currentStat, baseStat + Increment);
                break;
            case IncrementType.Set:
                currentStat = Increment;
                break;
        }

        if(Type == BuffType.BulletPierce && currentStat < 0) currentStat = 0;
        SetCurrentStat(currentStat);
    }

    private void SetCurrentStat(float value)
    {
        switch (Type)
        {
            case BuffType.Attack:
                EffectTarget.Stats.Attack = value;
                break;
            case BuffType.Defense:
                EffectTarget.Stats.Defense = value;
                break;
            case BuffType.Speed:
                EffectTarget.Stats.Speed = value;
                break;
            case BuffType.Health:
                EffectTarget.Stats.SetMaxHealth(value);
                break;
            case BuffType.Luck:
                EffectTarget.Stats.Luck = value;
                break;
            case BuffType.AttackSpeed:
                EffectTarget.Stats.AttackSpeed = value;
                break;
            case BuffType.BulletBounce:
                EffectTarget.Stats.AdditionalBulletsBounce = (int) value;
                break;
            case BuffType.BulletPierce:
                EffectTarget.Stats.AdditionalBulletsPierce = (int) value;
                break;
            case BuffType.BulletSize:
                EffectTarget.Stats.AdditionalBulletsSize = value;
                break;
            case BuffType.BulletSpeed:
                EffectTarget.Stats.AdditionalBulletsSpeed = value;
                break;
            case BuffType.BulletKnockBack:
                EffectTarget.Stats.BulletKnockBack = value;
                break;
            case BuffType.BulletMaxRange:
                EffectTarget.Stats.BulletsMaxRange = value;
                break;
        }
    }

    private void GetCurrentStat(out float currentStat, out float baseStat)
    {
        currentStat = 0;
        baseStat = 0;
        
        switch (Type)
        {
            case BuffType.Attack:
                currentStat = EffectTarget.Stats.Attack;
                baseStat = EffectTarget.BaseStats.Attack;
                break;
            case BuffType.Defense:
                currentStat = EffectTarget.Stats.Defense;
                baseStat = EffectTarget.BaseStats.Defense;
                break;
            case BuffType.Speed:
                currentStat = EffectTarget.Stats.Speed;
                baseStat = EffectTarget.BaseStats.Speed;
                break;
            case BuffType.Health:
                currentStat = EffectTarget.Stats.MaxHealth;
                baseStat = EffectTarget.BaseStats.MaxHealth;
                break;
            case BuffType.Luck:
                currentStat = EffectTarget.Stats.Luck;
                baseStat = EffectTarget.BaseStats.Luck;
                break;
            case BuffType.AttackSpeed:
                currentStat = EffectTarget.Stats.AttackSpeed;
                baseStat = EffectTarget.BaseStats.AttackSpeed;
                break;
            case BuffType.BulletBounce:
                currentStat = EffectTarget.Stats.AdditionalBulletsBounce;
                baseStat = EffectTarget.BaseStats.AdditionalBulletsBounce;
                break;
            case BuffType.BulletPierce:
                currentStat = EffectTarget.Stats.AdditionalBulletsPierce;
                baseStat = EffectTarget.BaseStats.AdditionalBulletsPierce;
                break;
            case BuffType.BulletSize:
                currentStat = EffectTarget.Stats.AdditionalBulletsSize;
                baseStat = EffectTarget.BaseStats.AdditionalBulletsSize;
                break;
            case BuffType.BulletSpeed:
                currentStat = EffectTarget.Stats.AdditionalBulletsSpeed;
                baseStat = EffectTarget.BaseStats.AdditionalBulletsSpeed;
                break;
            case BuffType.BulletKnockBack:
                currentStat = EffectTarget.Stats.BulletKnockBack;
                baseStat = EffectTarget.BaseStats.BulletKnockBack;
                break;
            case BuffType.BulletMaxRange:
                currentStat = EffectTarget.Stats.BulletsMaxRange;
                baseStat = EffectTarget.BaseStats.BulletsMaxRange;
                break;
        }
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
    /// If we have another IncremantalEffect with Multiplicative, the value is added to the current value
    /// </summary>
    [Tooltip("Stat = Stat * Increment")]
    Multiplicative,
    /// <summary>
    /// Stat = Stat * Increment
    /// <summary>
    Exponential,
    Set
}