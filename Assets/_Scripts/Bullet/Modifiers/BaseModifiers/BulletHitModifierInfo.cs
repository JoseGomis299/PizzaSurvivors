using UnityEngine;

[CreateAssetMenu(menuName = "Bullet Modifiers/Hit Modifier", fileName = "Bullet Hit Modifier")]
public class BulletHitModifierInfo : BulletModifierInfo
{
    public GameObject onHitEffect;
    public int remainsAfterHit;

    public enum HitModifierType : int
    {
        Explosive,
        Freezing,
        Fragmentation,
        Burning
    }
    
    [Header("Type")]
    public HitModifierType type;
    
    public BulletHitModifier GetBulletModification(Bullet target)
    {
        return type switch
        {
            HitModifierType.Explosive => new ExplosiveModifier(target, maxLevel, remainsAfterHit, priority, onHitEffect),
            HitModifierType.Freezing => new FreezingModifier(target, maxLevel, remainsAfterHit, priority, onHitEffect), 
            HitModifierType.Fragmentation => new FragmentationModifier(target, maxLevel, remainsAfterHit, priority, onHitEffect),
            HitModifierType.Burning => new BurningModifier(target, maxLevel, remainsAfterHit, priority, onHitEffect),
            _ => null
        };
    }
    
    
}

