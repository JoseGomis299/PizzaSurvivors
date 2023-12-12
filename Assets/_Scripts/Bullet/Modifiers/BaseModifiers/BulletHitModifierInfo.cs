using UnityEngine;

[CreateAssetMenu(menuName = "Bullet Modifiers/Hit Modifier", fileName = "Bullet Hit Modifier")]
public class BulletHitModifierInfo : BulletModifierInfo
{
    public GameObject OnHitEffect;

    public enum HitModifierType
    {
        Explosive,
        Freezing
    }
    
    [Header("Type")]
    public HitModifierType type;
    
    public override BulletModifier GetModifier(IEffectTarget target)
    {
        switch (type)
        {
            case HitModifierType.Explosive:
                return new ExplosiveModifier(target, maxLevel, remainsAfterHit, priority, OnHitEffect);
            case HitModifierType.Freezing:
                return new FreezingModifier(target, maxLevel, remainsAfterHit, priority, OnHitEffect);
        }

        return null;
    }
}