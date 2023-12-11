using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Bullet Shoot Modifier", fileName = "Bullet Shoot Modifier")]
public class BulletShootModifierInfo : BulletModifierInfo
{
    public List<BulletModifierInfo> modifiers;
    public override BulletModifier GetModifier(IEffectTarget target)
    {
        return System.Activator.CreateInstance(type.GetClass(), target, maxLevel, remainsAfterHit, priority, modifiers) as BulletShootModifier;
    }
}