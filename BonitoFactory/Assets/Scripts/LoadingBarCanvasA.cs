using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingBarCanvasA : MonoBehaviour
{
    public Canvas canvasA; // Reference to Canvas A

    void Start()
    {
        if (CameraManager.Instance != null && canvasA != null)
        {
            canvasA.worldCamera = CameraManager.Instance.mainCamera;
        }
    }
}
