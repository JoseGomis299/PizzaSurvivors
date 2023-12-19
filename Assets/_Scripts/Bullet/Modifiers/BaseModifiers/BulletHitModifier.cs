using System.Collections.Generic;
using UnityEngine;

public abstract class BulletHitModifier : BulletModifier
{
    protected readonly GameObject OnHitEffect;
    public int RemainsAfterHit {get; set;}

    protected BulletHitModifier(Bullet target, int maxStacks, int remainsAfterHit, int priority, GameObject onHitEffect) : base(target, maxStacks, priority)
    {
        OnHitEffect = onHitEffect;
        RemainsAfterHit = remainsAfterHit;
    }

    public abstract void OnHit(StatsManager target, Damage damage, List<BulletHitModifier> hitModifiers, Element element);
}