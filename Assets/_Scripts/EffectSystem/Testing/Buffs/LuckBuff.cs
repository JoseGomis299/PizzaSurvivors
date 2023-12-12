public class LuckBuff : StatsManagerEffect
{
    public LuckBuff(StatsManager target, float duration, int maxStacks, TimerType timerType, float luckIncrement, IncrementType incrementType) : base(target, duration, maxStacks, timerType, luckIncrement, incrementType) { }

    public override void Apply()
    {
        DeApplyOnTimerEnd();
        if(CurrentStacks >= MaxStacks) return;
        AddStack();

        EffectTarget.Stats.BaseLuck = IncrementStat(EffectTarget.Stats.BaseLuck, EffectTarget.BaseStats.BaseLuck);
    }

    public override void DeApply()
    {
        RemoveStack();
    }

    public override void ReApply()
    {
        for (int i = 0; i < CurrentStacks; i++)
        {
            EffectTarget.Stats.BaseLuck = IncrementStat(EffectTarget.Stats.BaseLuck, EffectTarget.BaseStats.BaseLuck);
        }
    }
}