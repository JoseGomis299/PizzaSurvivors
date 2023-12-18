using System.Collections.Generic;
using UnityEngine;

public class BurningModifier : BulletHitModifier
{
    private PersistentDamage _debuff;
    private ParticleSystem _particleSystem;
    public BurningModifier(IEffectTarget target, int maxStacks, int remainsAfterHit, int priority, GameObject onHiEffect) : base(target, maxStacks, remainsAfterHit, priority, onHiEffect)
    {
        _particleSystem = onHiEffect.GetComponent<ParticleSystem>();
    }
    
    public override void Apply()
    {
        AddStack();
    }

    public override void DeApply()
    {
        _debuff.DeApply();
    }

    public override void OnHit(IEffectTarget target, float damage, List<BulletHitModifier> hitModifiers, Element element)
    {
        if(target == null || !((MonoBehaviour)target).gameObject.activeInHierarchy) return;
        
        _debuff ??= new PersistentDamage(target, 3, 5, Element.Fire, DamageType.Additive, 1, OnHitEffect);
        target.ApplyEffect(_debuff);
    }
}