using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("Play button clicked!");
        SceneManager.LoadScene("FishAuctionMInigame"); // Change this to your game scene name
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in Unity Editor
        #else
        Application.Quit(); // Quit the application
        #endif
    }
}
