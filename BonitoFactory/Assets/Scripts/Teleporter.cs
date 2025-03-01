using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
public class Teleporter : MonoBehaviour
{
    public GameObject playerController;
    public Transform playerPosition;
    public bool inTriggerRange = false;

    [SerializeField] Transform MarketTeleporter;
    [SerializeField] Transform HomeTeleporter;

    void Awake()
    {
        if (gameObject.CompareTag("Teleporter-Home"))
        {
            HomeTeleporter = gameObject.transform;
            MarketTeleporter = GameObject.FindGameObjectWithTag("Teleporter-Market").transform;
        }
        else
        {
            HomeTeleporter = GameObject.FindGameObjectWithTag("Teleporter-Home").transform;
            MarketTeleporter = gameObject.transform;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Debug.Log("On Trigger Enter");
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            inTriggerRange = true;
            playerPosition = other.transform;
            playerController = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Debug.Log("On Trigger Exit");
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            inTriggerRange = false;
            playerPosition = null;
            playerController = null;
        }
    }

    void Update()
    {
        if (inTriggerRange && playerPosition && playerController)
        {
            // Debug.Log("Player in teleport zone");
            if (Input.GetKey(KeyCode.M) && gameObject.CompareTag("Teleporter-Home"))
            {
                Debug.Log("M Key Pressed");
                StartCoroutine(Teleport(MarketTeleporter));
            }
            else if (Input.GetKeyDown(KeyCode.T) && gameObject.CompareTag("Teleporter-Market"))
            {
                Debug.Log("T Key Pressed");
                StartCoroutine(Teleport(HomeTeleporter));
            }
        }
    }

    IEnumerator Teleport(Transform destination)
    {
        if (destination == null)
        {
            Debug.LogError("Teleport destination not found!");
            yield break;
        }

        Debug.Log("Teleporting...");
        GameObject popupField = playerController.transform.Find("PopupIcon").gameObject;
        popupField.SetActive(false);
        playerController.SetActive(false);
        yield return null;
        playerPosition.position = destination.position + new Vector3(5f, 0, 5f);
        yield return null;
        playerController.SetActive(true);
    }
}
