using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    public static int playerHealth; // Player health
    public static int StartPlayerHealth = 100; // Default starting health
    //public AudioMixer mixer; // Audio mixer for volume control
    public static float volumeLevel = 1.0f; // Stores current volume level
    public static GameHandler Instance; // Singleton instance
    public int money = 0;

    private void Awake()
    {
        // Ensure only one instance of GameManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Make the GameManager persistent
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame()
    {
        Debug.Log("Game started!");
        SceneManager.LoadScene("Level1");
    }

    public void ReturnToMainMenu()
    {
        Debug.Log("Returning to Main Menu");
        SceneManager.LoadScene("MainMenu");
    }

    public void RollCredits()
    {
        Debug.Log("Rolling Credits");
        SceneManager.LoadScene("Credits");
    }

    public void StartMinigame()
    {
        Debug.Log("Minigame started!");
        // Add your minigame logic here
        SceneManager.LoadScene("FishAuctionMinigame");
    }


    void Start()
    {
        // Initialize the game state
        playerHealth = StartPlayerHealth;

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

    // Resets all necessary static variables when starting a new game
    private void ResetGameVariables()
    {
        playerHealth = StartPlayerHealth;
        // Add other static variables that need resetting here
    }
}
