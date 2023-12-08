using System.Collections;
using System.Collections.Generic;

public abstract class BulletShootModifier : BulletModifier
{
    protected new readonly BulletSpawner EffectTarget;

    protected BulletShootModifier(IEffectTarget target, int maxStacks, int remainsAfterHit, int priority) : base(target,
        maxStacks, remainsAfterHit, priority)
    {
        EffectTarget = target as BulletSpawner;
    }

    public abstract List<BulletMovementData> GetModifications(BulletMovementData baseData);
}