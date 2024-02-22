using System;
using UnityEngine;

[Serializable]
public class MeleeAttack
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
    
    public void Attack(Vector2 position, float angle, LayerMask layerMask)
    {
        Collider2D hit = Shape.GetFirstCollider(position, angle, layerMask);

        if (hit && hit.TryGetComponent(out IDamageable health))
        {
            health.TakeDamage(Damage);
        }
    }
    
    public void AttackAll(Vector2 position, Vector2 direction, LayerMask layerMask)
    {
        Collider2D[] hits = Shape.GetColliders(position, direction, layerMask);

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent(out IDamageable health))
            {
                health.TakeDamage(Damage);
            }
        }
    }
    
    public void AttackAll(Vector2 position, float angle, LayerMask layerMask)
    {
        Collider2D[] hits = Shape.GetColliders(position, angle, layerMask);

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent(out IDamageable health))
            {
                health.TakeDamage(Damage);
            }
        }
    }
}

