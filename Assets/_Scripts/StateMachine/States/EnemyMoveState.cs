using UnityEngine;

public class EnemyMoveState : BaseState{
    
    private EnemyBase _enemyBase;
    private Transform _transform;
    private Transform _target;
    
    public EnemyMoveState(EnemyBase enemyBase, Transform transform, Transform target)
    {
        _enemyBase = enemyBase;
        _transform = transform;
        _target = target;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        _enemyBase.Direction = (_target.position -_transform.position).normalized;
    }

    public override void Exit()
    {
        base.Exit();
        _enemyBase.Direction = Vector2.zero;
    }
}