using System;
using ProjectUtils.ObjectPooling;
using UnityEngine;

public class ExplosiveAttackState : BaseState
{
    public event Action OnAttack;
    private Damage _damage;
    private float _radius;
    private LayerMask _targetLayer;
    private Transform _transform;
    private GameObject _explosionEffect;
    
    
    public ExplosiveAttackState(Transform transform, Damage damage, float radius, LayerMask targetLayer, GameObject explosionEffect)
    {
        _transform = transform;
        _damage = damage;
        _radius = radius;
        _targetLayer = targetLayer;
        _explosionEffect = explosionEffect;
        _explosionEffect.transform.localScale = Vector3.one * (radius * 2);
    }
    public override void Enter()
    {
        ObjectPool.Instance.InstantiateFromPool(_explosionEffect, _transform.position, Quaternion.identity, true);
        Collider2D[] hits = Physics2D.OverlapCircleAll(_transform.transform.position, _radius, _targetLayer);
        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent(out IDamageable health))
            {
                health.TakeDamage(_damage);
            }
        }
        OnAttack?.Invoke();
        
        _transform.GetComponent<IKillable>().OnDeath();
    }
}