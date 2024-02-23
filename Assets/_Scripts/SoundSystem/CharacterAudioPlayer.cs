using UnityEngine;

public abstract class CharacterAudioPlayer : BaseAudioPlayer
{
    [SerializeField] private float walkSoundCooldown = 0.5f;
    [SerializeField] protected AudioClip walkSound;
    [SerializeField] protected AudioClip shootSound;
    [SerializeField] protected AudioClip meleeAttackSound;
    [SerializeField] protected AudioClip hitSound;
    [SerializeField] protected AudioClip deathSound;
    
    private float _lastWalkSoundTime;

    protected override void Awake()
    {
        base.Awake();
        _lastWalkSoundTime = float.MinValue;
    }

    protected void HandleWalkSound()
    {
        if(walkSound == null || walkSoundCooldown > Time.time - _lastWalkSoundTime) return;
        _lastWalkSoundTime = Time.time;
        
        PlaySoundAtThisPosition(walkSound);
    }
    
    protected void HandlePlayerShootSound()
    {
        if(shootSound == null) return;
        PlaySoundAtThisPosition(shootSound);
    }
    
    protected void HandlePlayerHitSound()
    {
        if(hitSound == null) return;
        PlaySoundAtThisPosition(hitSound);
    }
    
    protected void HandlePlayerDeathSound()
    {
        if(deathSound == null) return;
        PlaySoundAtThisPosition(deathSound);
    }
    
    protected void HandlePlayerMeleeAttackSound()
    {
        if(meleeAttackSound == null) return;
        PlaySoundAtThisPosition(meleeAttackSound);
    }
}