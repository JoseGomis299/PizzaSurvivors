using System.Collections.Generic;
using UnityEngine;

public abstract class BulletHitModifier : BulletModifier
{
    protected readonly GameObject OnHitEffect;

    protected BulletHitModifier(IEffectTarget target, int maxStacks, int remainsAfterHit, int priority, GameObject onHitEffect) : base(target, maxStacks, remainsAfterHit, priority)
    {
        OnHitEffect = onHitEffect;
    }

    public abstract void OnHit(IEffectTarget target, float damage, List<BulletHitModifier> hitModifiers);
}