using System.Collections.Generic;
using ProjectUtils.Helpers;
using UnityEngine;

public class RangedEnemy : EnemyBase
{
    [SerializeField] private float attackRange;
    private EnemyRangedAttackState _rangedAttackState;
    
    public override void Initialize(int round)
    {
        base.Initialize(round);

        EnemyMoveState enemyMoveStateState = new EnemyMoveState(this, transform, player);

        if (TryGetComponent(out BulletSpawner bulletSpawner))
        {
            bulletSpawner.Initialize(new List<BulletModifierInfo>());
            
            _rangedAttackState = new EnemyRangedAttackState(bulletSpawner, transform, player, statsManager, statsManager.Stats.AttackCooldown);
            _rangedAttackState.OnAttack += InvokeOnEnemyRangedAttack;
            
            stateMachine.At(enemyMoveStateState, _rangedAttackState, new FuncPredicate(() => Vector3.Distance(transform.position, player.position) < attackRange));
            stateMachine.At(_rangedAttackState, enemyMoveStateState, new FuncPredicate(() => Vector3.Distance(transform.position, player.position) > attackRange+1));
        }
        
        stateMachine.SetState(enemyMoveStateState);
    }
    
    public override void OnDeath()
    {
        base.OnDeath();
        GetComponent<EnemyDropSystem>().DropItem();
        if(_rangedAttackState != null)
            _rangedAttackState.OnAttack -= InvokeOnEnemyRangedAttack;
    }
}