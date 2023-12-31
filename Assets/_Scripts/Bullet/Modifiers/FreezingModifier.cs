using System.Collections.Generic;
using UnityEngine;

public class FreezingModifier : BulletHitModifier
{
    public FreezingModifier(Bullet target, int maxStacks, int remainsAfterHit, int priority, GameObject onHiEffect) : base(target, maxStacks, remainsAfterHit, priority, onHiEffect)
    {
    }
    
    public override void Apply()
    {
        AddStack();
        EffectTarget.Stats.Element = Element.Ice;
    }
    
    public override void OnHit(StatsManager target, Damage damage, List<BulletHitModifier> hitModifiers, Element element)
    {
        if(target == null || !target.gameObject.activeInHierarchy) return;
        FreezingDebuff debuff = new FreezingDebuff(target, 3, 5, 0, -0.2f, IncrementType.AddBase);
        target.ApplyEffect(debuff);
    }
}