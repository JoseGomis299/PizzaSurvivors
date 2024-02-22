using System.Collections.Generic;
using ProjectUtils.Helpers;
using UnityEngine;

public class RangedEnemy : EnemyBase
{
    private CharacterMovement _characterMovement;

    [SerializeField] private float attackRange;

    public override void Initialize(int round)
    {
        base.Initialize(round);

        _characterMovement = GetComponent<CharacterMovement>();
        
        Transform player = GameObject.FindWithTag("Player").transform;
        EnemyMoveState enemyMoveStateState = new EnemyMoveState(_characterMovement, transform, player);

        if (TryGetComponent(out BulletSpawner bulletSpawner))
        {
            bulletSpawner.Initialize(new List<BulletModifierInfo>());
            
            EnemyRangedAttackState rangeAttackState = new EnemyRangedAttackState(bulletSpawner, transform, player, statsManager, statsManager.Stats.AttackCooldown);
            stateMachine.At(enemyMoveStateState, rangeAttackState, new FuncPredicate(() => Vector3.Distance(transform.position, player.position) < attackRange));
            stateMachine.At(rangeAttackState, enemyMoveStateState, new FuncPredicate(() => Vector3.Distance(transform.position, player.position) > attackRange+1));
        }
        
        stateMachine.SetState(enemyMoveStateState);
    }
}