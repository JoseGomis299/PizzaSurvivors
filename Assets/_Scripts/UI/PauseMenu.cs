using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle soundToggle;

    private void Awake()
    {
        GameManager.OnPauseGame += ShowMenu;
        GameManager.OnResumeGame += HideMenu;
        
        resumeButton.onClick.AddListener(ResumeGame);
        quitButton.onClick.AddListener(ExitGame);
        restartButton.onClick.AddListener(RestartGame);

        soundToggle.onValueChanged.AddListener(MenuManager.SoundEffectsToggleOnValueChanged);
        musicToggle.onValueChanged.AddListener(MenuManager.MusicToggleOnValueChanged);

        soundToggle.isOn = PlayerPrefs.GetInt("SoundEffects", 0) == 1;
        musicToggle.isOn = PlayerPrefs.GetInt("Music", 0) == 1;

        AudioManager.Instance.ToggleEffects(soundToggle.isOn);
        AudioManager.Instance.ToggleMusic(musicToggle.isOn);
    }

    private void OnDestroy()
    {
        GameManager.OnPauseGame -= ShowMenu;
        GameManager.OnResumeGame -= HideMenu;
    }

    private void ShowMenu()
    {
        pauseMenu.SetActive(true);
    }

    private void HideMenu()
    {
        pauseMenu.SetActive(false);
    }
    
    private void ResumeGame()
    {
        GameManager.Instance.ResumeGame();
    }
    
    private void RestartGame()
    {
        GameManager.Instance.RestartGame();
    }
    
    private void ExitGame()
    {
        GameManager.Instance.EndGame();
        SceneManager.LoadScene(0);
    }
}