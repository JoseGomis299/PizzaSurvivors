using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource effectSource;
    
    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    public void PlaySound(AudioClip clip)
    { 
        if(clip == null)
        {
            Debug.LogError("AudioClip is missing!");
            return;
        }
        
        effectSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        if(clip == null)
        {
            Debug.LogError("AudioClip is missing!");
            return;
        }
        
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void ChangeMasterVolume(float value)
    {
        AudioListener.volume = value;
    }
    
    public float GetMasterVolume()
    {
        return AudioListener.volume;
    }

    public void ChangeMusicVolume(float value)
    {
        musicSource.volume = value;
    }

    public void ChangeEffectsVolume(float value)
    {
        effectSource.volume = value;
    }

    public void PauseMusic()
    {
        musicSource.Pause();
    }

    public void StopEffect()
    {
        effectSource.Stop();
    }
    
    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void ResumeMusic()
    {
        musicSource.Play();
    }

    public void ToggleEffects()
    {
        effectSource.mute = !effectSource.mute;
    }
    
    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }
    
    public void ToggleEffects(bool value)
    {
        effectSource.mute = value;
    }
    
    public void ToggleMusic(bool value)
    {
        musicSource.mute = value;
        if(value)
            musicSource.Pause();
        else
            musicSource.Play();
    }
    
    public bool IsMusicPlaying()
    {
        return musicSource.isPlaying;
    }
}