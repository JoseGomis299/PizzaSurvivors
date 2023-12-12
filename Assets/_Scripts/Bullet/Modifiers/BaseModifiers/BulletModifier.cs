public abstract class BulletModifier : BaseEffect
{
    protected new Bullet EffectTarget;
    public int RemainsAfterHit {get; set;}
    public readonly int Priority;

    protected BulletModifier(IEffectTarget target, int maxStacks, int remainsAfterHit, int priority) : base(target, 1, maxStacks, TimerType.Infinite)
    {
        EffectTarget = target as Bullet;
        RemainsAfterHit = remainsAfterHit;
        Priority = priority;
    }

    protected override void DeApply() { }
    public override void ReApply() { }
}