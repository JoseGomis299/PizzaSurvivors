using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager_MainMenu : MonoBehaviour
{
    [SerializeField] protected Button playButton;
    [SerializeField] protected Button exitButton;

    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle sfxToggle;

    protected void OnEnable()
    {
        playButton.onClick.AddListener(PlayButtonOnClick);
        exitButton.onClick.AddListener(ExitButtonOnClick);

        sfxToggle.onValueChanged.AddListener(MenuManager.SoundEffectsToggleOnValueChanged);
        musicToggle.onValueChanged.AddListener(MenuManager.MusicToggleOnValueChanged);

        sfxToggle.isOn = PlayerPrefs.GetInt("SoundEffects", 0) == 1;
        musicToggle.isOn = PlayerPrefs.GetInt("Music", 0) == 1;

        AudioManager.Instance.ToggleEffects(sfxToggle.isOn);
        AudioManager.Instance.ToggleMusic(musicToggle.isOn);
    }

    private void ExitButtonOnClick()
    {
        Application.Quit();
    }

    private void PlayButtonOnClick()
    {
        SceneManager.LoadScene(1);
    }
}
