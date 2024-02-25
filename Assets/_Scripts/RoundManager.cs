using System;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public static event Action<int> OnRoundStart;
    public static event Action<int> OnRoundEnd;
    public static event Action<float> OnTimerChanged;
    
    [SerializeField] private float roundDuration = 60f;
    public float RoundTimer { get; private set; }
    public int CurrentRound { get; private set; }
    
    private bool _roundEnded;

    private void Start()
    {
        CurrentRound = 0;
        GoNextRound();
    }

    public void GoNextRound()
    {
        CurrentRound++;
        _roundEnded = false;
        RoundTimer = roundDuration;
        OnRoundStart?.Invoke(CurrentRound);
    }

    private void Update()
    {
        if (RoundTimer <= 0)
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                GoNextRound();
                return;
            }
            if (_roundEnded) return;
            
            OnRoundEnd?.Invoke(CurrentRound);
            _roundEnded = true;
            return;
        }
        
        RoundTimer -= Time.deltaTime;
        OnTimerChanged?.Invoke(RoundTimer);
    }
}