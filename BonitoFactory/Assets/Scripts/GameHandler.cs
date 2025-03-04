using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TMPro;

public class GameHandler : MonoBehaviour
{
    public static float volumeLevel = 1.0f; // Stores current volume level
    public static GameHandler Instance; // Singleton instance
    public TextMeshProUGUI moneyBalance;
    public TextMeshProUGUI deliveredFish;
    public GameObject deliveryPrefab;

    public int startingBalance = 1000;

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

        moneyBalance = GameObject.Find("MoneyBalance").GetComponent<TextMeshProUGUI>();
        deliveredFish = GameObject.Find("DeliveryFish").GetComponent<TextMeshProUGUI>();
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
        box.transform.position = new Vector3(65, 5, 100);
        Rigidbody rb = box.AddComponent<Rigidbody>();

        DeliveryBox deliveryBox = box.GetComponent<DeliveryBox>();
        if (deliveryBox != null)
        {
            deliveryBox.SetFishCount(fishCount);
        }
    }

    public void UpdateFishCounter(int newFishCount)
    {
        deliveredFish.text = newFishCount.ToString();
    }

    /*
    *   Deduct from money balance
    */
    public void DeductFromBalance(float amount)
    {
        float newBalance = float.Parse(moneyBalance.text) - amount;
        moneyBalance.text = newBalance.ToString();
    }

    /*
    *   Read from money balance
    */
    public int GetBalance()
    {
        return int.Parse(moneyBalance.text);
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
