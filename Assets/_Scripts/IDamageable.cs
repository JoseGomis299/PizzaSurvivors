using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(Damage damage);
}

public struct Damage
{
    public float value;
    public Element element;
    
    public Vector3 KnockBack => _direction*_knockBackForce;
    
    private readonly float _knockBackForce;
    private readonly Vector3 _direction;
    public Damage(float value, Element element, float knockBackForce, Vector3 hitDirection)
    {
        this.value = value;
        this.element = element;
        _knockBackForce = knockBackForce;
        _direction = hitDirection.normalized;
    }
}