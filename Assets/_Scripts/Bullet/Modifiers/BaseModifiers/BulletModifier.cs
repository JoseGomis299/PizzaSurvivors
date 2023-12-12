public abstract class BulletModifier : IEffect
{
    protected Bullet EffectTarget;
    public int RemainsAfterHit {get; set;}
    public readonly int Priority;
    
    protected int MaxStacks;
    protected int CurrentStacks { get; private set; }

    protected BulletModifier(IEffectTarget target, int maxStacks, int remainsAfterHit, int priority)
    {
        EffectTarget = target as Bullet;
        RemainsAfterHit = remainsAfterHit;
        Priority = priority;
        this.MaxStacks = maxStacks;
    }

    public virtual void Apply() { }
    public virtual void DeApply() { }
    
    protected void AddStack()
    {
        if(CurrentStacks < MaxStacks)
            CurrentStacks++;
    }
}