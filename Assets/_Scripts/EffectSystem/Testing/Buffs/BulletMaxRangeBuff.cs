using UnityEngine;

public class BulletMaxRangeBuff : IncrementalEffect
{
    public BulletMaxRangeBuff(StatsManager target, float duration, int maxStacks, TimerType timerType, float bulletRangeIncrement, IncrementType incrementType) : base(target, duration, maxStacks, timerType, bulletRangeIncrement, incrementType) { }
    
    public override void Apply()
    {
        DeApplyOnTimerEnd();
        if(CurrentStacks >= MaxStacks) return;
        AddStack();

        EffectTarget.Stats.BulletsMaxRange = IncrementStat(EffectTarget.Stats.BulletsMaxRange, EffectTarget.BaseStats.BulletsMaxRange);
    }
    
    public override void DeApply()
    {
        RemoveStack();
    }
    
    public override void ReApply()
    {
        for (int i = 0; i < CurrentStacks; i++)
        {
            EffectTarget.Stats.BulletsMaxRange = IncrementStat(EffectTarget.Stats.BulletsMaxRange, EffectTarget.BaseStats.BulletsMaxRange);
        }
    }
}