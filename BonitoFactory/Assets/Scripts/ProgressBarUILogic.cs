using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUILogic : MonoBehaviour
{
    public Image ProgressFill;
    public Image ProgressFillB;

    // Start is called before the first frame update
    public void Start()
    {
        this.Hide();
    }
    public void Show()
    {
        if(ProgressFill != null)
        {
            ProgressFill.transform.parent.gameObject.SetActive(true);
        }

        if(ProgressFillB != null)
        {
            ProgressFillB.transform.parent.gameObject.SetActive(true);
        }

    }

    public void Hide()
    {
        if (ProgressFill != null)
        {
            ProgressFill.transform.parent.gameObject.SetActive(false);
        }

        if (ProgressFillB != null)
        {
            ProgressFillB.transform.parent.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// Updates the progress bar fill amount (value should be between 0.0 and 1.0f).
    /// </summary>
    public void SetProgress(float progress)
    {
        if (ProgressFill != null)
        {
            ProgressFill.fillAmount = Mathf.Clamp01(progress);
        }

        if (ProgressFillB != null)
        {
            ProgressFillB.fillAmount = Mathf.Clamp01(progress);
        }
    }
}
