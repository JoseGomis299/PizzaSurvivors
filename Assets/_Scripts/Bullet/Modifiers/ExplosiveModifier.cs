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
    
    public override void OnHit(StatsManager target, Damage damage, List<BulletHitModifier> hitModifiers, Element element)
    {
        var collisions = Physics2D.OverlapCircleAll(EffectTarget.transform.position, EffectTarget.Stats.Size * 1.5f);
        foreach (var col in collisions)
        {
            if((CurrentStacks >= 2 && col.gameObject == EffectTarget.Spawner.gameObject) || !col.TryGetComponent(out IDamageable damageable)) continue;
            damage = new Damage(damage.value * 0.5f, damage.element, 15, col.transform.position - EffectTarget.transform.position);
            damageable.TakeDamage(damage);
            
            if(target != null && col.gameObject == target.gameObject) continue;
            foreach (var modifier in hitModifiers)
            {
                modifier.OnHit(col.GetComponent<StatsManager>(), damage, hitModifiers, element);
            }
        }

        GameObject effect = ObjectPool.Instance.InstantiateFromPool(OnHitEffect, EffectTarget.transform.position, Quaternion.identity, true);
        effect.transform.localScale = Vector3.one * EffectTarget.Stats.Size * 3;
    }
}