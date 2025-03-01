using UnityEngine;
using System.Collections;
public class Player_Pickup : MonoBehaviour
{
    public Transform PickUp_Hand; // The hand where the object will be held
    public Transform PickUp_Object; // The object currently being held
    private Rigidbody PickUp_ObjectRigidbody; // Cached Rigidbody of the picked-up object

    public GameObject Popup;

    public bool HasItem { get; private set; } = false; // Encapsulated field for better control
    public float ThrowForce = 12f; // Force applied when throwing the object

    public bool IsAiming { get; private set; } = false; // tracks when player is aiming
    private bool canInteract = true;
    private void Awake()
    {
        if (Popup != null)
        {
            Popup.SetActive(false); // Hide popup by default
        }
    }

    private void Update()
    {
        HandlePickupInput();
        HandleThrowInput();
    }

    void ShowPopup()
    {
        // Vector3 popupPosition = PickUp_Object.position + new Vector3(-4, 1, 3); // Adjust xDelta and zDelta as needed
        // Popup.transform.position = popupPosition; // Set the Popup position
        // Popup.transform.SetParent(PickUp_Object); // Set Popup as a child of the PickUp_Object
        Popup.SetActive(true);
    }

    void HidePopup()
    {
        Popup.SetActive(false);
    }

    private void HandlePickupInput()
    {
        if (Input.GetKeyDown(KeyCode.E) && canInteract)
        {

            if (!HasItem && PickUp_Object != null)
            {
                HidePopup();
                PickUp();

            }
            else if (HasItem)
            {
                Debug.Log("dropping");
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

        if (HasItem && Input.GetKeyUp(KeyCode.Q))
        {
            Throw();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickUp") && !HasItem)
        {

            PickUp_Object = other.transform;
            ShowPopup();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PickUp") && !HasItem)
        {
            HidePopup();
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

    public IEnumerator StartInteractionCooldown()
    {
        canInteract= false;
        yield return new WaitForSeconds(0.2f); // Small delay to prevent instant dropping
        canInteract= true;
    }


}