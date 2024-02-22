using UnityEngine;

public class MeleeEnemy : EnemyBase
{
    [SerializeField] private LayerMask playerLayer;
    [field: SerializeField] public float AttackRange { get; private set; }

    public override void Initialize(int round)
    {
        base.Initialize(round);

        GetComponent<CharacterMovement>();
        
        Transform player = GameObject.FindWithTag("Player").transform;
        EnemyMoveState enemyMoveStateState = new EnemyMoveState(this, transform, player);

        if (TryGetComponent(out MeleeAttacker meleeAttacker))
        {
            meleeAttacker.Initialize(statsManager.Stats.AttackCooldown, AttackRange, statsManager.Stats.Attack, playerLayer);

            EnemyMeleeAttackState meleeAttackState = new EnemyMeleeAttackState(meleeAttacker, transform, player,
                statsManager, statsManager.Stats.AttackCooldown);

            stateMachine.At(enemyMoveStateState, meleeAttackState, new FuncPredicate(() => Vector3.Distance(transform.position, player.position) <= AttackRange + 0.5f));
            stateMachine.At(meleeAttackState, enemyMoveStateState, new FuncPredicate(() => Vector3.Distance(transform.position, player.position) > AttackRange + 0.5f));
        }

        stateMachine.SetState(enemyMoveStateState);
    }
}