using UnityEngine;

public class MinigameTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger is a player
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            // Notify the GameManager to start the minigame
            GameHandler.Instance.StartMinigame();
        }
    }
}