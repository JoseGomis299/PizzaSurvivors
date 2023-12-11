using System.Collections;
using System.Collections.Generic;

public abstract class BulletShootModifier : BulletModifier
{
    protected new readonly BulletSpawner EffectTarget;
    protected List<BulletModifierInfo> Modifiers;

    protected BulletShootModifier(IEffectTarget target, int maxStacks, int remainsAfterHit, int priority, List<BulletModifierInfo> modifiers) : base(target,
        maxStacks, remainsAfterHit, priority)
    {
        EffectTarget = target as BulletSpawner;
        Modifiers = modifiers;
    }

    public abstract List<BulletShotData> GetModifications(BulletShotData baseData);
}