using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ProcessingStation : MonoBehaviour
{
    public GameObject inputPrefab;
    public GameObject outputPrefab;
    public GameObject ProgressBarTransform;


    
    public bool processingItem = false;
    public float timeLeft = 0f;
    private GameObject currentItem;
    private ProgressBarUILogic ProgressBar;

    private void Start()
    {
        ProgressBar = ProgressBarTransform.GetComponent<ProgressBarUILogic>();
    }
    // information for player interaction
    private bool playerInRange = false;
    private Transform interactingPlayer;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            playerInRange = true;
            interactingPlayer = other.transform;
        }
        ThrowableLogic thrownItem = other.gameObject.GetComponent<ThrowableLogic>();

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

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            playerInRange = false;
            interactingPlayer = null;
        }
    }


    public void ProcessItem(GameObject ObjectToProcess)
    {
        if (processingItem) return;

        CookingItem item = ObjectToProcess.GetComponent<CookingItem>();
        if (item != null && itemNameMatches(item))
        {
            processingItem = true;
            currentItem = item.gameObject;
            StartCoroutine(ProcessItem());
        }

        Destroy(ObjectToProcess);
    }

    private bool itemNameMatches(CookingItem item)
    {
        return inputPrefab != null && item.itemName == inputPrefab.GetComponent<CookingItem>().itemName;
    }

    private IEnumerator ProcessItem()
    {
        processingItem = true;

        if (ProgressBar != null)
        {
            ProgressBar.Show();
            ProgressBar.SetProgress(0);
        }

        float processTime = 3f; // Adjust this for different processing times
        float elapsedTime = 0f;

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
            Instantiate(outputPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }

        if (ProgressBar != null)
        {
            ProgressBar.Hide(); // Hide the progress bar when done
        }
        processingItem = false;
    }


}


//