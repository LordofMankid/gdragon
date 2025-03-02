using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessingStation : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject inputPrefab;
    public GameObject outputPrefab;

    public bool processingItem = false;
    public float timeLeft = 0f;
    private GameObject currentItem;

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
    }

    private bool itemNameMatches(CookingItem item)
    {
        return inputPrefab != null && item.itemName == inputPrefab.GetComponent<CookingItem>().itemName;
    }

    private IEnumerator ProcessItem()
    {

        // Destroy the input item
        Destroy(currentItem);
        yield return new WaitForSeconds(3f); // Simulates cooking time



        // Spawn the output item
        if (outputPrefab != null)
        {
            Debug.Log("creating item");
            Instantiate(outputPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }

        processingItem = false;
    }


}


//