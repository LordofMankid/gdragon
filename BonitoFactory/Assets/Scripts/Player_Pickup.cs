using UnityEngine;
using System.Collections;
public class Player_Pickup : MonoBehaviour
{
    public Transform PickUp_Hand; // The hand where the object will be held
    private Transform PickUp_Object; // The object currently being held
    private Rigidbody PickUp_ObjectRigidbody; // Cached Rigidbody of the picked-up object

    public bool HasItem { get; private set; } = false; // Encapsulated field for better control
    public float ThrowForce = 10f; // Force applied when throwing the object

    private void Update()
    {
        HandlePickupInput();
        HandleThrowInput();
    }

    private void HandlePickupInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!HasItem && PickUp_Object != null)
            {
                PickUp();
            }
            else if (HasItem)
            {
                Drop();
            }
        }
    }

    private void HandleThrowInput()
    {
        if (HasItem && Input.GetKeyDown(KeyCode.Q)) // Use 'Q' for throwing
        {
            Throw();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickUp") && !HasItem)
        {
            PickUp_Object = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PickUp") && !HasItem)
        {
            PickUp_Object = null;
        }
    }

    private void PickUp()
    {
        if (PickUp_Object == null) return;

        // Attach the object to the hand
        PickUp_Object.position = PickUp_Hand.position;
        PickUp_Object.parent = PickUp_Hand;

        // Disable physics while holding the object
        PickUp_ObjectRigidbody = PickUp_Object.GetComponent<Rigidbody>();
        if (PickUp_ObjectRigidbody != null)
        {
            PickUp_ObjectRigidbody.isKinematic = true;
        }

        HasItem = true;
    }

    private void Drop()
    {
        if (PickUp_Object == null) return;

        // Detach the object from the hand
        PickUp_Object.parent = null;

        // Re-enable physics
        if (PickUp_ObjectRigidbody != null)
        {
            PickUp_ObjectRigidbody.isKinematic = false;
        }

        HasItem = false;
        PickUp_Object = null;
        PickUp_ObjectRigidbody = null;
    }

    private void Throw()
    {
        if (PickUp_Object == null) return;

        Drop(); // Drop the object first

        // Apply throw force
        if (PickUp_ObjectRigidbody != null)
        {
            PickUp_ObjectRigidbody.AddForce(transform.forward * ThrowForce, ForceMode.Impulse);
        }

        // Optionally, add logic to make the object pickable by other players
        StartCoroutine(MakeObjectPickableAfterDelay(PickUp_Object));
    }

    private IEnumerator MakeObjectPickableAfterDelay(Transform thrownObject)
    {
        // Disable pickup for a short time to avoid immediate re-pickup
        thrownObject.tag = "Untagged"; // Temporarily remove the "PickUp" tag
        yield return new WaitForSeconds(0.5f); // Adjust delay as needed
        thrownObject.tag = "PickUp"; // Re-enable pickup
    }
}