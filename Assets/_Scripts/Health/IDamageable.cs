using System;
using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(Damage damage);
}

[Serializable]
public struct Damage
{
    public float value;
    public Element element;

    public Vector3 GetKnockBack(Vector3 position)
    {
        if(_direction == default) return (position-_origin).normalized*_knockBackForce;
        return _direction*_knockBackForce;
    } 
    
    [SerializeField] private float _knockBackForce;
    private readonly Vector3 _origin;
    private readonly Vector3 _direction;

    public Damage(float value, Element element, float knockBackForce, Vector3 origin, Vector3 direction = default)
    {
        this.value = value;
        this.element = element;
        _knockBackForce = knockBackForce;
        _origin = origin;
        _direction = direction;
    }
}