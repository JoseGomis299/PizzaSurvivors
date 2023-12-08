public abstract class BulletHitModifier : BulletModifier
{
    protected BulletHitModifier(IEffectTarget target, int maxStacks, int remainsAfterHit, int priority) : base(target, maxStacks, remainsAfterHit, priority)
    {
    }
}