using System.Collections.Generic;
using ProjectUtils.Helpers;
using UnityEngine;

public class Enemy : EnemyBase
{
    private CharacterMovement _characterMovement;
    
    [SerializeField] private float attackRange;

    public override void Initialize()
    {
        base.Initialize();

        _characterMovement = GetComponent<CharacterMovement>();
        
        Transform player = GameObject.FindWithTag("Player").transform;
        EnemyMoveState enemyMoveStateState = new EnemyMoveState(_characterMovement, transform, player);

        if (TryGetComponent(out BulletSpawner bulletSpawner))
        {
            bulletSpawner.Initialize(new List<BulletModifierInfo>());
            
            EnemyRangedAttackState rangeAttackState = new EnemyRangedAttackState(bulletSpawner, transform, player, statsManager, 2);
            stateMachine.At(enemyMoveStateState, rangeAttackState, new FuncPredicate(() => Vector3.Distance(transform.position, player.position) < attackRange));
            stateMachine.At(rangeAttackState, enemyMoveStateState, new FuncPredicate(() => Vector3.Distance(transform.position, player.position) > attackRange+1));
        }
        
        stateMachine.SetState(enemyMoveStateState);
    }
}