using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Log_ProcessingStation : MonoBehaviour
{
    public GameObject outputPrefab;
    public GameObject ProgressBarTransform;
    public bool processingItem = false;
    protected float elapsedTime = 0f;
    protected ProgressBarUILogic ProgressBar;
    private bool startProcessing = false;
    protected Transform interactingPlayer;
    private bool playerInRange = false;

    protected virtual void Start()
    {
        ProgressBar = ProgressBarTransform.GetComponent<ProgressBarUILogic>();
    }

    protected virtual void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("L was pressed");
            if (!processingItem)
            {
                ProcessItem();
            }
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            interactingPlayer = other.transform;
            playerInRange = true;
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            interactingPlayer = null;
            playerInRange = false;
        }
    }

    public virtual void ProcessItem()
    {
        if (processingItem) return;
        processingItem = true;
        elapsedTime = 0f;
        StartCoroutine(ProcessItemCoroutine());

        if (interactingPlayer != null)
        {
            Player_Pickup playerPickup = interactingPlayer.GetComponent<Player_Pickup>();
            if (playerPickup != null)
            {
                playerPickup.deleteItem(); // Clear the held item reference in the player script
            }
        }
    }

    protected virtual IEnumerator ProcessItemCoroutine()
    {
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
                ProgressBar.SetProgress(elapsedTime / processTime);
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
            ProgressBar.Hide();
        }

        processingItem = false;
        elapsedTime = 0f;
    }
}