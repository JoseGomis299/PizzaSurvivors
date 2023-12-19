using System.Collections.Generic;
using UnityEngine;

public class BurningModifier : BulletHitModifier
{
    private PersistentDamage _debuff;
    private ParticleSystem _particleSystem;
    public BurningModifier(Bullet target, int maxStacks, int remainsAfterHit, int priority, GameObject onHiEffect) : base(target, maxStacks, remainsAfterHit, priority, onHiEffect)
    {
        _particleSystem = onHiEffect.GetComponent<ParticleSystem>();
    }
    
    public override void Apply()
    {
        AddStack();
    }

    public override void OnHit(StatsManager target, Damage damage, List<BulletHitModifier> hitModifiers, Element element)
    {
        if(target == null || !target.gameObject.activeInHierarchy) return;
        
        _debuff ??= new PersistentDamage(target, 3, 5, Element.Fire, DamageType.Additive, 1, OnHitEffect);
        target.ApplyEffect(_debuff);
    }
}