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
    public bool ShopIsOpen = false;

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

    protected bool itemNameMatches(CookingItem item)
    {
        return inputPrefab != null && item.itemName == inputPrefab.GetComponent<CookingItem>().itemName;
    }

    // open stall menu with player that opened it
    public void ToggleStallMenu(Player_Pickup player)
    {
        // checks for out of bounds and if the stall menu is in use
        if (!playerInRange || !interactingPlayer.CompareTag(player.tag))
        {
            return;
        }
        else
        {
            ShopIsOpen = !ShopIsOpen;
            if (ShopIsOpen)
            {
                ShopMenu.Show(player.CompareTag("Player1"));
            } else
            {
                ShopMenu.Hide();
            }
        }
    }
    
    public void HandleSale()
    {

        Player_Pickup pickup = interactingPlayer.GetComponent<Player_Pickup>();

        if (pickup != null)
        {
            if (pickup == null || !pickup.HasItem)
            {
                Debug.Log("No player or item to sell.");
                return;
            }
        }

        pickup.deleteItem();


        GameHandler.Instance.AddToBalance(500);
    }
}
