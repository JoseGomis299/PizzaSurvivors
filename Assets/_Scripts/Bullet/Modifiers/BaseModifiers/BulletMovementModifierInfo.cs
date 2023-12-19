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
    
    public BulletMovementModifier GetBulletModification(Bullet target)
    {
        return type switch
        {
            MovementModifierType.Sinusoidal => new SineMovementModifier(target, maxLevel, priority, amplitude, frequency),
            MovementModifierType.Boomerang => new BoomerangModifier(target, maxLevel, priority, amplitude, frequency),
            _ => null
        };
    }
}