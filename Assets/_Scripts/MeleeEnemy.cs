using UnityEngine;

public class MeleeEnemy : EnemyBase
{
    private CharacterMovement _characterMovement;

    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float attackRange;

    public override void Initialize(int round)
    {
        base.Initialize(round);

        _characterMovement = GetComponent<CharacterMovement>();
        
        Transform player = GameObject.FindWithTag("Player").transform;
        EnemyMoveState enemyMoveStateState = new EnemyMoveState(_characterMovement, transform, player);

        if (TryGetComponent(out MeleeAttacker meleeAttacker))
        {
            meleeAttacker.Initialize(statsManager.Stats.AttackCooldown, attackRange, statsManager.Stats.Attack, playerLayer);

            EnemyMeleeAttackState meleeAttackState = new EnemyMeleeAttackState(meleeAttacker, transform, player,
                statsManager, statsManager.Stats.AttackCooldown);

            stateMachine.At(enemyMoveStateState, meleeAttackState, new FuncPredicate(() => Vector3.Distance(transform.position, player.position) <= attackRange + 0.5f));
            stateMachine.At(meleeAttackState, enemyMoveStateState, new FuncPredicate(() => Vector3.Distance(transform.position, player.position) > attackRange + 0.5f));
        }

        stateMachine.SetState(enemyMoveStateState);
    }
}