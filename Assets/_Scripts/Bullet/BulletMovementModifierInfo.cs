using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Bullet Movement Modifier", fileName = "Bullet Movement Modifier")]
public class BulletMovementModifierInfo : BulletModifierInfo
{
    public float amplitude;
    public float frequency;
    
    public override BulletModifier GetModifier(IEffectTarget target)
    {
        return System.Activator.CreateInstance(type.GetClass(), target, maxLevel, remainsAfterHit, priority, amplitude, frequency) as BulletModifier;
    }
}