public class BulletKnockBackBuff : IncrementalEffect
{
    public BulletKnockBackBuff(StatsManager target, float duration, int maxStacks, TimerType timerType , float knockBackIncrement, IncrementType incrementType) : base(target, duration, maxStacks, timerType, knockBackIncrement, incrementType){}

    public override void Apply()
    {
        DeApplyOnTimerEnd();
        if(CurrentStacks >= MaxStacks) return;
        AddStack();
        
        EffectTarget.Stats.BulletKnockBack = IncrementStat(EffectTarget.Stats.BulletKnockBack, EffectTarget.BaseStats.BulletKnockBack);
    }

    public override void DeApply()
    {
        RemoveStack();
    }

    public override void ReApply()
    {
        for (int i = 0; i < CurrentStacks; i++)
        {
            EffectTarget.Stats.BulletKnockBack = IncrementStat(EffectTarget.Stats.BulletKnockBack, EffectTarget.BaseStats.BulletKnockBack);
        }
    }
}