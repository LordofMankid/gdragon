using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.VisualScripting;

public class FishMiniGameController : MonoBehaviour
{
    // [SerializeField] bool disabled = false;

    private bool gameOver;

    [Header("Movement Settings")]
    public float moveSpeed = 1.0f;
    [SerializeField] float targetX;
    private bool movingRight = true;

    [Header("Boundaries & Markers")]
    public Transform LeftMarker;
    public Transform RightMarker;
    public Transform FishTarget;
    [SerializeField] float FishTarget_LeftEdge;
    [SerializeField] float FishTarget_RightEdge;

    [Header("Progress Bar Settings")]
    public Transform ProgressBarContainer;
    public float scaleAmount = 0.1f;
    public float descalingSpeed = 0.1f;
    private bool isScaling = false;

    [Header("UI References")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI currentPriceText;
    public float currentPrice;
    public TextMeshProUGUI moneyBalance;
    public bool deducted = false;

    public GameObject EndGamePopup;
    public GameObject ReplayButton;
    public GameObject LeaveButton;

    [Header("Color Settings")]
    public Color highlightColor = Color.green;
    private Color originalColor;
    private SpriteRenderer progressBarSpriteRenderer;

    private bool withinBounds;

    public float countdownTime = 10f;

    private void Awake()
    {
        LeftMarker = GameObject.FindGameObjectWithTag("LeftMarker_FishAuction").transform;
        RightMarker = GameObject.FindGameObjectWithTag("RightMarker_FishAuction").transform;

        targetX = RightMarker.position.x;
        ProgressBarContainer = GameObject.FindGameObjectWithTag("ProgressBarContainer_FishAuction").transform;
        progressBarSpriteRenderer = ProgressBarContainer.GetComponentInChildren<SpriteRenderer>();
        originalColor = progressBarSpriteRenderer.color;

        currentPriceText = GameObject.FindGameObjectWithTag("CurrentPrice_FishAuction").GetComponent<TextMeshProUGUI>();
        timerText = GameObject.FindGameObjectWithTag("Timer_FishAuction").GetComponent<TextMeshProUGUI>();
        moneyBalance = GameObject.FindGameObjectWithTag("MoneyBalance").GetComponentInChildren<TextMeshProUGUI>();
        FishTarget = GameObject.FindGameObjectWithTag("Target_FishAuction").transform;
        EndGamePopup = GameObject.FindGameObjectWithTag("EndGamePopup_FishAuction");
        if (EndGamePopup == null)
        {
            Debug.LogError("EndGamePopup GameObject is missing. Please check the tag.");
            return;
        }

        ReplayButton = GameObject.FindGameObjectWithTag("ReplayButton_FishAuction");
        LeaveButton = GameObject.FindGameObjectWithTag("LeaveButton_FishAuction");

        EndGamePopup.SetActive(false);
        gameOver = false;

        moneyBalance.text = "1000"; // Temporary
    }

    void Start()
    {
        StartCountdown();
    }

    private void Update()
    {
        if (gameOver) return; // Stop updates when the game is over

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetX, transform.position.y, transform.position.z), moveSpeed * Time.deltaTime);

        if (Mathf.Abs(transform.position.x - targetX) < 0.01f)
        {
            movingRight = !movingRight;
            targetX = movingRight ? RightMarker.position.x : LeftMarker.position.x;
        }

        FishTarget_LeftEdge = FishTarget.localPosition.x - 65f;
        FishTarget_RightEdge = FishTarget.localPosition.x + 65f;
        withinBounds = transform.localPosition.x >= FishTarget_LeftEdge && transform.localPosition.x <= FishTarget_RightEdge;

        HandleBidClick();
    }


    private void EndGame()
    {
        if (!gameOver)
        {
            gameOver = true;
            StopAllCoroutines();
            DeductFromBalance();
            ShowEndGamePopup();
        }
    }

    private void ShowEndGamePopup()
    {
        EndGamePopup.SetActive(true);
        ReplayButton.GetComponent<Button>().onClick.AddListener(ResetGame);
        LeaveButton.GetComponent<Button>().onClick.AddListener(LeaveGame);
    }

    private void ResetGame()
    {
        targetX = RightMarker.position.x;
        movingRight = true;
        isScaling = false;

        currentPrice = 0;
        currentPriceText.text = "Current Price: $0";
        ProgressBarContainer.localScale = new Vector3(50, ProgressBarContainer.localScale.y, ProgressBarContainer.localScale.z);
        EndGamePopup.SetActive(false);

        timerText.text = "Timer: " + countdownTime + "s";
        StartCountdown();
        Time.timeScale = 1;
        deducted = false;
        gameOver = false;
    }

    private void LeaveGame()
    {
        Debug.Log("Leave Button was clicked");
    }


    public void StartCountdown()
    {
        StartCoroutine(CountdownCoroutine());
    }

    private IEnumerator CountdownCoroutine()
    {
        float timer = countdownTime;
        while (timer > 0)
        {
            timerText.text = "Timer: " + Mathf.Ceil(timer).ToString() + "s";
            timer -= Time.deltaTime;
            yield return null;
        }

        timerText.text = "Timer: 0s";
        EndGame();
    }

    void HandleBidClick()
    {
        if (Input.GetMouseButtonDown(0) && withinBounds)
        {
            ProgressBarContainer.localScale += new Vector3(scaleAmount, 0, 0);
            StartCoroutine(ChangeColorTemporarily(highlightColor, 1.0f));
            RandomizeTargetPosition();
        }
        else if (!isScaling)
        {
            StartCoroutine(DescaleProgressBar());
        }
    }

    private void RandomizeTargetPosition()
    {
        float randomX = UnityEngine.Random.Range(LeftMarker.position.x, RightMarker.position.x);
        FishTarget.position = new Vector3(randomX, FishTarget.position.y, FishTarget.position.z);
    }

    private void UpdateCurrentPrice()
    {
        float localX = ProgressBarContainer.localScale.x;
        float price = 200f - (localX / 600f * 200f * 3 / 5);
        Debug.Log(localX / 600f);
        currentPrice = Mathf.Floor(price);
        currentPriceText.text = "Current Price: $" + currentPrice;

        Debug.Log(currentPriceText.text);
    }

    private void DeductFromBalance()
    {
        if (!deducted)
        {
            int currentBalance = int.Parse(moneyBalance.text);
            int newBalance = (int)(currentBalance - currentPrice);
            Debug.Log("New Balance: " + newBalance);
            moneyBalance.text = newBalance.ToString();
            deducted = true;
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
        while (ProgressBarContainer.localScale.x > 0)
        {
            ProgressBarContainer.localScale -= new Vector3(descalingSpeed * Time.deltaTime, 0, 0);
            UpdateCurrentPrice();
            yield return null;
        }
        isScaling = false;
    }
}
