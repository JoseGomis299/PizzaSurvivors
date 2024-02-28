using System;
using UnityEngine;

public class ExplosiveEnemy : EnemyBase
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float radius;
    [SerializeField] private Damage damage;
    private ExplosiveAttackState _explosiveState;
    [field: SerializeField] public float AttackRange { get; private set; }

    public override void Initialize(int round)
    {
        base.Initialize(round);

        damage.value = statsManager.Stats.Attack;
        EnemyMoveState enemyMoveStateState = new EnemyMoveState(this, transform, player);
        
        _explosiveState = new ExplosiveAttackState(transform, damage, radius, playerLayer, explosionEffect);

        stateMachine.At(enemyMoveStateState, _explosiveState, new FuncPredicate(() => Vector3.Distance(transform.position, player.position) <= AttackRange));
        _explosiveState.OnAttack += InvokeOnEnemyExplosive;

        stateMachine.SetState(enemyMoveStateState);
    }

    public override void OnDeath()
    {
        base.OnDeath();
        if(!_explosiveState.HasExploded) GetComponent<EnemyDropSystem>().DropItem();
        if(_explosiveState != null)
            _explosiveState.OnAttack -= InvokeOnEnemyExplosive;
    }
}

