using UnityEngine;

public class EnemyRangedAttackState : BaseState
{
    private BulletSpawner _bulletSpawner;
    private Transform _shotPoint;
    private Transform _target;
    
    public EnemyRangedAttackState(BulletSpawner bulletSpawner,Transform shotPoint, Transform target)
    {
        _shotPoint = shotPoint;
        _target = target;
        _bulletSpawner = bulletSpawner;
    }
    
    public override void Update()
    {
        base.Update();
        _bulletSpawner.SpawnBullet((_target.position -_shotPoint.position).normalized);
    }
}