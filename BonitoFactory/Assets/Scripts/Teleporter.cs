using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
public class Teleporter : MonoBehaviour
{
    public GameObject playerController;
    public Transform player;
    public bool inTriggerRange = false;

    [SerializeField] Transform MarketTeleporter;
    [SerializeField] Transform HomeTeleporter;
    [SerializeField] Transform spawnLocation;


    void Awake()
    {
        if (gameObject.CompareTag("Teleporter-Home"))
        {
            HomeTeleporter = gameObject.transform;
            spawnLocation = GameObject.FindGameObjectWithTag("MarketSpawn").transform;
            MarketTeleporter = GameObject.FindGameObjectWithTag("Teleporter-Market").transform;
        }
        else
        {
            HomeTeleporter = GameObject.FindGameObjectWithTag("Teleporter-Home").transform;
            spawnLocation = GameObject.FindGameObjectWithTag("HomeSpawn").transform;
            MarketTeleporter = gameObject.transform;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Debug.Log("On Trigger Enter");
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            inTriggerRange = true;
            player = other.transform;
            playerController = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Debug.Log("On Trigger Exit");
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            inTriggerRange = false;
            player = null;
            playerController = null;
        }
    }

    void Update()
    {
        if (inTriggerRange && player && playerController)
        {
            // Debug.Log("Player in teleport zone");
            if (Input.GetKey(KeyCode.M) && gameObject.CompareTag("Teleporter-Home"))
            {
                Debug.Log("M Key Pressed");
                StartCoroutine(Teleport(MarketTeleporter));
            }
            else if (Input.GetKeyDown(KeyCode.H) && gameObject.CompareTag("Teleporter-Market"))
            {
                Debug.Log("H Key Pressed");
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
        player.position = spawnLocation.position;
        yield return null;
        playerController.SetActive(true);
    }
}
