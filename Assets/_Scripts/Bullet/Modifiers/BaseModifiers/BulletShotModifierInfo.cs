using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bullet Modifiers/Shot Modifier", fileName = "Bullet Shot Modifier")]
public class BulletShotModifierInfo : BulletModifierInfo
{
    public List<BulletModifierInfo> modifiers;
    
    public enum ShootModifierType
    {
        TripleShot,
        MultiDirectionalShot
    }
    
    [Header("Type")]
    public ShootModifierType type;
    
    public override BulletModifier GetModifier(IEffectTarget target)
    {
        return type switch
        {
            ShootModifierType.TripleShot => new TripleShotModifier(target, maxLevel, remainsAfterHit, priority, modifiers),
            ShootModifierType.MultiDirectionalShot => new MultiDirectionalShotModifier(target, maxLevel, remainsAfterHit, priority, modifiers),
            _ => null
        };
    }
}