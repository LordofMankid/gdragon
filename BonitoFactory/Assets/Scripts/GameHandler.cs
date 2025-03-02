using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    // public static int playerHealth; // Player health
    // public static int StartPlayerHealth = 100; // Default starting health
    // public AudioMixer mixer; // Audio mixer for volume control
    public static float volumeLevel = 1.0f; // Stores current volume level
    public static GameHandler Instance; // Singleton instance
    public GameObject moneyBalance;
    public int startingBalance = 1000;
    public GameObject deliveryPrefab;

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

        moneyBalance = GameObject.FindGameObjectWithTag("MoneyBalance");
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Level1");
        Debug.Log("Game started.");
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

    /* adjust the number of fish or you can pass in money and convert in GameHandler conversion instead, as long as 
     * you pass in a number of fish into the setFishCount function
     * 
     */
    public void DeliverFish(int fishCount)
    {
        if (deliveryPrefab == null)
        {
            Debug.LogError("Delivery Prefab is missing!");
            return;
        }

        // spawn the box
        GameObject box = Instantiate(deliveryPrefab);

        DeliveryBox deliveryBox = box.GetComponent<DeliveryBox>();
        if (deliveryBox != null)
        {
            deliveryBox.SetFishCount(fishCount);
        }
    }
    void Start()
    {
        SetLevel(volumeLevel);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
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
        Time.timeScale = 1f; // Reset game time

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop game in editor
#else
        Application.Quit(); // Quit the application
#endif
    }

    public void SetLevel(float sliderValue)
    {
        // mixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20); // Adjust volume
        volumeLevel = sliderValue; // Store volume level
    }

    // Resets all necessary static variables when starting a new game
    private void ResetGameVariables()
    {
        moneyBalance.GetComponent<TMPro.TextMeshProUGUI>().text = startingBalance.ToString();
        // Add other static variables that need resetting here
    }
}
