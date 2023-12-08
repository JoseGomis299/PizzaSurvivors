public abstract class BulletStatsModifier : BulletModifier
{
    protected BulletStatsModifier(IEffectTarget target, int maxStacks, int remainsAfterHit, int priority) : base(target, maxStacks, remainsAfterHit, priority)
    {
    }
}