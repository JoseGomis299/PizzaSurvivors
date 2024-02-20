using System;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IKillable
{
    public static event Action OnEnemyDeath;
    public static event Action OnEnemyHit;
    
    protected StateMachine stateMachine;
    protected StatsManager statsManager;

    public virtual void Initialize()
    {
        GetComponent<HealthComponent>().OnHealthUpdate += InvokeOnEnemyHit;
        statsManager = GetComponent<StatsManager>();
        stateMachine = new StateMachine();
    }

    public void OnDeath()
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
    
    protected void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    protected void Update()
    {
        stateMachine.Update();
    }
}