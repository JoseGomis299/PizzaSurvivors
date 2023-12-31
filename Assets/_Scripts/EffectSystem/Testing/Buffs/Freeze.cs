using UnityEngine;

public class Freeze : BaseEffect
{
    private SpriteRenderer _renderer;
    
    public Freeze(StatsManager target, float duration, int maxStacks, TimerType timerType, Timer timer) : base(target, duration, maxStacks, timerType)
    {
        Timer = timer;
        _renderer = ((MonoBehaviour)target).GetComponent<SpriteRenderer>();
    }
    public override void Apply()
    {
        DeApplyOnTimerEnd();
        if(CurrentStacks >= MaxStacks) return;
        AddStack();

        ((StatsManager)EffectTarget).Stats.Speed *= 0;
        _renderer.color = Color.blue;
    }

    public override void DeApply()
    {
        if(CurrentStacks <= 0) return;
        RemoveStack();
        
        _renderer.color = Color.white;    
    }

    public override void ReApply() { }
}

