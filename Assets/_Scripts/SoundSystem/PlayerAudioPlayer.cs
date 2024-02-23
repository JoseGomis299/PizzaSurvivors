using UnityEngine;

public class PlayerAudioPlayer : CharacterAudioPlayer
{
    [SerializeField] private AudioClip rollSound;
    [SerializeField] private AudioClip collectSound;
    [SerializeField] private AudioClip interactSound;
    protected override void SubscribeToEvents()
    {
        PlayerController.OnPlayerDeath += HandlePlayerDeathSound;
        PlayerController.OnPlayerHit += HandlePlayerHitSound;
        PlayerController.OnPlayerShoot += HandlePlayerShootSound;
        PlayerController.OnPlayerMoved += HandleWalkSound;
        PlayerController.OnPlayerRolled += HandleRollSound;
        ItemCollector.OnItemCollected += HandleCollectSound;
        Interacter.OnInteract += HandleInteractSound;
    }

    protected override void UnsubscribeToEvents()
    {
        PlayerController.OnPlayerDeath -= HandlePlayerDeathSound;
        PlayerController.OnPlayerHit -= HandlePlayerHitSound;
        PlayerController.OnPlayerShoot -= HandlePlayerShootSound;
        PlayerController.OnPlayerMoved -= HandleWalkSound;
        PlayerController.OnPlayerRolled -= HandleRollSound;
        ItemCollector.OnItemCollected -= HandleCollectSound;
        Interacter.OnInteract -= HandleInteractSound;
    }
    
    private void HandleRollSound()
    {
        if(rollSound == null) return;
        PlaySoundAtThisPosition(rollSound);
    }
    
    private void HandleInteractSound()
    {
        if(interactSound == null) return;
        PlaySoundAtThisPosition(interactSound);
    }
    
    private void HandleCollectSound()
    {
        if(collectSound == null) return;
        PlaySoundAtThisPosition(collectSound);
    }
}