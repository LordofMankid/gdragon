using UnityEngine;
using System.Collections;
using TMPro;

public class GameController : MonoBehaviour
{
    protected bool isGameActive = false; // Track if the game is currently active

    public virtual void StartGame()
    {
        isGameActive = true;
        Debug.Log("Game started.");
        // Additional logic to initialize the game state
    }

    public virtual void StopGame()
    {
        isGameActive = false;
        Time.timeScale = 0; // Stops all scripts
        // Consider using a GameState
        Debug.Log("Game stopped.");
        // Additional logic to handle game stopping
    }

    public bool IsGameActive()
    {
        return isGameActive;
    }
}

public class FishMiniGameController : GameController
{
    public float moveSpeed = 1.0f; // Speed of movement
    public float oscillationAmplitude = 1.0f; // Amplitude of oscillation
    private float targetX; // Target X position
    private bool movingRight = true; // Direction of movement

    public Transform LeftMarker;
    public Transform RightMarker;

    public Transform ProgressBarContainer;
    public float scaleAmount = 0.1f;
    public float descalingSpeed = 0.1f;
    public Transform Fish_LeftEdge;
    public Transform Fish_RightEdge;
    private bool isScaling = false;
    
    public Color highlightColor = Color.green; 
    private Color originalColor;
    public SpriteRenderer progressBarSpriteRenderer;

    public float positionX;
    public float leftEdgeX;
    public float rightEdgeX;
    public bool withinBounds;

    private TimerController timerController; // Reference to the TimerController
    public TextMeshProUGUI timerText;

    void Start()
    {
        // Call the base class StartGame method if needed
        StartGame(); // Optional: Start the game when the mini-game controller starts

        // Set the initial target position to the right marker
        targetX = RightMarker.position.x; // Start moving towards the right marker
        progressBarSpriteRenderer = ProgressBarContainer.GetComponentInChildren<SpriteRenderer>();
        originalColor = progressBarSpriteRenderer.color;

        timerController = gameObject.AddComponent<TimerController>(); // Add TimerController to the same GameObject
        timerController.timerText = timerText; // Assign the TextMeshPro component
        timerController.StartCountdown(this);
    }

    void Update()
    {
        // Move the pointer towards the target position
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetX, transform.position.y, transform.position.z), moveSpeed * Time.deltaTime);

        // Check if the pointer has reached the target position
        if (Mathf.Abs(transform.position.x - targetX) < 0.01f)
        {
            // Switch target position based on current direction
            if (movingRight)
            {
                targetX = LeftMarker.position.x; // Move to the left marker
                movingRight = false; // Change direction
            }
            else
            {
                targetX = RightMarker.position.x; // Move to the right marker
                movingRight = true; // Change direction
            }
        }

        // For Debugging 
        positionX = transform.position.x;
        leftEdgeX = Fish_LeftEdge.position.x;
        rightEdgeX = Fish_RightEdge.position.x;
        withinBounds = positionX <= leftEdgeX && positionX >= rightEdgeX;

        ClickAction();
    }  
    
    void ClickAction()
    {
        // Check if the pointer is within the bounds of the edges
        if (Input.GetMouseButtonDown(0) && positionX <= Fish_LeftEdge.position.x && positionX >= Fish_RightEdge.position.x)
        {
            // Scale the ProgressBarContainer
            ProgressBarContainer.localScale += new Vector3(scaleAmount, 0, 0);
            Debug.Log("Clicked! Increment!");
            StartCoroutine(ChangeColorTemporarily(highlightColor, 1.0f)); // Change color for 1 second
        }
        else
        {
            if (!isScaling)
            {
                StartCoroutine(DescaleProgressBar());
            }
            Debug.Log("Decrementing...");
        }
    }

    private IEnumerator ChangeColorTemporarily(Color newColor, float duration)
    {
        progressBarSpriteRenderer.color = newColor;
        yield return new WaitForSeconds(duration);
        progressBarSpriteRenderer.color = originalColor;
    }

    private IEnumerator DescaleProgressBar()
    {
        isScaling = true;

        // Gradually descaling the ProgressBarContainer
        while (ProgressBarContainer.localScale.x > 0)
        {
            ProgressBarContainer.localScale -= new Vector3(descalingSpeed * Time.deltaTime, 0, 0);
            yield return null; // Wait for the next frame
        }

        isScaling = false; // Reset scaling state
    }
}