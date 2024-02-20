using UnityEngine;

public class MenuManager
{
    private MenuManager() { }
    public static void SoundEffectsToggleOnValueChanged(bool value)
    {
        PlayerPrefs.SetInt("SoundEffects", value ? 1 : 0);
        AudioManager.Instance.ToggleEffects(value);
    }

    public static void MusicToggleOnValueChanged(bool value)
    {
        PlayerPrefs.SetInt("Music", value ? 1 : 0);
        AudioManager.Instance.ToggleMusic(value);
    }
}
