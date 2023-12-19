public abstract class BulletModifier 
{
    protected Bullet EffectTarget;
    public readonly int Priority;

    private readonly int _maxStacks;
    protected int CurrentStacks { get; private set; }

    protected BulletModifier(Bullet target, int maxStacks, int priority)
    {
        EffectTarget = target;
        Priority = priority;
        _maxStacks = maxStacks;
    }

    public abstract void Apply();
    
    protected void AddStack()
    {
        if(CurrentStacks < _maxStacks)
            CurrentStacks++;
    }
}