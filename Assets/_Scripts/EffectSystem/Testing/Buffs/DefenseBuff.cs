public class DefenseBuff : StatsManagerEffect
{
    public DefenseBuff(StatsManager target, float duration, int maxStacks, TimerType timerType , float defenseIncrement, IncrementType incrementType) : base(target, duration, maxStacks, timerType, defenseIncrement, incrementType){}

    public override void Apply()
    {
        DeApplyOnTimerEnd();
        if(CurrentStacks >= MaxStacks) return;
        AddStack();
        
        EffectTarget.Stats.BaseDefense = IncrementStat(EffectTarget.Stats.BaseDefense, EffectTarget.BaseStats.BaseDefense);
    }

    protected override void DeApply()
    {
        RemoveStack();
    }

    public override void ReApply()
    {
        for (int i = 0; i < CurrentStacks; i++)
        {
            EffectTarget.Stats.BaseDefense = IncrementStat(EffectTarget.Stats.BaseDefense, EffectTarget.BaseStats.BaseDefense);
        }
    }
}