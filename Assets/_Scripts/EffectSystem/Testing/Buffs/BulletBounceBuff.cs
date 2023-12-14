public class BulletBounceBuff : StatsManagerEffect
{
    public BulletBounceBuff(StatsManager target, float duration, int maxStacks, TimerType timerType, float bulletBounceIncrement, IncrementType incrementType) : base(target, duration, maxStacks, timerType, bulletBounceIncrement, incrementType) { }
    
    public override void Apply()
    {
        DeApplyOnTimerEnd();
        if(CurrentStacks >= MaxStacks) return;
        AddStack();

        EffectTarget.Stats.AdditionalBulletsBounce = (int) IncrementStat(EffectTarget.Stats.AdditionalBulletsBounce, EffectTarget.BaseStats.AdditionalBulletsBounce);
    }
    
    public override void DeApply()
    {
        RemoveStack();
    }
    
    public override void ReApply()
    {
        for (int i = 0; i < CurrentStacks; i++)
        {
            EffectTarget.Stats.AdditionalBulletsBounce = (int) IncrementStat(EffectTarget.Stats.AdditionalBulletsBounce, EffectTarget.BaseStats.AdditionalBulletsBounce);
        }
    }
}