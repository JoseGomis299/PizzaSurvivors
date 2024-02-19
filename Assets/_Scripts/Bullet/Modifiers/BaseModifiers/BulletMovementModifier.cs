public abstract class BulletMovementModifier : BulletModifier
{
    protected float _time;
    protected readonly float amplitude;
    protected readonly float frequency;
    
    protected BulletMovementModifier(Bullet target, int maxStacks, int priority, float amplitude, float frequency) : base(target, maxStacks, priority)
    {
        this.amplitude = amplitude;
        this.frequency = frequency;
    }
    
    public abstract void ModifyMovement();
    
}