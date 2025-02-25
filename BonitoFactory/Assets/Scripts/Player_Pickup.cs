using UnityEngine;
using System.Collections;
public class Player_Pickup : MonoBehaviour
{
    public Transform PickUp_Hand; // The hand where the object will be held
    public Transform PickUp_Object; // The object currently being held
    private Rigidbody PickUp_ObjectRigidbody; // Cached Rigidbody of the picked-up object

    public bool HasItem { get; private set; } = false; // Encapsulated field for better control
    public float ThrowForce = 12f; // Force applied when throwing the object

    public bool IsAiming { get; private set; } = false; // tracks when player is aiming
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
        if (HasItem && Input.GetKey(KeyCode.Q))
        {
            IsAiming = true;
        }
        else
        {
            IsAiming = false;
        }

        if (HasItem && Input.GetKeyUp(KeyCode.Q)) // Use 'Q' for throwing
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

    public void PickUp()
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
            PickUp_ObjectRigidbody.AddForce(transform.forward * 5, ForceMode.Impulse);
        }


        HasItem = false;
        PickUp_Object = null;
        PickUp_ObjectRigidbody = null;
    }

    private void Throw()
    {
        if (PickUp_Object == null) return;

        // Detach the object from the hand
        PickUp_Object.parent = null;

        // Re-enable physics and apply stronger throw force
        if (PickUp_ObjectRigidbody != null)
        {
            PickUp_ObjectRigidbody.isKinematic = false;
            float upwardForceRatio = 0.2f;
            Vector3 throwDirection = transform.forward + Vector3.up * upwardForceRatio;
            PickUp_ObjectRigidbody.AddForce(throwDirection * ThrowForce, ForceMode.Impulse);
        }

        // Optionally, add logic to make the object pickable by other players
        //StartCoroutine(MakeObjectPickableAfterDelay(PickUp_Object));
        ThrowableLogic thrownObject = PickUp_Object.GetComponent<ThrowableLogic>();
        if (thrownObject != null)
        {
            thrownObject.OnThrown();
        }
        HasItem = false;
        PickUp_Object = null;
        PickUp_ObjectRigidbody = null;
    }

    private IEnumerator MakeObjectPickableAfterDelay(Transform thrownObject)
    {
        // Disable pickup for a short time to avoid immediate re-pickup
        thrownObject.tag = "Untagged"; // Temporarily remove the "PickUp" tag
        yield return new WaitForSeconds(0.5f); // Adjust delay as needed
        thrownObject.tag = "PickUp"; // Re-enable pickup
    }
}