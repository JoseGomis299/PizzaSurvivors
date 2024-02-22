using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(Damage damage);
}

public struct Damage
{
    public float value;
    public Element element;

    public Vector3 GetKnockBack(Vector3 position)
    {
        if(_direction == Vector3.zero) return (position-_origin).normalized*_knockBackForce;
        return _direction*_knockBackForce;
    } 
    
    private readonly float _knockBackForce;
    private readonly Vector3 _origin;
    private readonly Vector3 _direction;

    public Damage(float value, Element element, float knockBackForce, Vector3 vector, bool vectorIsOrigin = true)
    {
        this.value = value;
        this.element = element;
        _knockBackForce = knockBackForce;
        if (vectorIsOrigin)
        {
            _origin = vector;
            _direction = Vector3.zero;
        }
        else
        {
            _origin = vector;
            _direction = vector.normalized;
        }
    }
}