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
        switch (type)
        {
            case ShootModifierType.TripleShot:
                return new TripleShotModifier(target, maxLevel, remainsAfterHit, priority, modifiers);
            case ShootModifierType.MultiDirectionalShot:
                return new MultiDirectionalShotModifier(target, maxLevel, remainsAfterHit, priority, modifiers);
        }
        return null;
    }
}