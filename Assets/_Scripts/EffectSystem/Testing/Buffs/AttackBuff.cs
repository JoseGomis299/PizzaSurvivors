using UnityEngine;

public class AttackBuff : StatsManagerEffect
{
    public AttackBuff(StatsManager target, float duration, int maxStacks, TimerType timerType, float attackIncrement, IncrementType incrementType) : base(target, duration, maxStacks, timerType, attackIncrement, incrementType) { }

    public override void Apply()
    {
        DeApplyOnTimerEnd();
        if(CurrentStacks >= MaxStacks) return;
        AddStack();

        EffectTarget.Stats.Attack = IncrementStat(EffectTarget.Stats.Attack, EffectTarget.BaseStats.Attack);
    }

    public override void DeApply()
    {
        RemoveStack();
    }

    public override void ReApply()
    {
        for (int i = 0; i < CurrentStacks; i++)
        {
            EffectTarget.Stats.Attack = IncrementStat(EffectTarget.Stats.Attack, EffectTarget.BaseStats.Attack);
        }
    }
}