using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class FishMiniGameController : MonoBehaviour
{

    private bool gameOver;

    [Header("Movement Settings")]
    public float moveSpeed = 50f;
    public float targetX;
    private bool movingRight = true;

    [Header("Boundaries & Markers")]
    // public Transform LeftMarker;
    // public Transform RightMarker;
    public Transform FishTarget;
    [SerializeField] float FishTarget_LeftEdge;
    [SerializeField] float FishTarget_RightEdge;

    [Header("Progress Bar Settings")]
    public Transform ProgressBarContainer;
    public float scaleAmount = 0.1f;
    public float descalingSpeed = 0.1f;
    private bool isScaling = false;

    [Header("UI References")]
    // public TextMeshProUGUI MoneyBalance;
    // public TextMeshProUGUI DeliveryFish;
    public TextMeshProUGUI Timer;
    public TextMeshProUGUI currentPriceText;
    public float currentPrice;
    public bool deducted = false;

    [Header("Color Settings")]
    public Color highlightColor = Color.green;
    private Color originalColor;
    private SpriteRenderer progressBarSpriteRenderer;

    private bool withinBounds;
    public bool mouseClicked;

    public float countdownTime = 10f;

    public GameObject BidBtn;
    public GameObject StartGameBtn;
    public GameObject PlayAgainBtn;
    public GameObject ExitGameBtn;

    public Transform PlayerPointer;

    public float LeftBound = -760f;
    public float RightBound = 760f;

    public GameObject InsufficientFunds;

    private int fishCount = 0;

    private void Awake()
    {
        targetX = RightBound;
        ProgressBarContainer = GameObject.FindGameObjectWithTag("ProgressBarContainer_FishAuction").transform;
        progressBarSpriteRenderer = ProgressBarContainer.GetComponentInChildren<SpriteRenderer>();
        originalColor = progressBarSpriteRenderer.color;

        PlayerPointer = GameObject.FindGameObjectWithTag("Pointer_FishAuction").transform;

        currentPriceText = GameObject.FindGameObjectWithTag("CurrentPrice_FishAuction").GetComponent<TextMeshProUGUI>();
        Timer = GameObject.FindGameObjectWithTag("Timer_FishAuction").GetComponent<TextMeshProUGUI>();
        // MoneyBalance = GameObject.FindGameObjectWithTag("MoneyBalance").GetComponent<TextMeshProUGUI>();
        // DeliveryFish = GameObject.FindGameObjectWithTag("DeliveryFish").GetComponent<TextMeshProUGUI>();
        FishTarget = GameObject.FindGameObjectWithTag("Target_FishAuction").transform;

        StartGameBtn = GameObject.FindGameObjectWithTag("StartGameBtn_FishAuction");
        ExitGameBtn = GameObject.FindGameObjectWithTag("ExitGameBtn_FishAuction");
        PlayAgainBtn = GameObject.FindGameObjectWithTag("PlayAgainBtn_FishAuction");
        BidBtn = GameObject.FindGameObjectWithTag("BidBtn_FishAuction");

        gameOver = true;
        StartGameBtn.SetActive(true);
        ExitGameBtn.SetActive(true);
        PlayAgainBtn.SetActive(false);
        BidBtn.SetActive(false);

        InsufficientFunds = GameObject.FindGameObjectWithTag("InsufficientFunds");
        InsufficientFunds.SetActive(false);

        StartGameBtn.GetComponent<Button>().interactable = true;
        PlayAgainBtn.GetComponent<Button>().interactable = true;
        BidBtn.GetComponent<Button>().interactable = true;

        CheckBalance();
    }

    public void StartGame()
    {
        Debug.Log("Starting the fish minigame");
        gameOver = false;
        ExitGameBtn.SetActive(false);
        StartGameBtn.SetActive(false);
        PlayAgainBtn.SetActive(false);
        BidBtn.SetActive(true);
        StartCoroutine(CountdownCoroutine());
    }

    public void ResetGame()
    {
        targetX = RightBound;
        movingRight = true;
        isScaling = false;

        PlayerPointer.localPosition = new Vector3(0, PlayerPointer.localPosition.y, PlayerPointer.localPosition.z);

        currentPrice = 0;
        currentPriceText.text = "Bid: $0";
        ProgressBarContainer.localScale = new Vector3(50, ProgressBarContainer.localScale.y, ProgressBarContainer.localScale.z);

        Timer.text = "Time: " + countdownTime + "s";
        StartCoroutine(CountdownCoroutine());
        deducted = false;
        gameOver = false;

        PlayAgainBtn.SetActive(false);
        ExitGameBtn.SetActive(false);
        BidBtn.SetActive(true);
    }

    public void ExitGame()
    {
        // Handle fish logic
        Destroy(gameObject);
        // Deliver that fish
        GameHandler.Instance.DeliverFish(fishCount);
        Debug.Log("Exit Button was clicked");
    }

    public void OnBidButtonClicked()
    {
        if (withinBounds)
        {
            ProgressBarContainer.localScale += new Vector3(scaleAmount, 0, 0);
            StartCoroutine(ChangeColorTemporarily(highlightColor, 1.0f));
            RandomizeTargetPosition();
        }

        mouseClicked = false;

        if (!isScaling)
        {
            StartCoroutine(DescaleProgressBar());
        }
    }

    private void CheckBalance()
    {
        if (GameHandler.Instance.GetBalance() < 200)
        {
            StartGameBtn.GetComponent<Button>().interactable = false;
            PlayAgainBtn.GetComponent<Button>().interactable = false;
            BidBtn.GetComponent<Button>().interactable = false;
            InsufficientFunds.SetActive(true);
        }
    }

    private void Update()
    {
        // Debug.Log("Update is running");
        // Debug.Log("Gameover: " + gameOver);

        if (gameOver) return;

        PlayerPointer.localPosition = Vector3.MoveTowards(PlayerPointer.localPosition, new Vector3(targetX, PlayerPointer.localPosition.y, PlayerPointer.localPosition.z), moveSpeed * Time.deltaTime);
        if (Mathf.Abs(PlayerPointer.localPosition.x - targetX) < 0.01f)
        {
            movingRight = !movingRight;
            targetX = movingRight ? RightBound : LeftBound;
        }

        // Debug.Log("Pointer Loc: " + PlayerPointer.localPosition.x);

        FishTarget_LeftEdge = FishTarget.localPosition.x - 70f;
        FishTarget_RightEdge = FishTarget.localPosition.x + 70f;
        withinBounds = PlayerPointer.localPosition.x >= FishTarget_LeftEdge && PlayerPointer.localPosition.x <= FishTarget_RightEdge;
    }


    private void EndGame()
    {
        if (!gameOver)
        {
            gameOver = true;
            StopAllCoroutines();
            if (!deducted)
            {
                GameHandler.Instance.DeductFromBalance(currentPrice);
            }

            ExitGameBtn.SetActive(true);
            PlayAgainBtn.SetActive(true);
            BidBtn.SetActive(false);
            StartGameBtn.SetActive(false);

            fishCount += 5;
            GameHandler.Instance.UpdateFishCounter(fishCount);
            CheckBalance();
        }
    }

    private IEnumerator CountdownCoroutine()
    {
        float timer = countdownTime;
        while (timer > 0)
        {
            Timer.text = "Time: " + Mathf.Ceil(timer).ToString() + "s";
            timer -= Time.deltaTime;
            yield return null;
        }

        Timer.text = "Time: 0s";
        EndGame();
    }

    private void RandomizeTargetPosition()
    {
        float randomX = UnityEngine.Random.Range(LeftBound + 50f, RightBound - 50f);
        FishTarget.localPosition = new Vector3(randomX, FishTarget.localPosition.y, FishTarget.localPosition.z);
    }

    private void UpdateCurrentPrice()
    {
        float localX = Mathf.Clamp(ProgressBarContainer.localScale.x, 0, 600f);
        float price = 200f - (localX / 600f * 200f * 3 / 5);
        currentPrice = Mathf.Floor(price);
        currentPriceText.text = "Bid: $" + currentPrice;

        Debug.Log("Current Bid: " + currentPriceText.text);
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