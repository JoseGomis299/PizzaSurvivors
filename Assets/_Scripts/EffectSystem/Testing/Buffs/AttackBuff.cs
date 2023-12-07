using UnityEngine;

public class AttackBuff : StatsManagerEffect
{
    public AttackBuff(StatsManager target, float duration, int maxStacks, TimerType timerType, float attackIncrement, IncrementType incrementType) : base(target, duration, maxStacks, timerType, attackIncrement, incrementType) { }

    public override void Apply()
    {
        DeApplyOnTimerEnd();
        if(CurrentStacks >= MaxStacks) return;
        AddStack();

        EffectTarget.Stats.BaseAttack = IncrementStat(EffectTarget.Stats.BaseAttack, EffectTarget.BaseStats.BaseAttack);
        Debug.Log($"Buff applied, attack is now: {EffectTarget.Stats.BaseAttack} stacks: {CurrentStacks} max stacks: {MaxStacks}");
    }

    protected override void DeApply()
    {
        RemoveStack();
    }

    public override void ReApply()
    {
        for (int i = 0; i < CurrentStacks; i++)
        {
            EffectTarget.Stats.BaseAttack = IncrementStat(EffectTarget.Stats.BaseAttack, EffectTarget.BaseStats.BaseAttack);
        }
    }
}
