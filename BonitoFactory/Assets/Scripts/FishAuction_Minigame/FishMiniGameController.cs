using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System;

public class FishMiniGameController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 1.0f;
    private float targetX;
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
    public TextMeshProUGUI currentPrice;

    [Header("Color Settings")]
    public Color highlightColor = Color.green;
    private Color originalColor;
    private SpriteRenderer progressBarSpriteRenderer;

    private bool withinBounds;
    private CurrencyManager currencyManager;

    public float countdownTime = 10f;

    private void Start()
    {
        LeftMarker = GameObject.FindGameObjectWithTag("LeftMarker_FishAuction").transform;
        RightMarker = GameObject.FindGameObjectWithTag("RightMarker_FishAuction").transform;

        if (LeftMarker == null || RightMarker == null)
        {
            Debug.LogError("One or more Transforms are not assigned in the Inspector!", this);
        }

        targetX = RightMarker.position.x;
        progressBarSpriteRenderer = ProgressBarContainer.GetComponentInChildren<SpriteRenderer>();
        originalColor = progressBarSpriteRenderer.color;

        // currencyManager = FindObjectOfType<CurrencyManager>();

        currentPrice = GameObject.FindGameObjectWithTag("CurrentPrice_FishAuction").GetComponent<TextMeshProUGUI>();
        timerText = GameObject.FindGameObjectWithTag("Timer_FishAuction").GetComponent<TextMeshProUGUI>();
        FishTarget = GameObject.FindGameObjectWithTag("Target_FishAuction").transform;
        StartCountdown();
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
        Time.timeScale = 0;
    }

    void Update()
    {
        if (Time.timeScale == 0) return;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetX, transform.position.y, transform.position.z), moveSpeed * Time.deltaTime);
        if (Mathf.Abs(transform.position.x - targetX) < 0.01f)
        {
            movingRight = !movingRight;
            targetX = movingRight ? RightMarker.position.x : LeftMarker.position.x;
        }
        FishTarget_LeftEdge = FishTarget.position.x - (FishTarget.localScale.x / 2);
        FishTarget_RightEdge = FishTarget.position.x + (FishTarget.localScale.x / 2);
        withinBounds = transform.position.x >= FishTarget_LeftEdge && transform.position.x <= FishTarget_RightEdge;

        ClickAction();
    }

    void ClickAction()
    {
        if (Input.GetMouseButtonDown(0) && withinBounds)
        {
            ProgressBarContainer.localScale += new Vector3(scaleAmount, 0, 0);
            StartCoroutine(ChangeColorTemporarily(highlightColor, 1.0f));
            MoveFishMarker();
            UpdateCurrentPrice();
            // CheckMiniGameSuccess();
        }
        else if (!isScaling)
        {
            StartCoroutine(DescaleProgressBar());
        }
    }

    private void MoveFishMarker()
    {
        float randomX = UnityEngine.Random.Range(LeftMarker.position.x, RightMarker.position.x);
        FishTarget.position = new Vector3(randomX, FishTarget.position.y, FishTarget.position.z);
    }
    private void UpdateCurrentPrice()
    {
        float localX = ProgressBarContainer.localScale.x; // Normalized progress (0 to 1)
        // float startX = 21f;
        // float endX = 621f;
        // int startPrice = 200;
        // int endPrice = 75;

        // Interpolate from $200 to $75 based on the current x position
        float price = 200f - (localX / 600f * 200f * 3 / 5);
        Debug.Log(localX / 600f);
        currentPrice.text = "Current Price: $" + Mathf.Floor(price);

        Debug.Log(currentPrice.text);
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
            yield return null;
        }
        isScaling = false;
    }

    // void CheckMiniGameSuccess()
    // {
    //     if (ProgressBarContainer.localScale.x >= 1)
    //     {
    //         currencyManager.DeductCurrency(Mathf.RoundToInt(ProgressBarContainer.localScale.x * 1));
    //     }
    // }
}
