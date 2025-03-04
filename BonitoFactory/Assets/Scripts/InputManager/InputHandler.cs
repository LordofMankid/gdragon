using UnityEngine.InputSystem;
using UnityEngine;
using System;

public class InputHandler : MonoBehaviour
{
    // private Camera playerCam;

    // public static event Action OnStartGame;
    // public static event Action OnBid;
    // public static event Action OnResetGame;
    // public static event Action OnExitGame;

    // private void Awake()
    // {
    //     playerCam = GameObject.FindGameObjectWithTag("Camera_P1").GetComponent<Camera>();
    // }

    // public void OnClick(InputAction.CallbackContext context)
    // {
    //     if (!context.started) return;

    //     var rayHit = Physics2D.GetRayIntersection(playerCam.ScreenPointToRay(Mouse.current.position.ReadValue()));
    //     if (!rayHit.collider) return;

    //     HandleRaycastHit(rayHit.collider.gameObject);
    // }

    // private void HandleRaycastHit(GameObject hitObject)
    // {
    //     switch (hitObject.name)
    //     {
    //         case "StartGameBtn":
    //             OnStartGame?.Invoke();
    //             break;
    //         case "BidBtn":
    //             OnBid?.Invoke();
    //             break;
    //         case "PlayAgainBtn":
    //             OnResetGame?.Invoke();
    //             break;
    //         case "ExitGameBtn":
    //             OnExitGame?.Invoke();
    //         default:
    //             Debug.Log(hitObject.name);
    //             break;
    //     }
    // }
}
