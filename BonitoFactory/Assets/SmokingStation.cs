using System.Collections;
using UnityEngine;

public class SmokerStation : ProcessingStation
{
    public GameObject logPrefab; // Prefab for the log input
    public int maxLogs = 5; // Maximum number of logs the smoker can hold
    public float burnTimePerLog = 15f; // Time each log burns for
    public GameObject burnProgressBarTransform; // Transform for the burn progress bar UI

    private int currentLogs = 0; // Current number of logs in the smoker
    private float remainingBurnTime = 0f; // Remaining burn time
    private ProgressBarUILogic burnProgressBar; // Reference to the burn progress bar logic
    private bool isBurning = false; // Whether the logs are currently burning
    private float savedSmokingProgress = 0f; // Saved progress for the smoking process

    private float processTime = 3f;

    protected override void Start()
    {
        base.Start(); // Call the base class Start method
        if (burnProgressBarTransform != null)
        {
            burnProgressBar = burnProgressBarTransform.GetComponent<ProgressBarUILogic>();
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other); // Call the base class method to handle player and item interactions

        // Handle logs separately
        ThrowableLogic thrownItem = other.gameObject.GetComponent<ThrowableLogic>();
        if (thrownItem != null && thrownItem.IsThrown)
        {
            CookingItem item = thrownItem.gameObject.GetComponent<CookingItem>();
            if (item != null && item.itemName == "Log") // Check if the item is a log
            {
                AddLog();
                Destroy(thrownItem.gameObject); // Destroy the log after adding it
            }
        }
    }

    // Add a log to the smoker
    private void AddLog()
    {
        if (currentLogs < maxLogs)
        {
            currentLogs++;
            remainingBurnTime += burnTimePerLog; // Add burn time for the new log

            if (!isBurning)
            {
                StartCoroutine(BurnLogs()); // Start burning logs if not already burning
            }

            Debug.Log($"Log added! Current logs: {currentLogs}, Remaining burn time: {remainingBurnTime}");
        }
        else
        {
            Debug.Log("Smoker is full! Cannot add more logs.");
        }
    }

    // Coroutine to handle the burning of logs
    private IEnumerator BurnLogs()
    {
        isBurning = true;

        if (burnProgressBar != null)
        {
            burnProgressBar.Show();
        }

        while (remainingBurnTime > 0)
        {
            remainingBurnTime -= Time.deltaTime; // Decrease burn time

            if (burnProgressBar != null)
            {
                burnProgressBar.SetProgress(remainingBurnTime / (maxLogs * burnTimePerLog)); // Update burn progress bar
            }

            yield return null;
        }

        // When burn time runs out
        isBurning = false;
        currentLogs = 0;
        remainingBurnTime = 0f;

        if (burnProgressBar != null)
        {
            burnProgressBar.Hide(); // Hide the burn progress bar
        }

        Debug.Log("Logs have burned out!");

        // Pause the smoking progress bar if no logs are left
        if (processingItem && ProgressBar != null)
        {
            savedSmokingProgress = elapsedTime / processTime; // Save the current progress
        }
    }

    public override bool ProcessItem(GameObject ObjectToProcess)
    {
        Debug.Log("yah");
        CookingItem item = ObjectToProcess.GetComponent<CookingItem>();
        if (item != null && item.itemName == "Log") // Check if the item is a log
        {
            Debug.Log("adding log");
            AddLog(); // Handle logs separately
            Destroy(ObjectToProcess); // Destroy the log after adding it
        }
        else
        {
            return base.ProcessItem(ObjectToProcess); // Handle regular items using the base class logic
        }

        return true;
    }

    protected override IEnumerator ProcessItem()
    {
        // Accept the item into the smoker
        processingItem = true;

        if (ProgressBar != null)
        {
            ProgressBar.Show();
            ProgressBar.SetProgress(savedSmokingProgress); // Restore saved progress
        }

        // Wait until logs are burning to start processing
        while (!isBurning)
        {
            yield return null; // Wait until logs are added and burning
        }

        // Start processing the item
        elapsedTime = savedSmokingProgress * processTime; // Restore elapsed time from saved progress

        while (elapsedTime < processTime)
        {
            if (isBurning) // Only progress if logs are burning
            {
                elapsedTime += Time.deltaTime;

                if (ProgressBar != null)
                {
                    ProgressBar.SetProgress(elapsedTime / processTime); // Update progress bar
                }
            }

            yield return null;
        }

        // Item processing complete
        if (outputPrefab != null)
        {
            Debug.Log("Output Type is not null!");
            Instantiate(outputPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }

        if (ProgressBar != null)
        {
            ProgressBar.Hide(); // Hide the progress bar when done
        }

        processingItem = false;
        elapsedTime = 0f;
        savedSmokingProgress = 0f; // Reset saved progress
    }
}