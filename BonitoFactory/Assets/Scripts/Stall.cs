using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stall : MonoBehaviour
{
    public GameObject inputPrefab;
    public GameObject ShopLogicTransform;

    protected float elapsedTime = 0f;
    protected ShopUI ShopMenu;

    // Information for player interaction
    protected bool playerInRange = false; // Track if a player is in range
    protected Transform interactingPlayer; // Reference to the interacting player

    protected virtual void Start()
    {
        ShopMenu = ShopLogicTransform.GetComponent<ShopUI>();
    }


    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            playerInRange = true;
            interactingPlayer = other.transform;
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

}
