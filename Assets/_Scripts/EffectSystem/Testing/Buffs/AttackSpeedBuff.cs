public class AttackSpeedBuff : StatsManagerEffect
{
    public AttackSpeedBuff(StatsManager target, float duration, int maxStacks, TimerType timerType, float attackSpeedIncrement, IncrementType incrementType) : base(target, duration, maxStacks, timerType, attackSpeedIncrement, incrementType) { }

    public override void Apply()
    {
        DeApplyOnTimerEnd();
        if(CurrentStacks >= MaxStacks) return;
        AddStack();

        EffectTarget.Stats.BaseAttackSpeed = IncrementStat(EffectTarget.Stats.BaseAttackSpeed, EffectTarget.BaseStats.BaseAttackSpeed);
    }

    public override void DeApply()
    {
        RemoveStack();
    }

    public override void ReApply()
    {
        for (int i = 0; i < CurrentStacks; i++)
        {
            EffectTarget.Stats.BaseAttackSpeed = IncrementStat(EffectTarget.Stats.BaseAttackSpeed, EffectTarget.BaseStats.BaseAttackSpeed);
        }
    }
}