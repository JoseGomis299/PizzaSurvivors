using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(Damage damage);
}

public struct Damage
{
    public float value;
    public Element element;
    
    public Vector3 GetKnockBack(Vector3 position) => (position-_origin).normalized*_knockBackForce;
    
    private readonly float _knockBackForce;
    private readonly Vector3 _origin;
    public Damage(float value, Element element, float knockBackForce, Vector3 origin)
    {
        this.value = value;
        this.element = element;
        _knockBackForce = knockBackForce;
        _origin = origin;
    }
}