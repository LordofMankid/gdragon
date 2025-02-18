using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    public static int playerHealth; // Player health
    public static int StartPlayerHealth = 100; // Default starting health
    public AudioMixer mixer; // Audio mixer for volume control
    public static float volumeLevel = 1.0f; // Stores current volume level

    void Start()
    {
        // Initialize the game state
        playerHealth = StartPlayerHealth;

        // Ensure the audio level is set correctly at the start
        SetLevel(volumeLevel);
    }

    // Restart the current level
    public void Restart()
    {
        Time.timeScale = 1f; // Reset game time
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload current scene
    }

    // Restart the game and return to Main Menu
    public void RestartGame()
    {
        Time.timeScale = 1f; // Reset game time
        SceneManager.LoadScene("MainMenu"); // Load Main Menu
        ResetGameVariables();
    }

    // Replay the last level the player died in
    public void ReplayLastLevel()
    {
        Time.timeScale = 1f; // Reset game time
        SceneManager.LoadScene("lastLevelDied"); // Load last level
        ResetGameVariables();
    }

    // Quit the game (works in build mode)
    public void QuitGame()
    {
        Time.timeScale = 1f; // Reset game time

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop game in editor
        #else
        Application.Quit(); // Quit the application
        #endif
    }

    // Adjust game volume using UI Slider
    public void SetLevel(float sliderValue)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20); // Adjust volume
        volumeLevel = sliderValue; // Store volume level
    }

    // Resets all necessary static variables when starting a new game
    private void ResetGameVariables()
    {
        playerHealth = StartPlayerHealth;
        // Add other static variables that need resetting here
    }
}
