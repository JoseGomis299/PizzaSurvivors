using UnityEngine;

public class MeleeEnemy : EnemyBase
{
    [SerializeField] private LayerMask playerLayer;
    private EnemyMeleeAttackState _meleeAttackState;
    [field: SerializeField] public float AttackRange { get; private set; }

    public override void Initialize(int round)
    {
        base.Initialize(round);

        EnemyMoveState enemyMoveStateState = new EnemyMoveState(this, transform, player);

        if (TryGetComponent(out MeleeAttacker meleeAttacker))
        {
            meleeAttacker.Initialize(statsManager.Stats.AttackCooldown, AttackRange, playerLayer);

            _meleeAttackState = new EnemyMeleeAttackState(meleeAttacker, transform, player,
                statsManager, statsManager.Stats.AttackCooldown);

            stateMachine.At(enemyMoveStateState, _meleeAttackState, new FuncPredicate(() => Vector3.Distance(transform.position, player.position) <= AttackRange));
            stateMachine.At(_meleeAttackState, enemyMoveStateState, new FuncPredicate(() => Vector3.Distance(transform.position, player.position) > AttackRange));
            
            _meleeAttackState.OnAttack += InvokeOnEnemyExplosive;
        }

        stateMachine.SetState(enemyMoveStateState);
    }

    public override void OnDeath()
    {
        base.OnDeath();
        GetComponent<EnemyDropSystem>().DropItem();
        if(_meleeAttackState != null)
            _meleeAttackState.OnAttack -= InvokeOnEnemyExplosive;
    }
}