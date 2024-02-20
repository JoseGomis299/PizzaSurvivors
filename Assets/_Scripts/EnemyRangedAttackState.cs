using System;
using System.Collections;
using ProjectUtils.Helpers;
using UnityEngine;

public class EnemyRangedAttackState : BaseState
{
    public static event Action OnAttack;
    
    private BulletSpawner _bulletSpawner;
    private Transform _shotPoint;
    private Transform _target;
    private StatsManager _statsManager;
    
    private float _timeBetweenAttacks;
    private float _lastAttackTime;
    
    private bool _isAttacking;
    private Coroutine _animationCoroutine;
    private Coroutine _currentCoroutine;

    public EnemyRangedAttackState(BulletSpawner bulletSpawner,Transform shotPoint, Transform target, StatsManager statsManager, float timeBetweenAttacks){
        _shotPoint = shotPoint;
        _statsManager = statsManager;
        _target = target;
        _bulletSpawner = bulletSpawner;
        _timeBetweenAttacks = timeBetweenAttacks;
        
        _bulletSpawner.transform.localScale = Vector3.one;
    }

    public override void Enter()
    {
        base.Enter();
        _lastAttackTime = Time.time;
        _bulletSpawner.transform.localScale = Vector3.one;
    }

    public override void Update()
    {
        base.Update();
        if(Time.time - _lastAttackTime < _timeBetweenAttacks) return;
        
        if (!_isAttacking)
        {
            _animationCoroutine = _bulletSpawner.StartCoroutine(PlayAnimation());
            _isAttacking = true;
        }
            
        if (_bulletSpawner.SpawnBullet((_target.position - _shotPoint.position).normalized))
        {
            _isAttacking = false;
            _lastAttackTime = Time.time;
            OnAttack?.Invoke();
        }
    }
    
    public override void Exit()
    {
        base.Exit();
        if (_animationCoroutine != null)
        {
            _bulletSpawner.StopCoroutine(_animationCoroutine);
            CoroutineController.Stop(_currentCoroutine);
            
            _bulletSpawner.transform.DoScale(Vector3.one, 0.1f, Transitions.TimeScales.Scaled);
            _animationCoroutine = null;
        }

        _isAttacking = false;
    }

    private IEnumerator PlayAnimation()
    {
        _bulletSpawner.ResetTimer();

        yield return _currentCoroutine = _bulletSpawner.transform.DoScale(Vector3.one * 0.8f, _statsManager.Stats.AttackSpeed/2f, Transitions.TimeScales.Scaled);
        yield return _currentCoroutine = _bulletSpawner.transform.DoScale(Vector3.one * 1.1f, _statsManager.Stats.AttackSpeed/2f * 0.9f, Transitions.TimeScales.Scaled);
        yield return _currentCoroutine = _bulletSpawner.transform.DoScale(Vector3.one, _statsManager.Stats.AttackSpeed/2f * 0.1f, Transitions.TimeScales.Scaled);
    }
}