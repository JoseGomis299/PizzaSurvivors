using System.Collections.Generic;
using ProjectUtils.Helpers;
using ProjectUtils.ObjectPooling;
using Unity.Mathematics;
using UnityEngine;

public class FragmentationModifier : BulletHitModifier
{
    public FragmentationModifier(Bullet target, int maxStacks, int remainsAfterHit, int priority, GameObject onHitEffect) : base(target, maxStacks, remainsAfterHit, priority, onHitEffect)
    {
    }

    public override void Apply()
    {
        AddStack();
        if (EffectTarget.ID == 0 && CurrentStacks >= 2) RemainsAfterHit = 1;
    }

    public override void OnHit(StatsManager target, float damage, List<BulletHitModifier> hitModifiers, Element element)
    {
        if(target == null) return;
        
        var bullet = ObjectPool.Instance.InstantiateFromPool(EffectTarget.Spawner.bulletPrefab, EffectTarget.transform.position, quaternion.identity, true);
        bullet.GetComponent<Bullet>().Initialize(EffectTarget, EffectTarget.Direction.Rotate(15f), target);
        bullet = ObjectPool.Instance.InstantiateFromPool(EffectTarget.Spawner.bulletPrefab, EffectTarget.transform.position, quaternion.identity, true);
        bullet.GetComponent<Bullet>().Initialize(EffectTarget, EffectTarget.Direction.Rotate(-15f), target);
    }
}