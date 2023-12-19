using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bullet Modifiers/Shot Modifier", fileName = "Bullet Shot Modifier")]
public class ShotModifierInfo : BulletModifierInfo
{
    public List<BulletModifierInfo> modifiers;
    
    public enum ShootModifierType
    {
        TripleShot,
        MultiDirectionalShot
    }
    
    [Header("Type")]
    public ShootModifierType type;
    
    public BulletShotModifier GetBulletShotModification(BulletSpawner target)
    {
        return type switch
        {
            ShootModifierType.TripleShot => new TripleShotModifier(target, maxLevel, priority, modifiers),
            ShootModifierType.MultiDirectionalShot => new MultiDirectionalShotModifier(target, maxLevel, priority, modifiers),
            _ => null
        };
    }
}