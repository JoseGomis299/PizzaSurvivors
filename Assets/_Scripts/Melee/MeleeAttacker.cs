using System;
using UnityEngine;

public class MeleeAttacker : MonoBehaviour
{
    private float _lastAttackTime;
    private float _timeBetweenAttacks;
    private LayerMask _layer;
    private CombosContainer _combosContainer;
    
    public void Initialize(float timeBetweenAttacks, float attackRange, LayerMask layer)
    {
        _lastAttackTime = float.MinValue;
        _timeBetweenAttacks = timeBetweenAttacks;
        _layer = layer;
        
        _lastAttackTime = float.MinValue;

        _combosContainer = GetComponent<CombosContainer>();
        _combosContainer.Initialize(attackRange);
    }

    public bool MeleeAttack(Vector3 direction)
    {
        if (_timeBetweenAttacks > Time.time - _lastAttackTime) return false;
        _lastAttackTime = Time.time;

        _combosContainer.Attack(0, direction.normalized, _layer);
        return true;
    }

    public void ResetTimer()
    {
        _lastAttackTime = Time.time;
    }
}