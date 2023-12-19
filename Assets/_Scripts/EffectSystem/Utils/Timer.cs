using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    public float ElapsedTime { get; private set; }
    public float Duration { get; private set; }
    
    public event Action OnTimerEnd;
    private MonoBehaviour _timePlayer;
    
    private Coroutine _timerCoroutine;

    public Timer(float duration, MonoBehaviour monoBehaviour)
    {
        Duration = duration;
        _timePlayer = monoBehaviour;
    }
    
    public void ResetTimer() => ElapsedTime = Duration;

    public void ResetAndStartTimer()
    {
        ResetTimer();
        StartTimer();
    }
    
    public void ResetAndStartTimer(float duration)
    {
        Duration = duration;
        ResetTimer();   
        StartTimer();
    }

    public void StartTimer()
    {
        if(!_timePlayer.gameObject.activeInHierarchy) return;
        
        if (_timerCoroutine != null)
            _timePlayer.StopCoroutine(_timerCoroutine);
        _timerCoroutine = _timePlayer.StartCoroutine(TickTimer());
    }
    
    public void StopTimer()
    {
        if (_timerCoroutine != null)
            _timePlayer.StopCoroutine(_timerCoroutine);
    }
    
    public void SetDuration(float duration)
    {
        Duration = duration;
        ResetTimer();
    }

    private IEnumerator TickTimer()
    {
        while (ElapsedTime > 0)
        {
            ElapsedTime -= Time.deltaTime;
            yield return null;
        }
        OnTimerEnd?.Invoke();
    }
}