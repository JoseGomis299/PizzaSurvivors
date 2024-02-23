using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class AudioPlayer : MonoBehaviour
{
    [Header("Sound Effects")]
    [SerializeField] private AudioClip addIngredientSound;
    [SerializeField] private AudioClip removeIngredientSound;
    [SerializeField] private AudioClip spawnSound;
    [SerializeField] private AudioClip enemyMeleeAttackSound;
    [SerializeField] private AudioClip loseSound;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip buttonClickSound;
    
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
        Pizza.OnIngredientPlaced += HandleAddIngredientSound;
        Pizza.OnIngredientRemoved += HandleRemoveIngredientSound;
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
        
        Pizza.OnIngredientPlaced -= HandleAddIngredientSound;
        Pizza.OnIngredientRemoved -= HandleRemoveIngredientSound;
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
    
    private void HandleSpawnSound()
    {
        if(spawnSound == null) return;
        AudioManager.Instance.PlaySound(spawnSound);
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