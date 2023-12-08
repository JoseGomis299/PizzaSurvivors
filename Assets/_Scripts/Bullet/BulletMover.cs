using System.Collections.Generic;
using UnityEngine;

public class BulletMover
{
    public float Speed { get; set; }
    private readonly float _speed;
    public Vector2 Direction { get; set; }
    private readonly Vector2 _direction;

    private readonly Rigidbody2D _rb;
    private readonly List<BulletMovementModifier> _modifiers;
    
    public BulletMover(Rigidbody2D rb, List<BulletMovementModifier> modifiers, float speed, Vector2 direction)
    {
        _rb = rb;
        _modifiers = modifiers;
        Speed = speed;
        Direction = direction;
        _speed = speed;
        _direction = direction;
    }

    public void Move()
    {
        Speed = _speed;
        Direction = _direction;
        foreach (var modifier in _modifiers)
            modifier.Apply();
        
        Direction.Normalize();
        _rb.velocity = Direction * Speed;
    }
}