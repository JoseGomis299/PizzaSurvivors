using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AudioPlayer : MonoBehaviour
{
    [Header("Sound Effects")]
    [SerializeField] private AudioClip walkSound;
    [SerializeField] private AudioClip addIngredientSound;
    [SerializeField] private AudioClip removeIngredientSound;
    [SerializeField] private AudioClip collectSound;
    [SerializeField] private AudioClip spawnSound;
    [SerializeField] private AudioClip enemyShootSound;
    [SerializeField] private AudioClip enemyMeleeAttackSound;
    [SerializeField] private AudioClip enemyHitSound;
    [SerializeField] private AudioClip enemyDeathSound;
    [SerializeField] private AudioClip playerShootSound;
    [SerializeField] private AudioClip playerHitSound;
    [SerializeField] private AudioClip playerDeathSound;
    [SerializeField] private AudioClip loseSound;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private AudioClip interactSound;
    
    [Header("Music")]
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip gameMusic;
    
    private bool _wasPlayingMusic;
    
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        SubscribeToEvents();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UnsubscribeToEvents();
        SubscribeToEvents();

        if (scene.buildIndex == 0)
        {
            AudioManager.Instance.PlayMusic(mainMenuMusic);
        }else if (scene.buildIndex == 1)
        {
            AudioManager.Instance.PlayMusic(gameMusic);
        }
    }

    private void OnDestroy()
    {
        UnsubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        SpawningSystem.OnEnemySpawned += HandleSpawnSound;
        PlayerController.OnPlayerDeath += HandlePlayerDeathSound;
        PlayerController.OnPlayerHit += HandlePlayerHitSound;
        PlayerController.OnPlayerShoot += HandlePlayerShootSound;
        PlayerController.OnPlayerMoved += HandleWalkSound;
        PlayerController.OnPlayerRolled += HandleWalkSound;
        EnemyBase.OnEnemyDeath += HandleEnemyDeathSound;
        EnemyBase.OnEnemyHit += HandleEnemyHitSound;
        EnemyRangedAttackState.OnAttack += HandleEnemyShootSound;
        ItemCollector.OnItemCollected += HandleCollectSound;
        Pizza.OnIngredientPlaced += HandleAddIngredientSound;
        Pizza.OnIngredientRemoved += HandleRemoveIngredientSound;
        Interacter.OnInteract += HandleInteractSound;
        EnemyMeleeAttackState.OnAttack += HandleEnemyMeleeAttackSound;

        foreach (var button in FindObjectsOfType<Button>())
        {
            button.onClick.AddListener(HandleButtonClickSound);
        }
        
        foreach (var toggle in FindObjectsOfType<Toggle>())
        {
            toggle.onValueChanged.AddListener(HandleToggleChangedSound);
        }
    }

    private void UnsubscribeToEvents()
    {
        SpawningSystem.OnEnemySpawned -= HandleSpawnSound;
        PlayerController.OnPlayerDeath -= HandlePlayerDeathSound;
        PlayerController.OnPlayerHit -= HandlePlayerHitSound;
        PlayerController.OnPlayerShoot -= HandlePlayerShootSound;
        PlayerController.OnPlayerMoved -= HandleWalkSound;
        PlayerController.OnPlayerRolled -= HandleWalkSound;
        EnemyBase.OnEnemyDeath -= HandleEnemyDeathSound;
        EnemyBase.OnEnemyHit -= HandleEnemyHitSound;
        EnemyRangedAttackState.OnAttack -= HandleEnemyShootSound;
        ItemCollector.OnItemCollected -= HandleCollectSound;
        Pizza.OnIngredientPlaced -= HandleAddIngredientSound;
        Pizza.OnIngredientRemoved -= HandleRemoveIngredientSound;
        Interacter.OnInteract -= HandleInteractSound;
        EnemyMeleeAttackState.OnAttack -= HandleEnemyMeleeAttackSound;

        foreach (var button in FindObjectsOfType<Button>())
        {
            button.onClick.RemoveListener(HandleButtonClickSound);
        }
        
        foreach (var toggle in FindObjectsOfType<Toggle>())
        {
            toggle.onValueChanged.RemoveListener(HandleToggleChangedSound);
        }
    }
    
    private void HandleEnemyMeleeAttackSound()
    {
        if(enemyMeleeAttackSound == null) return;
        AudioManager.Instance.PlaySound(enemyMeleeAttackSound);
    }
    
    private void HandleInteractSound()
    {
        if(interactSound == null) return;
        AudioManager.Instance.PlaySound(interactSound);
    }
    
    private void HandleSpawnSound()
    {
        if(spawnSound == null) return;
        AudioManager.Instance.PlaySound(spawnSound);
    }
    
    private void HandleCollectSound()
    {
        if(collectSound == null) return;
        AudioManager.Instance.PlaySound(collectSound);
    }
    
    private void HandleAddIngredientSound()
    {
        if(addIngredientSound == null) return;
        AudioManager.Instance.PlaySound(addIngredientSound);
    }
    
    private void HandleRemoveIngredientSound()
    {
        if(removeIngredientSound == null) return;
        AudioManager.Instance.PlaySound(removeIngredientSound);
    }
    
    private void HandleWalkSound()
    {
        if(walkSound == null) return;
        AudioManager.Instance.PlaySound(walkSound);
    }
    
    private void HandleEnemyShootSound()
    {
        if(enemyShootSound == null) return;
        AudioManager.Instance.PlaySound(enemyShootSound);
    }
    
    private void HandleEnemyHitSound()
    {
        if(enemyHitSound == null) return;
        AudioManager.Instance.PlaySound(enemyHitSound);
    }
    
    private void HandleEnemyDeathSound()
    {
        if(enemyDeathSound == null) return;
        AudioManager.Instance.PlaySound(enemyDeathSound);
    }
    
    private void HandlePlayerShootSound()
    {
        if(playerShootSound == null) return;
        AudioManager.Instance.PlaySound(playerShootSound);
    }
    
    private void HandlePlayerHitSound()
    {
        if(playerHitSound == null) return;
        AudioManager.Instance.PlaySound(playerHitSound);
    }
    
    private void HandlePlayerDeathSound()
    {
        if(playerDeathSound == null) return;
        AudioManager.Instance.PlaySound(playerDeathSound);
    }
    
    private void HandleWinSound()
    {
        if (winSound != null)
        {
            AudioManager.Instance.PlaySound(winSound);
            if(AudioManager.Instance.IsMusicPlaying())
            {
                _wasPlayingMusic = true;
                AudioManager.Instance.StopMusic();
            } else _wasPlayingMusic = false;
        }
    }
    
    private void HandleLoseSound()
    {
        if (loseSound != null)
        {
            AudioManager.Instance.PlaySound(loseSound);
            if(AudioManager.Instance.IsMusicPlaying())
            {
                _wasPlayingMusic = true;
                AudioManager.Instance.StopMusic();
            } else _wasPlayingMusic = false;
        }
    }
    
    private void HandleOnRestart()
    {
        if(_wasPlayingMusic) AudioManager.Instance.ResumeMusic();
    }

    private void HandleButtonClickSound()
    {
        if(buttonClickSound != null)
            AudioManager.Instance.PlaySound(buttonClickSound);
    }
    
    private void HandleToggleChangedSound(bool _)
    {
        if(buttonClickSound != null)
            AudioManager.Instance.PlaySound(buttonClickSound);
    }
}