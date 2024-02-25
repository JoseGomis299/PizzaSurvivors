using System;
using ProjectUtils.Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static event Action OnGameStart;
    public static event Action OnGameEnd;
    public static event Action OnPauseGame;
    public static event Action OnResumeGame;
    public static event Action OnRestartGame;
    
    public static GameManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        OnGameStart?.Invoke();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0f)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void EndGame()
    {
        IngredientInventory.Clear();
        CoroutineController.StopAll();
        
        OnGameEnd?.Invoke();
    }
    
    public void PauseGame()
    {
        Time.timeScale = 0f;
        OnPauseGame?.Invoke();
    }
    
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        OnResumeGame?.Invoke();
    }
    
    public void RestartGame()
    {
        IngredientInventory.Clear();
        CoroutineController.StopAll();
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        OnRestartGame?.Invoke();
    }
}