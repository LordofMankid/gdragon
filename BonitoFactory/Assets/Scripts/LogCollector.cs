using UnityEngine;
using UnityEngine.UI;

public class LogCollector : MonoBehaviour
{
    public Text logCountText; // UI Text for collected logs
    private int logCount = 0; // Stores collected logs

    void Start()
    {
        UpdateLogUI();
    }

    public void CollectLog()
    {
        Debug.Log("Collect Button Pressed!"); // Shows button is working

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);
        Debug.Log("Nearby Objects: " + colliders.Length); // Shows how many objects are near

        foreach (Collider2D collider in colliders)
        {
            Transform logTransform = collider.transform; // Get the detected object's transform

            // If it's a child object, go to its parent
            if (logTransform.parent != null)
            {
                logTransform = logTransform.parent;
            }

            Debug.Log("Found Object: " + logTransform.gameObject.name); // Print detected object

            if (logTransform.CompareTag("Log")) // Ensure it's checking the Log object
            {
                logCount++;
                Debug.Log("Log Collected! New Count: " + logCount);
                UpdateLogUI();
                Destroy(logTransform.gameObject); // Destroy the entire Log prefab
                break;
            }
        }
    }




    void UpdateLogUI()
    {
        logCountText.text = "Logs: " + logCount;
    }
}
