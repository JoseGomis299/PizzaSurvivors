using System.Collections.Generic;
using ProjectUtils.ObjectPooling;
using UnityEngine;

public class ExplosiveModifier : BulletHitModifier
{
    public ExplosiveModifier(IEffectTarget target, int maxStacks, int remainsAfterHit, int priority, GameObject onHiEffect) : base(target, maxStacks, remainsAfterHit, priority, onHiEffect)
    {
    }
    public override void Apply()
    {
        AddStack();
    }
    
    public override void OnHit(IEffectTarget target, float damage, List<BulletHitModifier> hitModifiers)
    {
        var collisions = Physics2D.OverlapCircleAll(EffectTarget.transform.position, EffectTarget.Stats.BaseSize * 1.5f);
        foreach (var col in collisions)
        {
            if(col.gameObject == EffectTarget.gameObject || !col.TryGetComponent(out IDamageable damageable)) continue;
            damageable.TakeDamage(damage * 0.5f);
            
            if(target != null && col.gameObject == ((MonoBehaviour)target).gameObject) continue;
            foreach (var modifier in hitModifiers)
            {
                modifier.OnHit(col.GetComponent<IEffectTarget>(), damage * 0.5f, hitModifiers);
            }
        }

        GameObject effect = ObjectPool.Instance.InstantiateFromPool(OnHitEffect, EffectTarget.transform.position, Quaternion.identity, true);
        effect.transform.localScale = Vector3.one * EffectTarget.Stats.BaseSize * 3;
    }
}