using UnityEngine;

public class EnemyAudioPlayer : CharacterAudioPlayer
{
    [SerializeField] private AudioClip spawnSound;
    private EnemyBase _enemyBase;
    
    protected override void Awake()
    {
        base.Awake();
        _enemyBase = GetComponent<EnemyBase>();
    }

    protected override void SubscribeToEvents()
    {
        _enemyBase.OnEnemyDeath += HandleDeathSound;
        _enemyBase.OnEnemyHit += HandleHitSound;
        _enemyBase.OnEnemyRangedAttack += HandleShootSound;
        _enemyBase.OnEnemyMeleeAttack += HandleMeleeAttackSound;
        _enemyBase.OnEnemySpawn += HandleSpawnSound;
    }

    protected override void UnsubscribeToEvents()
    {
        _enemyBase.OnEnemyDeath -= HandleDeathSound;
        _enemyBase.OnEnemyHit -= HandleHitSound;
        _enemyBase.OnEnemyRangedAttack -= HandleShootSound;
        _enemyBase.OnEnemyMeleeAttack -= HandleMeleeAttackSound;
        _enemyBase.OnEnemySpawn -= HandleSpawnSound;
    }
    
    private void HandleSpawnSound()
    {
        if(spawnSound == null) return;
        PlaySoundAtThisPosition(spawnSound);
    }
}