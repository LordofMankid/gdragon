using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableLogic : MonoBehaviour
{
    public bool IsThrown { get; private set; } = false;
    public bool IsGrounded { get; private set; } = false;

    private Rigidbody rb;
    private bool throwDelay = false;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnThrown()
    {
        rb.isKinematic = false;
        IsThrown = true;
        gameObject.tag = "Untagged"; // Temporarily disable pickup

        StartCoroutine(EnablePickupAfterDelay(0.2f)); // 1 second delay before pickup is re-enabled
    }

    private IEnumerator EnablePickupAfterDelay(float delay)
    {
        throwDelay = true;
        yield return new WaitForSeconds(delay);
        throwDelay = false; // Re-enable pickup
        Debug.Log("Pickup re-enabled");
    }


    private void OnCollisionEnter(Collision collision)
    {

        if (IsThrown && (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2")) ) // Ensure your floor has the tag "Ground"
        {
            if (throwDelay)
            {
                return;
            }
            gameObject.tag = "PickUp";
            AutoPickup();
            IsThrown = false;
        } else
        {
            gameObject.tag = "PickUp";
            IsThrown = false;
        }
    }

    public void AutoPickup()
    {
        // Find all players in the scene
        GameObject[] players = FindObjectsWithTags(new string[] { "Player1", "Player2" });

        // Find the nearest player
        Transform nearestPlayer = null;
        float nearestDistance = float.MaxValue;
        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestPlayer = player.transform;
            }
        }

        // Assign the object to the nearest player
        if (nearestPlayer != null)
        {
            Player_Pickup playerPickup = nearestPlayer.GetComponent<Player_Pickup>();

            if (playerPickup != null && !playerPickup.HasItem && IsThrown && !throwDelay)
            {
                rb.velocity = Vector3.zero;  // Stop any movement
                rb.angularVelocity = Vector3.zero; // Stop rotation
                rb.isKinematic = true;
                rb.detectCollisions = false;
                playerPickup.PickUp_Object = transform;
                playerPickup.PickUp();
            }
        }
    }

    // Method to find objects with multiple tags
    private GameObject[] FindObjectsWithTags(string[] tags)
    {
        List<GameObject> result = new List<GameObject>();

        // Iterate through each tag and find objects
        foreach (string tag in tags)
        {
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);
            result.AddRange(objectsWithTag);
        }

        // Optionally, remove duplicates if an object has multiple tags
        HashSet<GameObject> uniqueObjects = new HashSet<GameObject>(result);
        return new List<GameObject>(uniqueObjects).ToArray();
    }
}
