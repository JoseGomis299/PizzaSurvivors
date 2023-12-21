using System.Collections;
using ProjectUtils.Helpers;
using UnityEngine;

public class EnemyRangedAttackState : BaseState
{
    private BulletSpawner _bulletSpawner;
    private Transform _shotPoint;
    private Transform _target;
    private StatsManager _statsManager;
    
    private bool _isAttacking;
    private Coroutine _animationCoroutine;
    private Coroutine _currentCoroutine;

    public EnemyRangedAttackState(BulletSpawner bulletSpawner,Transform shotPoint, Transform target, StatsManager statsManager)
    {
        _shotPoint = shotPoint;
        _statsManager = statsManager;
        _target = target;
        _bulletSpawner = bulletSpawner;
    }

    public override void Enter()
    {
        base.Enter();
        _bulletSpawner.ResetTimer();
    }

    public override void Update()
    {
        base.Update();

        if (!_isAttacking)
        {
            _animationCoroutine = _bulletSpawner.StartCoroutine(PlayAnimation());
            _isAttacking = true;
        }
            
        if (_bulletSpawner.SpawnBullet((_target.position - _shotPoint.position).normalized))
        {
            _isAttacking = false;
        }
    }
    
    public override void Exit()
    {
        base.Exit();
        _bulletSpawner.StopCoroutine(_animationCoroutine);
        CoroutineController.Stop(_currentCoroutine);
        
        _bulletSpawner.transform.DoScale(Vector3.one, 0.1f, Transitions.TimeScales.Scaled);
        _isAttacking = false;
    }

    private IEnumerator PlayAnimation()
    {
        yield return _currentCoroutine = _bulletSpawner.transform.DoScale(Vector3.one * 0.8f, _statsManager.Stats.AttackSpeed/2f, Transitions.TimeScales.Scaled);
        yield return _currentCoroutine = _bulletSpawner.transform.DoScale(Vector3.one * 1.1f, _statsManager.Stats.AttackSpeed/2f * 0.9f, Transitions.TimeScales.Scaled);
        yield return _currentCoroutine = _bulletSpawner.transform.DoScale(Vector3.one, _statsManager.Stats.AttackSpeed/2f * 0.1f, Transitions.TimeScales.Scaled);
    }
}