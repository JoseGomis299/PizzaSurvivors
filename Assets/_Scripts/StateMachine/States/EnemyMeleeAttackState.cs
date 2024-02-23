using System;
using System.Collections;
using ProjectUtils.Helpers;
using UnityEngine;

public class EnemyMeleeAttackState : BaseState
{
    public event Action OnAttack;

    private MeleeAttacker _meleeAttacker;
    private Transform _attackPoint;
    private Transform _target;
    private StatsManager _statsManager;
    
    private float _timeBetweenAttacks;
    private float _lastAttackTime;
    
    private bool _isAttacking;
    private Coroutine _animationCoroutine;
    private Coroutine _currentCoroutine;

    public EnemyMeleeAttackState(MeleeAttacker meleeAttacker, Transform attackPoint, Transform target, StatsManager statsManager, float timeBetweenAttacks)
    {
        _meleeAttacker = meleeAttacker;
        _attackPoint = attackPoint;
        _statsManager = statsManager;
        _target = target;
        _timeBetweenAttacks = timeBetweenAttacks;
    }

    public override void Enter()
    {
        _lastAttackTime = Time.time;
        _isAttacking = false;
    }

    public override void Update()
    {
        if(Time.time - _lastAttackTime < _timeBetweenAttacks) return;
        
        if (!_isAttacking)
        {
            _animationCoroutine = _meleeAttacker.StartCoroutine(PlayAnimation());
            _isAttacking = true;
        }
            
        if (_meleeAttacker.MeleeAttack((_target.position - _meleeAttacker.transform.position).normalized))
        {
            _isAttacking = false;
            _lastAttackTime = Time.time;
            OnAttack?.Invoke();
        }
    }
    
    public override void Exit()
    {
        if (_animationCoroutine != null)
        {
            _meleeAttacker.StopCoroutine(_animationCoroutine);
            CoroutineController.Stop(_currentCoroutine);
            
            _meleeAttacker.transform.DoScale(Vector3.one, 0.1f, Transitions.TimeScales.Scaled);
            _animationCoroutine = null;
        }

        _isAttacking = false;
    }

    private IEnumerator PlayAnimation()
    {
        _meleeAttacker.ResetTimer();
        
        yield return _currentCoroutine = _attackPoint.transform.DoScale(Vector3.one * 0.8f, _statsManager.Stats.AttackCooldown/2f, Transitions.TimeScales.Scaled);
        yield return _currentCoroutine = _attackPoint.transform.DoScale(Vector3.one * 1.1f, _statsManager.Stats.AttackCooldown/2f * 0.9f, Transitions.TimeScales.Scaled);
        yield return _currentCoroutine = _attackPoint.transform.DoScale(Vector3.one, _statsManager.Stats.AttackCooldown/2f * 0.1f, Transitions.TimeScales.Scaled);
    }
}