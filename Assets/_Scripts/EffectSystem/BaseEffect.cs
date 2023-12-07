using UnityEngine;

public abstract class BaseEffect {
    
    protected Timer Timer;
    private readonly TimerType _timerType;

    public float Duration
    {
        get => Timer.Duration;
        protected set => Timer.SetDuration(value);
    }
    public float ElapsedTime => Timer.ElapsedTime;

    public int CurrentStacks { get; private set; }
    public readonly int MaxStacks;
    
    protected readonly IEffectTarget EffectTarget;
    
    protected BaseEffect(IEffectTarget target, float duration, int maxStacks, TimerType timerType)
    {
        EffectTarget = target;
        MaxStacks = maxStacks;
        
        Timer = new Timer(duration, target as MonoBehaviour);
        _timerType = timerType;
    }

    /// <summary>
    /// Applies the effect to the target, increasing the current stacks.
    /// </summary>
    public abstract void Apply();
    
    /// <summary>
    /// De-applies the effect from the target, decreasing the current stacks.
    /// </summary>
    protected abstract void DeApply();
    
    /// <summary>
    /// Applies the effect current stacks times without applying the ApplyEffect logic.
    /// </summary>
    public abstract void ReApply();
    
    /// <summary>
    /// Starts the timer and sets the OnTimerEnd event to DeApply.
    /// </summary>
    protected void DeApplyOnTimerEnd()
    {
        StartTimer();
        Timer.OnTimerEnd -= DeApply;
        Timer.OnTimerEnd += DeApply;
    }
    
    private void StartTimer()
    {
        switch (_timerType)
        {
            case TimerType.Default:
            case TimerType.Stacking:
                Timer.ResetAndStartTimer();
                break;
            case TimerType.Infinite:
                break;
        }
    }

    protected void AddStack()
    {
        if(CurrentStacks < MaxStacks) CurrentStacks++;
    }
    
    protected void RemoveStack()
    {
        if(CurrentStacks <= 0) return;
        
        switch (_timerType)
        {
            case TimerType.Default:
                CurrentStacks = 0;
                break;
            case TimerType.Infinite:
                CurrentStacks--;
                break;
            case TimerType.Stacking:
                CurrentStacks--;
                if (CurrentStacks > 0) Timer.ResetAndStartTimer();
                break;
        }

        EffectTarget.ReApplyEffects();
    }
    
}

public enum TimerType
{
    /// <summary>
    /// Resets the timer when the effect is applied.
    /// When the timer ends, the effect will be removed.
    /// </summary>
    [Tooltip("Resets the timer when the effect is applied. When the timer ends, the effect will be removed.")]
    Default,
    /// <summary>
    /// Infinite timer, the effect will never be removed.
    /// </summary>
    [Tooltip("Infinite timer, the effect will never be removed.")]
    Infinite,
    /// <summary>
    /// Each time the effect is applied, the timer will be reset.
    /// When the timer ends, if there are still stacks left, the timer will be reset;
    /// </summary>
    [Tooltip("Each time the effect is applied, the timer will be reset. When the timer ends, if there are still stacks left, the timer will be reset;")]
    Stacking
}