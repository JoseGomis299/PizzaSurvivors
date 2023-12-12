using UnityEngine;

[CreateAssetMenu(menuName = "Bullet Modifiers/Movement Modifier", fileName = "Bullet Movement Modifier")]
public class BulletMovementModifierInfo : BulletModifierInfo
{
    public float amplitude;
    public float frequency;
    
    public enum MovementModifierType
    {
        Sinusoidal,
        Boomerang
    }
    
    [Header("Type")]
    public MovementModifierType type;
    
    public override BulletModifier GetModifier(IEffectTarget target)
    {
        switch (type)
        {
            case MovementModifierType.Sinusoidal:
                return new SineMovementModifier(target, maxLevel, remainsAfterHit, priority, amplitude, frequency);
            case MovementModifierType.Boomerang:
                return new BoomerangModifier(target, maxLevel, remainsAfterHit, priority, amplitude, frequency);
        }

        return null;
    }
}