public abstract class BulletMovementModifier : BulletModifier
{
    protected float time;
    protected readonly float amplitude;
    protected readonly float frequency;
    
    protected BulletMovementModifier(IEffectTarget target, int maxStacks, int remainsAfterHit, int priority, float amplitude, float frequency) : base(target, maxStacks, remainsAfterHit, priority)
    {
        this.amplitude = amplitude;
        this.frequency = frequency;
    }
    
    public abstract void ModifyMovement();
    
}