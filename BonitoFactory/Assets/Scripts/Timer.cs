using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 900f; // 15 minutes
    public Text timerText; // Legacy Text
    private bool isRunning = true;

    void Update()
    {
        if (isRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerUI();
            }
            else
            {
                EndGame();
            }
        }
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = $"Time Left: {minutes:00}:{seconds:00}";
    }

    void EndGame()
    {
        timeRemaining = 0;
        isRunning = false;
        timerText.text = "Time's Up!";
        Debug.Log("Time's up!");

        // Stop all movement and actions
        Time.timeScale = 0f;

        // Destroy all logs
        DestroyAllLogs();
    }

    void DestroyAllLogs()
    {
        GameObject[] logs = GameObject.FindGameObjectsWithTag("Log");
        foreach (GameObject log in logs)
        {
            Destroy(log);
        }
    }
}
