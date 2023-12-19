public class BulletSizeModifier : BulletStatsModifier
{
    public BulletSizeModifier(Bullet target, int maxStacks, int priority, float multiplier) : base(target, maxStacks, priority, multiplier) { }

    public override void Apply()
    {
        EffectTarget.Stats.Size *= Multiplier;
    }
}