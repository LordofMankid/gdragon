using System.Collections;
using UnityEngine;
using TMPro;

public class TimerController : MonoBehaviour
{
    public float countdownTime = 10f;
    public TextMeshProUGUI timerText;
    public float timeLeft;
    
    public void StartCountdown()
    {
        StartCoroutine(CountdownCoroutine());
    }

    private IEnumerator CountdownCoroutine()
    {
        Debug.Log("In Countdown Coroutine");
        float timer = countdownTime;

        while (timer > 0)
        {
            timeLeft = timer;
            timerText.text = "Timer: " + Mathf.Ceil(timer).ToString() + "s";
            timer -= Time.deltaTime;
            yield return null;
        }

        timerText.text = "Timer: 0s";
        Time.timeScale = 0;
    }
}
