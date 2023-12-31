using UnityEngine;

public class FreezingDebuff : IncrementalEffect
{
    private readonly Freeze _freeze;

    public FreezingDebuff(StatsManager target, float duration, int maxStacks, TimerType timerType, float increment, IncrementType incrementType) : base(target, duration, maxStacks, timerType, increment, incrementType, BuffType.Speed) {
        _freeze = new Freeze(target, duration, 1, timerType, Timer);
    }

    public override void Apply() {
        AddStack();
        DeApplyOnTimerEnd();
        if(CurrentStacks >= MaxStacks) {
            EffectTarget.ApplyEffect(_freeze);
            return;
        }
        
        IncrementStat();
    }

    public override void DeApply()
    {
        RemoveStack();
    }

    public override void ReApply()
    {
        for (int i = 0; i < CurrentStacks; i++)
        {
           IncrementStat();
        }
    }
}



