using UnityEngine;

public class EnemyMoveState : BaseState{
    
    private CharacterMovement _characterMovement;
    private Transform _transform;
    private Transform _target;
    
    public EnemyMoveState(CharacterMovement characterMovement, Transform transform, Transform target)
    {
        _characterMovement = characterMovement;
        _transform = transform;
        _target = target;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        _characterMovement.UpdateMovement((_target.position -_transform.position).normalized, Time.fixedDeltaTime);
    }
}