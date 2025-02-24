using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    public static int playerHealth = 100;
    public AudioMixer mixer;
    public static float volumeLevel = 1.0f;

    void Start()
    {
        SetLevel(volumeLevel);
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        Debug.Log("Game started.");
    }

    public void StopGame()
    {
        Time.timeScale = 0;
        Debug.Log("Game stopped.");
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void SetLevel(float sliderValue)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        volumeLevel = sliderValue;
    }
}
