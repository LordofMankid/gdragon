using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppingStation : ProcessingStation
{
    private bool isPaused = false;
    protected virtual void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E)) // Check for 'E' key press
        {
            if (isPaused) // Resume processing if paused
            {
                StartCoroutine(ProcessItem());
                isPaused = false;
            }
            else if (!processingItem) // Start processing if not already processing
            {
                // Check if the player is holding an item
                if (interactingPlayer != null)
                {
                    ThrowableLogic heldItem = interactingPlayer.GetComponentInChildren<ThrowableLogic>();
                    if (heldItem != null && itemNameMatches(heldItem.gameObject.GetComponent<CookingItem>()))
                    {
                        ProcessItem(heldItem.gameObject);
                    }
                }
            }
        }
    }
    // Start is called before the first frame update
    protected override IEnumerator ProcessItem()
    {
        processingItem = true;
        float processTime = 3f; // Adjust this for different processing times


        if (ProgressBar != null)
        {
            ProgressBar.Show();
            ProgressBar.SetProgress(elapsedTime / processTime); // Set progress based on elapsed time
        }



        while (elapsedTime < processTime)
        {
            if (!playerInRange) // If the player leaves, stop processing
            {
                processingItem = false;
                isPaused = true;
                yield break; // Exit the coroutine
            }

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
        isPaused = false;
        elapsedTime = 0f; // Reset elapsed time after processing is complete
    }
}
