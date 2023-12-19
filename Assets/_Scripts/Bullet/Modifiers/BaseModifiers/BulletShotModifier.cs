using System.Collections;
using System.Collections.Generic;

public abstract class BulletShotModifier : BulletModifier
{
    protected new readonly BulletSpawner EffectTarget;
    protected List<BulletModifierInfo> Modifiers;

    protected BulletShotModifier(BulletSpawner target, int maxStacks, int priority, List<BulletModifierInfo> modifiers) : base(null, maxStacks, priority)
    {
        EffectTarget = target;
        Modifiers = modifiers;
    }

    public abstract List<BulletShotData> GetModifiedShotData(BulletShotData baseData);
}