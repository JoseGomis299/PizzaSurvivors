using System;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IKillable
{
    public event Action OnEnemyDeath;
    public event Action OnEnemyHit;
    public event Action OnEnemyRangedAttack;
    public event Action OnEnemyMeleeAttack;
    public event Action OnEnemySpawn;
    
    protected StateMachine stateMachine;
    protected StatsManager statsManager;
    protected CharacterMovement characterMovement;
    protected Transform player;
    
    public Vector2 Direction {get; set;}

    public virtual void Initialize(int round)
    {
        GetComponent<HealthComponent>().OnHealthUpdate += InvokeOnEnemyHit;
        statsManager = GetComponent<StatsManager>();
        characterMovement = GetComponent<CharacterMovement>();
        player = GameObject.FindWithTag("Player").transform;
        stateMachine = new StateMachine();
        statsManager.ResetStats();
        statsManager.Stats.MultiplyBasicStats(1 + (round-1)*0.05f);
        OnEnemySpawn?.Invoke();
    }

    public virtual void OnDeath()
    {
        GetComponent<EnemyDropSystem>().DropItem();
        transform.parent = null;
        gameObject.SetActive(false);
        
        GetComponent<HealthComponent>().OnHealthUpdate -= InvokeOnEnemyHit;
        OnEnemyDeath?.Invoke();
    }
    
    private void InvokeOnEnemyHit(float _)
    {
        OnEnemyHit?.Invoke();
    }
    
    protected void InvokeOnEnemyMeleeAttack()
    {
        OnEnemyMeleeAttack?.Invoke();
    }
    
    protected void InvokeOnEnemyRangedAttack()
    {
        OnEnemyRangedAttack?.Invoke();
    }
    
    protected virtual void FixedUpdate()
    {
        stateMachine.FixedUpdate();
        characterMovement.UpdateMovement(Direction, Time.fixedDeltaTime);
        characterMovement.UpdateFacingDirection((player.position - transform.position).normalized);
    }

    protected virtual void Update()
    {
        stateMachine.Update();
    }
}