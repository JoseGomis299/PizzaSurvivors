using UnityEngine;

[CreateAssetMenu(menuName = "Bullet Modifiers/Hit Modifier", fileName = "Bullet Hit Modifier")]
public class BulletHitModifierInfo : BulletModifierInfo
{
    public GameObject OnHitEffect;

    public enum HitModifierType
    {
        Explosive,
        Freezing,
        Fragmentation
    }
    
    [Header("Type")]
    public HitModifierType type;
    
    public override BulletModifier GetModifier(IEffectTarget target)
    {
        return type switch
        {
            HitModifierType.Explosive => new ExplosiveModifier(target, maxLevel, remainsAfterHit, priority, OnHitEffect),
            HitModifierType.Freezing => new FreezingModifier(target, maxLevel, remainsAfterHit, priority, OnHitEffect), 
            HitModifierType.Fragmentation => new FragmentationModifier(target, maxLevel, remainsAfterHit, priority, OnHitEffect),
            _ => null
        };
    }
    
    
}

