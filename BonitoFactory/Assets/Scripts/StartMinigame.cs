using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Apply this script to each minigame

public class StartMinigame : MonoBehaviour
{
    [SerializeField] Camera Player1Cam;
    [SerializeField] Camera Player2Cam;
    private Camera CurrentView;

    public KeyCode MinigameKey;

    [SerializeField] GameObject MinigamePrefab;
    private GameObject MinigameInstance;
    private bool playerInTriggerZone = false;


    void Awake()
    {
        Player1Cam = GameObject.FindGameObjectWithTag("Camera_P1").GetComponent<Camera>();
        Player2Cam = GameObject.FindGameObjectWithTag("Camera_P2").GetComponent<Camera>();
        if (Player1Cam == null || Player2Cam == null)
        {
            Debug.LogError("Cameras are not assigned properly!");
        }
    }

    void Update()
    {
        if (playerInTriggerZone && Input.GetKey(MinigameKey))
        {
            InitializeMinigame();
            playerInTriggerZone = false; // Reset flag
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1"))
        {
            playerInTriggerZone = true;
            CurrentView = Player1Cam;
        }

        if (other.CompareTag("Player2"))
        {
            playerInTriggerZone = true;
            CurrentView = Player2Cam;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (MinigameInstance != null)
        {
            Destroy(MinigameInstance);
            MinigameInstance = null;
        }
        CurrentView = null;
    }

    void InitializeMinigame()
    {
        if (MinigameInstance == null) // Prevent multiple instantiations
        {
            MinigameInstance = Instantiate(MinigamePrefab);
            Canvas canvas = MinigameInstance.GetComponentInChildren<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = CurrentView;
            MinigameInstance.SetActive(true);
        }
    }
}
