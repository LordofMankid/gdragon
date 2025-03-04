using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasB : MonoBehaviour
{
    public Canvas canvasB;

    private void Start()
    {
        CameraManager.OnSplitScreenEnabled += EnableCanvasB;
        CameraManager.OnSplitScreenDisabled += DisableCanvasB;
    }
    private void OnEnable()
    {
        // Subscribe to events
        CameraManager.OnSplitScreenEnabled += EnableCanvasB;
        CameraManager.OnSplitScreenDisabled += DisableCanvasB;
    }

    private void OnDisable()
    {
        // Unsubscribe from events
        CameraManager.OnSplitScreenEnabled -= EnableCanvasB;
        CameraManager.OnSplitScreenDisabled -= DisableCanvasB;
    }
    // Start is called before the first frame update

    private void EnableCanvasB()
    {
        if (CameraManager.Instance != null && canvasB != null)
        {
            canvasB.worldCamera = CameraManager.Instance.mainCameraB;
            canvasB.gameObject.SetActive(true); // Enable Canvas B
        }
    }

    private void DisableCanvasB()
    {
        if (canvasB != null)
        {
            canvasB.gameObject.SetActive(false); // Disable Canvas B
        }
    }
}
