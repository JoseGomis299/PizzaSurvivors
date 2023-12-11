using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Bullet Hit Modifier", fileName = "Bullet HitModifier")]
public class BulletHitModifierInfo : BulletModifierInfo
{
    public GameObject OnHitEffect;

    public override BulletModifier GetModifier(IEffectTarget target)
    {
        return System.Activator.CreateInstance(type.GetClass(), target, maxLevel, remainsAfterHit, priority, OnHitEffect) as BulletHitModifier;
    }
}