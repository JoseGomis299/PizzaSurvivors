public class BulletSizeBuff : IncrementalEffect
{
    public BulletSizeBuff(StatsManager target, float duration, int maxStacks, TimerType timerType, float bulletSizeIncrement, IncrementType incrementType) : base(target, duration, maxStacks, timerType, bulletSizeIncrement, incrementType) { }
    
    public override void Apply()
    {
        DeApplyOnTimerEnd();
        if(CurrentStacks >= MaxStacks) return;
        AddStack();

        EffectTarget.Stats.AdditionalBulletsSize = IncrementStat(EffectTarget.Stats.AdditionalBulletsSize, EffectTarget.BaseStats.AdditionalBulletsSize);
    }
    
    public override void DeApply()
    {
        RemoveStack();
    }
    
    public override void ReApply()
    {
        for (int i = 0; i < CurrentStacks; i++)
        {
            EffectTarget.Stats.AdditionalBulletsSize = IncrementStat(EffectTarget.Stats.AdditionalBulletsSize, EffectTarget.BaseStats.AdditionalBulletsSize);
        }
    }
}