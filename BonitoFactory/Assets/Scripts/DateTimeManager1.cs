using UnityEngine;
using UnityEngine.UI;

public class DateTimeManager : MonoBehaviour
{
    public Text dateTimeText; // Legacy UI Text

    void Start()
    {
        InvokeRepeating("UpdateDateTime", 0f, 1f); // Update every second
    }

    void UpdateDateTime()
    {
        if (dateTimeText != null)
        {
            dateTimeText.text = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        else
        {
            Debug.LogError("DateTimeText not assigned in Inspector!");
        }
    }
}
