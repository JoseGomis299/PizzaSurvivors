using System;
using System.Collections;
using ProjectUtils.Helpers;
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
    
    public bool HasExploded { get; private set; }
    
    
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
        CoroutineController.Start(Explode());
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(0.5f);
        yield return _transform.GetComponent<SpriteRenderer>().DoBlink(1, 5, new Color(0, 0, 0, 0));
        
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
        HasExploded = true;
        _transform.GetComponent<IKillable>().OnDeath();
    }
}