using UnityEngine;

public class Freeze : BaseEffect
{
    private SpriteRenderer _renderer;
    
    public Freeze(IEffectTarget target, float duration, int maxStacks, TimerType timerType, Timer timer) : base(target, duration, maxStacks, timerType)
    {
        Timer = timer;
        _renderer = ((MonoBehaviour)target).GetComponent<SpriteRenderer>();
    }
    public override void Apply()
    {
        DeApplyOnTimerEnd();
        if(CurrentStacks >= MaxStacks) return;
        AddStack();

        ((StatsManager)EffectTarget).Stats.BaseSpeed *= 0;
        _renderer.color = Color.blue;
    }

    protected override void DeApply()
    {
        if(CurrentStacks <= 0) return;
        RemoveStack();
        
        _renderer.color = Color.white;    
    }

    public override void ReApply() { }
}

