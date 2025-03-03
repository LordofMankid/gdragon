using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableResourceModel : MonoBehaviour
{
    public Transform modelParent; // Parent for the actual model
    public GameObject defaultCube; // Default cube placeholder

    private void Start()
    {
        CheckAndAssignModel();
    }

    private void CheckAndAssignModel()
    {
        // Check if modelParent has any active children (custom models)
        bool hasCustomModel = false;
        foreach (Transform child in modelParent)
        {
            if (child.gameObject.activeSelf)
            {
                hasCustomModel = true;
                break;
            }
        }

        // If no custom model exists, enable default cube
        if (!hasCustomModel)
        {
            if (defaultCube != null)
                defaultCube.SetActive(true);
        }
        else
        {
            if (defaultCube != null)
                defaultCube.SetActive(false);
        }
    }
}
