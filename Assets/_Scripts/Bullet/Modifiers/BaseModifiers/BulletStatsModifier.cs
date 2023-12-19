public abstract class BulletStatsModifier : BulletModifier
{
    protected readonly float Multiplier;

    protected BulletStatsModifier(Bullet target, int maxStacks, int priority, float multiplier) : base(target, maxStacks, priority)
    {
        Multiplier = multiplier;
    }
}