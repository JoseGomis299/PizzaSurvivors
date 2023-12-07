public abstract class StatsManagerEffect : IncrementalEffect
{
    protected new readonly StatsManager EffectTarget;

    protected StatsManagerEffect(StatsManager target, float duration, int maxStacks, TimerType timerType,
        float increment, IncrementType incrementType) : base(target, duration, maxStacks, timerType, increment,
        incrementType)
    {
        EffectTarget = target;
    }
}