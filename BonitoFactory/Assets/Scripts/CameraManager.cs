using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    public CinemachineVirtualCamera sharedVCam;
    public CinemachineVirtualCamera player1VCam;
    public CinemachineVirtualCamera player2VCam;
    public Camera mainCamera;
    public Camera mainCameraB;
    public float splitThreshold = 10f; // Distance at which split-screen activates
    public float fixedFOV = 60f; // Set a fixed FOV to prevent zoom changes
    public float fixedOrthographicSize = 5f; // If using Orthographic mode
    public GameObject Divider;

    public static CameraManager Instance;

    private bool isSplitScreen = false;


    public static event System.Action OnSplitScreenEnabled; // Event for split-screen enabled
    public static event System.Action OnSplitScreenDisabled; // Event for split-screen disabled

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        EnableSharedCamera();
    }

    void Update()
    {
        float distance = Vector3.Distance(player1.position, player2.position);

        if (distance > splitThreshold && !isSplitScreen)
        {
            EnableSplitScreen();
        }
        else if (distance <= splitThreshold && isSplitScreen)
        {
            EnableSharedCamera();
        }
    }

    void EnableSplitScreen()
    {
        isSplitScreen = true;

        sharedVCam.gameObject.SetActive(false);
        player1VCam.gameObject.SetActive(true);
        player2VCam.gameObject.SetActive(true);
        Divider.SetActive(true);
        // Set viewport for split-screen (Side-by-side example)
        mainCamera.rect = new Rect(0, 0, 0.5f, 1); // Left half
        mainCameraB.rect = new Rect(0.5f, 0, 0.5f, 1); // Right half

        // Fix zoom issues by setting a consistent FOV or orthographic size
        if (mainCamera.orthographic)
        {
            mainCamera.orthographicSize = fixedOrthographicSize;
            mainCameraB.orthographicSize = fixedOrthographicSize;
        }
        else
        {
            mainCamera.fieldOfView = fixedFOV;
            mainCameraB.fieldOfView = fixedFOV;
        }
        // notify listeners that split-screen is enabled
        OnSplitScreenEnabled?.Invoke();
    }

    void EnableSharedCamera()
    {
        isSplitScreen = false;

        sharedVCam.gameObject.SetActive(true);
        player1VCam.gameObject.SetActive(false);
        player2VCam.gameObject.SetActive(false);
        Divider.SetActive(false);
        // Reset viewport to full screen (Shared camera view)
        mainCamera.rect = new Rect(0, 0, 1, 1);
        mainCameraB.rect = new Rect(0, 0, 1, 1);

        // Restore zoom levels to match the shared camera
        if (mainCamera.orthographic)
        {
            mainCamera.orthographicSize = fixedOrthographicSize;
            mainCameraB.orthographicSize = fixedOrthographicSize;
        }
        else
        {
            mainCamera.fieldOfView = fixedFOV;
            mainCameraB.fieldOfView = fixedFOV;
        }

        // notify listeners that split-screen is disabled
        OnSplitScreenDisabled?.Invoke();
    }
}
