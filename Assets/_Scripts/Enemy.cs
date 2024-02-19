using System;
using System.Collections.Generic;
using ProjectUtils.Helpers;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private StatsManager _statsManager;
    private CharacterMovement _characterMovement;
    
    private StateMachine _stateMachine;
    
    [SerializeField] private float attackRange;

    private void Start()
    {
        _statsManager = GetComponent<StatsManager>();
        _characterMovement = GetComponent<CharacterMovement>();
        
        Transform player = GameObject.FindWithTag("Player").transform;
        
        _stateMachine = new StateMachine();
        EnemyMoveState enemyMoveStateState = new EnemyMoveState(_characterMovement, transform, player);

        if (TryGetComponent(out BulletSpawner bulletSpawner))
        {
            bulletSpawner.Initialize(new List<BulletModifierInfo>());
            
            EnemyRangedAttackState rangeAttackState = new EnemyRangedAttackState(bulletSpawner, transform, player, _statsManager, 2);
            _stateMachine.At(enemyMoveStateState, rangeAttackState, new FuncPredicate(() => Vector3.Distance(transform.position, player.position) < attackRange));
            _stateMachine.At(rangeAttackState, enemyMoveStateState, new FuncPredicate(() => Vector3.Distance(transform.position, player.position) > attackRange+1));
        }
        
        _stateMachine.SetState(enemyMoveStateState);
    }

    private void FixedUpdate()
    {
        _stateMachine.FixedUpdate();
    }

    private void Update()
    {
        _stateMachine.Update();
        

    }
}