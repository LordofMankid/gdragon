using UnityEngine;

public class DeliveryBox : MonoBehaviour
{
    public GameObject throwableFishPrefab; // Assign a fish prefab that inherits from ThrowableLogic
    private int fishCount = 5; // Number of fish stored in the box
    private bool playerInRange = false;
    private Transform interactingPlayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            playerInRange = true;
            interactingPlayer = other.transform;
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

    private void Update()
    {
        // Check if player is in range and presses 'E'
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Player_Pickup playerPickup = interactingPlayer.GetComponent<Player_Pickup>();

            SpawnFish(interactingPlayer);
        }
    }

    private void SpawnFish(Transform player)
    {
        if (fishCount <= 0) return; // No more fish to spawn

        // Instantiate the throwable fish
        GameObject fish = Instantiate(throwableFishPrefab, transform.position + Vector3.up * 1f, Quaternion.identity);

        // Assign it to the player�s hand
        Player_Pickup playerPickup = player.GetComponent<Player_Pickup>();
        if (playerPickup != null && !playerPickup.HasItem)
        {
            playerPickup.StartCoroutine(playerPickup.StartInteractionCooldown());
            playerPickup.PickUp_Object = fish.transform;
            playerPickup.PickUp();
        }

        // Decrease fish count
        fishCount--;

        // Destroy the box if no fish remain
        if (fishCount <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetFishCount(int count)
    {
        fishCount = count;
    }
}
