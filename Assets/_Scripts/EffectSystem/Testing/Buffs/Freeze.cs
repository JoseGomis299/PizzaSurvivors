using UnityEngine;

public class Freeze : BaseEffect
{
    private Renderer _renderer;
    
    public Freeze(StatsManager target, float duration, int maxStacks, TimerType timerType, Timer timer) : base(target, duration, maxStacks, timerType)
    {
        Timer = timer;
        _renderer = target.GetComponent<Renderer>();
    }
    public override void Apply()
    {
        DeApplyOnTimerEnd();
        if(CurrentStacks >= MaxStacks) return;
        AddStack();

        ((StatsManager)EffectTarget).Stats.BaseSpeed *= 0;
        _renderer.material.color = Color.blue;
    }

    protected override void DeApply()
    {
        if(CurrentStacks <= 0) return;
        RemoveStack();
        
        _renderer.material.color = Color.white;    
    }

    public override void ReApply() { }
}

