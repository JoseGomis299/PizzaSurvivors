public abstract class BulletStatsModifier : BulletModifier
{
    protected readonly float Multiplier;

    protected BulletStatsModifier(IEffectTarget target, int maxStacks, int remainsAfterHit, int priority, float multiplier) : base(target, maxStacks, remainsAfterHit, priority)
    {
        Multiplier = multiplier;
    }
}