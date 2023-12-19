public class BulletPierceBuff : IncrementalEffect
{
    public BulletPierceBuff(StatsManager target, float duration, int maxStacks, TimerType timerType, float bulletPierceIncrement, IncrementType incrementType) : base(target, duration, maxStacks, timerType, bulletPierceIncrement, incrementType) { }
    
    public override void Apply()
    {
        DeApplyOnTimerEnd();
        if(CurrentStacks >= MaxStacks) return;
        AddStack();

        EffectTarget.Stats.AdditionalBulletsPierce = (int) IncrementStat(EffectTarget.Stats.AdditionalBulletsPierce, EffectTarget.BaseStats.AdditionalBulletsPierce);
    }
    
    public override void DeApply()
    {
        RemoveStack();
    }
    
    public override void ReApply()
    {
        for (int i = 0; i < CurrentStacks; i++)
        {
            EffectTarget.Stats.AdditionalBulletsPierce = (int) IncrementStat(EffectTarget.Stats.AdditionalBulletsPierce, EffectTarget.BaseStats.AdditionalBulletsPierce);
        }
    }
}