using System.Collections;
using UnityEngine;
using TMPro;

public class TimerController : MonoBehaviour
{
    public float countdownTime = 10f; // Total countdown time in seconds
    private GameController gameController; // Reference to the GameController
    public TextMeshProUGUI timerText; // Reference to the TextMeshPro component
    public float timeLeft;

    // Method to start the countdown
    public void StartCountdown(GameController controller)
    {
        gameController = controller; // Store the reference to the GameController
        StartCoroutine(CountdownCoroutine());
    }

    private IEnumerator CountdownCoroutine()
    {
        Debug.Log("In Countdown Coroutine");
        float timer = countdownTime; // Initialize the timer

        while (timer > 0)
        {
            // Update the timer text
            timeLeft = timer;
            timerText.text = "Timer: " + Mathf.Ceil(timer).ToString() + "s";
            timer -= Time.deltaTime; // Decrement the timer
            yield return null; // Wait for the next frame
        }

        timerText.text = "Timer: 0s";
        // Timer has reached zero, stop the game
        StopGame();
    }

    private void StopGame()
    {
        if (gameController != null)
        {
            gameController.StopGame(); // Call the method to stop the game
            Debug.Log("Game stopped.");
        }
    }
}