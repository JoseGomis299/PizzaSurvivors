using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class SystemAudioPlayer : BaseAudioPlayer
{
    [Header("Sound Effects")]
    [SerializeField] private AudioClip loseSound;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip buttonClickSound;
    
    [Header("Music")]
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip gameMusic;
    
    private bool _wasPlayingMusic;
    
    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        base.OnSceneLoaded(scene, mode);
        
        if (scene.buildIndex == 0)
        {
            AudioManager.Instance.PlayMusic(mainMenuMusic);
        }else if (scene.buildIndex == 1)
        {
            AudioManager.Instance.PlayMusic(gameMusic);
        }
    }

    protected override void SubscribeToEvents()
    {
        foreach (var button in FindObjectsOfType<Button>())
        {
            button.onClick.AddListener(HandleButtonClickSound);
        }
        
        foreach (var toggle in FindObjectsOfType<Toggle>())
        {
            toggle.onValueChanged.AddListener(HandleToggleChangedSound);
        }
    }

    protected override  void UnsubscribeToEvents()
    {
        foreach (var button in FindObjectsOfType<Button>())
        {
            button.onClick.RemoveListener(HandleButtonClickSound);
        }
        
        foreach (var toggle in FindObjectsOfType<Toggle>())
        {
            toggle.onValueChanged.RemoveListener(HandleToggleChangedSound);
        }
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