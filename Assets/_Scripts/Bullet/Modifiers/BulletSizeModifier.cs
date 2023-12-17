public class BulletSizeModifier : BulletStatsModifier
{
    public BulletSizeModifier(IEffectTarget target, int maxStacks, int remainsAfterHit, int priority, float multiplier) : base(target, maxStacks, remainsAfterHit, priority, multiplier) { }

    public override void Apply()
    {
        EffectTarget.Stats.Size *= Multiplier;
    }
}