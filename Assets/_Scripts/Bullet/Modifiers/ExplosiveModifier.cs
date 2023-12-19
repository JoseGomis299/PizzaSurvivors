using System.Collections.Generic;
using ProjectUtils.ObjectPooling;
using UnityEngine;

public class ExplosiveModifier : BulletHitModifier
{
    public ExplosiveModifier(Bullet target, int maxStacks, int remainsAfterHit, int priority, GameObject onHiEffect) : base(target, maxStacks, remainsAfterHit, priority, onHiEffect)
    {
    }
    public override void Apply()
    {
        AddStack();
    }
    
    public override void OnHit(IEffectTarget target, float damage, List<BulletHitModifier> hitModifiers, Element element)
    {
        var collisions = Physics2D.OverlapCircleAll(EffectTarget.transform.position, EffectTarget.Stats.Size * 1.5f);
        foreach (var col in collisions)
        {
            if((CurrentStacks >= 2 && col.gameObject == EffectTarget.Spawner.gameObject) || !col.TryGetComponent(out IDamageable damageable)) continue;
            damageable.TakeDamage(damage * 0.5f, element);
            
            if(target != null && col.gameObject == ((MonoBehaviour)target).gameObject) continue;
            foreach (var modifier in hitModifiers)
            {
                modifier.OnHit(col.GetComponent<IEffectTarget>(), damage * 0.5f, hitModifiers, element);
            }
        }

        GameObject effect = ObjectPool.Instance.InstantiateFromPool(OnHitEffect, EffectTarget.transform.position, Quaternion.identity, true);
        effect.transform.localScale = Vector3.one * EffectTarget.Stats.Size * 3;
    }
}