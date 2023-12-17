public class SpeedBuff : StatsManagerEffect
{
    public SpeedBuff(StatsManager target, float duration, int maxStacks, TimerType timerType , float speedIncrement, IncrementType incrementType) : base(target, duration, maxStacks, timerType, speedIncrement, incrementType){}

    public override void Apply()
    {
        DeApplyOnTimerEnd();
        if(CurrentStacks >= MaxStacks) return;
        AddStack();
        
        EffectTarget.Stats.Speed = IncrementStat(EffectTarget.Stats.Speed, EffectTarget.BaseStats.Speed);
    }

    public override void DeApply()
    {
        RemoveStack();
    }

    public override void ReApply()
    {
        for (int i = 0; i < CurrentStacks; i++)
        {
            EffectTarget.Stats.Speed = IncrementStat(EffectTarget.Stats.Speed, EffectTarget.BaseStats.Speed);
        }
    }
}