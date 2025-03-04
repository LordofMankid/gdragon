using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ProcessingStation : MonoBehaviour
{
    public GameObject inputPrefab;
    public GameObject outputPrefab;
    public GameObject ProgressBarTransform;



    public bool processingItem = false;
    protected float elapsedTime = 0f;
    protected GameObject currentItem;
    protected ProgressBarUILogic ProgressBar;

    // Information for player interaction
    protected bool playerInRange = false; // Track if a player is in range
    protected Transform interactingPlayer; // Reference to the interacting player

    protected virtual void Start()
    {
        ProgressBar = ProgressBarTransform.GetComponent<ProgressBarUILogic>();
    }


    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            playerInRange = true;
            interactingPlayer = other.transform;
        }
        ThrowableLogic thrownItem = other.gameObject.GetComponent<ThrowableLogic>();
        Debug.Log("thrownItem is here");
        if (thrownItem != null && thrownItem.IsThrown) // Only handle thrown items
        {
            if (itemNameMatches(thrownItem.gameObject.GetComponent<CookingItem>()))
            {
                ProcessItem(thrownItem.gameObject); // Process it
            }
            else
            {
                // If incorrect, do nothing (it just bounces off naturally)
                Debug.Log("Incorrect item! Bouncing off...");
            }
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            playerInRange = false;
            interactingPlayer = null;
        }
    }


    public virtual void ProcessItem(GameObject ObjectToProcess)
    {
        if (processingItem) return;

        CookingItem item = ObjectToProcess.GetComponent<CookingItem>();
        if (item != null && itemNameMatches(item))
        {
            processingItem = true;
            currentItem = item.gameObject;
            elapsedTime = 0f;
            StartCoroutine(ProcessItem());
            Destroy(ObjectToProcess);
        }


    }

    protected bool itemNameMatches(CookingItem item)
    {
        Debug.Log(item.itemName);
        return inputPrefab != null && item.itemName == inputPrefab.GetComponent<CookingItem>().itemName + "(Clone)" || item.itemName == inputPrefab.GetComponent<CookingItem>().itemName;
    }

    protected virtual IEnumerator ProcessItem()
    {
        processingItem = true;

        if (ProgressBar != null)
        {
            ProgressBar.Show();
            ProgressBar.SetProgress(0);
        }

        float processTime = 3f; // Adjust this for different processing times

        while (elapsedTime < processTime)
        {
            elapsedTime += Time.deltaTime;

            if (ProgressBar != null)
            {
                ProgressBar.SetProgress(elapsedTime / processTime); // Update progress bar
            }


            yield return null;
        }

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
    }


}


//