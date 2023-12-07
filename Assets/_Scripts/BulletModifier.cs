public abstract class BulletModifier : BaseEffect
{
    //TODO - Add Bullet class
    //protected readonly Bullet ModifierTarget;

    protected BulletModifier(IEffectTarget target, int maxLevel) : base(target, 1, maxLevel, TimerType.Infinite)
    {
        //ModifierTarget = target as Bullet;
    }
}