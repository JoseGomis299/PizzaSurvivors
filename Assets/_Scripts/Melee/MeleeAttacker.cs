using System;
using UnityEngine;

public class MeleeAttacker : MonoBehaviour
{
    public event Action<Transform, Vector2, Vector2, MeleeAttack> OnAttack;

    private float _lastAttackTime;
    private float _timeBetweenAttacks;
    private float _attackRange;
    private float _dmg;
    private LayerMask _layer;
    private CombosContainer _combosContainer;
    
    public void Initialize(float timeBetweenAttacks, float attackRange, float dmg, LayerMask layer)
    {
        _lastAttackTime = float.MinValue;
        _timeBetweenAttacks = timeBetweenAttacks;
        _attackRange = attackRange;
        _dmg = dmg;
        _layer = layer;
        
        _lastAttackTime = float.MinValue;

        _combosContainer = GetComponent<CombosContainer>();
        _combosContainer.Initialize(attackRange);
    }

    public bool MeleeAttack(Transform target)
    {
        if (_timeBetweenAttacks > Time.time - _lastAttackTime) return false;
        
        Vector2 dir = target.position - transform.position;
        _combosContainer.Attack(0, dir.normalized, _layer);
        
        _lastAttackTime = Time.time;

        return true;
    }

    public void ResetTimer()
    {
        _lastAttackTime = Time.time;
    }
}