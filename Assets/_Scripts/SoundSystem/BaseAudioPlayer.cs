using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class BaseAudioPlayer : MonoBehaviour
{
    protected virtual void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void Start()
    {
        SubscribeToEvents();
    }
    
    protected virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UnsubscribeToEvents();
        SubscribeToEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeToEvents();
    }

    protected abstract void SubscribeToEvents();
    protected abstract void UnsubscribeToEvents();
    
    protected void PlaySound(AudioClip clip)
    {
        AudioManager.Instance.PlaySound(clip);
    }
    
    protected void PlayMusic(AudioClip clip)
    {
        AudioManager.Instance.PlayMusic(clip);
    }
    
    protected void StopMusic()
    {
        AudioManager.Instance.StopMusic();
    }
 
    protected void PlaySoundAtThisPosition(AudioClip clip)
    {
        if(AudioManager.Instance.AreEffectsMuted()) return;
        AudioSource.PlayClipAtPoint(clip, transform.position, AudioManager.Instance.GetEffectsVolume());
    }
}