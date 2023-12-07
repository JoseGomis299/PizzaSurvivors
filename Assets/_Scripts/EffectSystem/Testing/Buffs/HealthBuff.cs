public class HealthBuff : StatsManagerEffect
{
    public HealthBuff(StatsManager target, float duration, int maxStacks, TimerType timerType , float healthIncrement, IncrementType incrementType) : base(target, duration, maxStacks, timerType, healthIncrement, incrementType){}

    public override void Apply()
    {
        DeApplyOnTimerEnd();
        if(CurrentStacks >= MaxStacks) return;
        AddStack();

        EffectTarget.Stats.BaseHealth = IncrementStat(EffectTarget.Stats.BaseHealth, EffectTarget.BaseStats.BaseHealth);
    }

    protected override void DeApply()
    {
        RemoveStack();
    }

    public override void ReApply()
    {
        for (int i = 0; i < CurrentStacks; i++)
        {
            EffectTarget.Stats.BaseHealth = IncrementStat(EffectTarget.Stats.BaseHealth, EffectTarget.BaseStats.BaseHealth);
        }
    }
}

