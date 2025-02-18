using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuPrefab;  // Assign the Pause Menu Prefab in the Inspector
    private GameObject pauseMenuInstance;
    public Button resumeButton;
    public Button restartButton;
    public Button quitButton;

    private bool isPaused = false;

    void Start()
    {
        // Check if there's already a pause menu instance in the scene
        if (GameObject.FindWithTag("PauseMenu") == null)
        {
            pauseMenuInstance = Instantiate(pauseMenuPrefab);
            pauseMenuInstance.SetActive(false);
        }
        else
        {
            pauseMenuInstance = GameObject.FindWithTag("PauseMenu");
        }

        // Assign buttons dynamically if not assigned
        resumeButton = pauseMenuInstance.transform.Find("button_Resume").GetComponent<Button>();
        restartButton = pauseMenuInstance.transform.Find("button_Restart").GetComponent<Button>();
        quitButton = pauseMenuInstance.transform.Find("button_Quit").GetComponent<Button>();

        // Add button listeners
        resumeButton.onClick.AddListener(ResumeGame);
        restartButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        pauseMenuInstance.SetActive(true);
        Time.timeScale = 0f; // Pause game
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuInstance.SetActive(false);
        Time.timeScale = 1f; // Resume game
        isPaused = false;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Ensure game runs before restarting
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
