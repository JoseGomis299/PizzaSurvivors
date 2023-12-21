using ProjectUtils.Helpers;
using UnityEngine;

public interface IShape
{
    public Collider2D GetFirstCollider(Vector2 position, Vector2 direction, LayerMask layerMask);
    public Collider2D[] GetColliders(Vector2 position, Vector2 direction, LayerMask layerMask);

#if UNITY_EDITOR
    public void DrawGizmos(Vector2 position, Vector2 direction, Color color);
#endif
}

public readonly struct Rectangle : IShape
{
    private readonly Vector2 _size;
    
    public Rectangle(Vector2 size)
    {
        _size = size;
    }

    public Collider2D GetFirstCollider(Vector2 position, Vector2 direction, LayerMask layerMask)
    {
        return Physics2D.OverlapBox(position, _size, Vector2.SignedAngle(Vector2.right, direction), layerMask);
    }

    public Collider2D[] GetColliders(Vector2 position, Vector2 direction, LayerMask layerMask)
    {
        return Physics2D.OverlapBoxAll(position, _size, Vector2.SignedAngle(Vector2.right, direction), layerMask);
    }
    
#if UNITY_EDITOR
    public void DrawGizmos(Vector2 position, Vector2 direction, Color color)
    {
        var angle = Vector2.SignedAngle(Vector2.right, direction);
        
        Gizmos.color = color;
        Helpers.DrawWiredRectangle(position, _size, angle, color);
    }
#endif
}

public readonly struct Circle : IShape
{
    private readonly float _radius;
    
    public Circle(float radius)
    {
        _radius = radius;
    }

    public Collider2D GetFirstCollider(Vector2 position, Vector2 direction, LayerMask layerMask)
    {
        return Physics2D.OverlapCircle(position, _radius, layerMask);
    }

    public Collider2D[] GetColliders(Vector2 position, Vector2 direction, LayerMask layerMask)
    {
        return Physics2D.OverlapCircleAll(position, _radius, layerMask);
    }
    
#if UNITY_EDITOR
    public void DrawGizmos(Vector2 position, Vector2 direction, Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(position, _radius);
    }
#endif
}

public readonly struct Capsule : IShape
{
    private readonly Vector2 _size;
    private readonly CapsuleDirection2D _orientation;
    public Capsule(Vector2 size, CapsuleDirection2D orientation)
    {
        _size = size;
        _orientation = orientation;
    }

    public Collider2D GetFirstCollider(Vector2 position, Vector2 direction, LayerMask layerMask)
    {
        return Physics2D.OverlapCapsule(position, _size, _orientation, Vector2.SignedAngle(Vector2.right, direction), layerMask);
    }

    public Collider2D[] GetColliders(Vector2 position, Vector2 direction, LayerMask layerMask)
    {
        return Physics2D.OverlapCapsuleAll(position, _size, _orientation, Vector2.SignedAngle(Vector2.right, direction), layerMask);
    }
    
#if UNITY_EDITOR
    public void DrawGizmos(Vector2 position, Vector2 direction, Color color)
    {
        Gizmos.color = color;
        Helpers.DrawWiredCapsule(position, _size, Vector2.SignedAngle(Vector2.right, direction), _orientation, color);
       
    }
#endif
}

public readonly struct Line : IShape
{
    private readonly float _length;

    public Line(float length)
    {
        _length = length;
    }

    public Collider2D GetFirstCollider(Vector2 position, Vector2 direction, LayerMask layerMask)
    {
        return Physics2D.Raycast(position, direction, _length, layerMask).collider;
    }

    public Collider2D[] GetColliders(Vector2 position, Vector2 direction, LayerMask layerMask)
    {
        var hits = Physics2D.RaycastAll(position, direction, _length, layerMask);
        var colliders = new Collider2D[hits.Length];

        for (var i = 0; i < hits.Length; i++)
            colliders[i] = hits[i].collider;

        return colliders;
    }

#if UNITY_EDITOR
    public void DrawGizmos(Vector2 position, Vector2 direction, Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawLine(position, position + direction * _length);
    }
#endif
}