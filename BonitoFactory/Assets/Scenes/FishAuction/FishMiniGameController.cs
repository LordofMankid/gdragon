using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class FishMiniGameController : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    private float targetX;
    private bool movingRight = true;

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

    public bool withinBounds;
    
    private TimerController timerController;
    private CurrencyManager currencyManager;
    
    public TextMeshProUGUI tempTimer;  
    public Text currencyText;  

    void Start()
    {
        if (Fish_LeftEdge == null || Fish_RightEdge == null || LeftMarker == null || RightMarker == null)
        {
            Debug.LogError("One or more Transforms are not assigned in the Inspector!", this);
        }

        targetX = RightMarker.position.x;
        progressBarSpriteRenderer = ProgressBarContainer.GetComponentInChildren<SpriteRenderer>();
        originalColor = progressBarSpriteRenderer.color;

        timerController = FindObjectOfType<TimerController>();
        if (timerController != null)
        {
            timerController.StartCountdown();
        }
        else
        {
            Debug.LogError("TimerController not found in the scene!");
        }

        currencyManager = FindObjectOfType<CurrencyManager>();
        if (currencyManager == null)
        {
            Debug.LogError("CurrencyManager not found!");
        }

        currencyText = GameObject.Find("Canvas/CurrencyText").GetComponent<Text>();
        tempTimer = GameObject.Find("Minigame UI/Canvas/TempTimer").GetComponent<TextMeshProUGUI>();
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

        withinBounds = transform.position.x <= Fish_LeftEdge.position.x && transform.position.x >= Fish_RightEdge.position.x;

        ClickAction();
    }

    void ClickAction()
    {
        if (Input.GetMouseButtonDown(0) && withinBounds)
        {
            ProgressBarContainer.localScale += new Vector3(scaleAmount, 0, 0);
            StartCoroutine(ChangeColorTemporarily(highlightColor, 1.0f));
            CheckMiniGameSuccess();
        }
        else
        {
            if (!isScaling)
            {
                StartCoroutine(DescaleProgressBar());
            }
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
            yield return null;
        }
        isScaling = false;
    }

    void CheckMiniGameSuccess()
    {
        if (ProgressBarContainer.localScale.x >= 1)
        {
            currencyManager.DeductCurrency(Mathf.RoundToInt(ProgressBarContainer.localScale.x * 1));
        }
    }
}
