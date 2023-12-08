public abstract class BulletMovementModifier : BulletModifier
{
    protected float Time;
    protected readonly float Amplitude;
    protected readonly float Frequency;
    
    protected BulletMovementModifier(IEffectTarget target, int maxStacks, int remainsAfterHit, int priority, float amplitude, float frequency) : base(target, maxStacks, remainsAfterHit, priority)
    {
        Amplitude = amplitude;
        Frequency = frequency;
    }
}