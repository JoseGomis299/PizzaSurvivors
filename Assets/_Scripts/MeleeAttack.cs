using UnityEngine;

public readonly struct MeleeAttack
{
    public readonly Damage Damage;
    public readonly IShape Shape;
    
    public MeleeAttack(Damage damage, IShape shape)
    {
        Damage = damage;
        Shape = shape;
    }

    public void Attack(Vector2 position, Vector2 direction, LayerMask layerMask)
    {
        Collider2D hit = Shape.GetFirstCollider(position, direction, layerMask);

        if (hit && hit.TryGetComponent(out IDamageable health))
        {
            health.TakeDamage(Damage);
        }
    }
}