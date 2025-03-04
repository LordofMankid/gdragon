using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement; // Needed for Restart functionality

public class GameHandler_PauseMenu : MonoBehaviour
{
    public static bool GameisPaused = false; // Tracks pause state
    public GameObject pauseMenuUI; // Pause Menu UI reference
    public AudioMixer mixer; // Audio Mixer for volume control
    public static float volumeLevel = 1.0f; // Stores volume level
    private Slider sliderVolumeCtrl; // Slider reference

    void Awake()
    {
        // Ensure the audio level is set correctly
        SetLevel(volumeLevel);

        // Find the volume slider if it exists in the UI
        GameObject sliderTemp = GameObject.FindWithTag("PauseMenuSlider");
        if (sliderTemp != null)
        {
            sliderVolumeCtrl = sliderTemp.GetComponent<Slider>();
            sliderVolumeCtrl.value = volumeLevel;
        }
    }

    void Start()
    {
        pauseMenuUI.SetActive(false); // Hide pause menu at start
        GameisPaused = false;
    }

    void Update()
    {
        // Toggle pause menu with Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameisPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true); // Show the pause menu
        Time.timeScale = 0f; // Stop game time
        GameisPaused = true; // Update state
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false); // Hide the pause menu
        Time.timeScale = 1f; // Resume game time
        GameisPaused = false; // Update state
    }

    public void Restart()
    {
        Time.timeScale = 1f; // Reset game time
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload current scene
    }

    public void SetLevel(float sliderValue)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20); // Adjust volume
        volumeLevel = sliderValue; // Store volume level
    }

    public void QuitGame()
    {
        Time.timeScale = 1f; // Reset game time

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop game in editor
        #else
        Application.Quit(); // Quit the application
        #endif
    }
}
