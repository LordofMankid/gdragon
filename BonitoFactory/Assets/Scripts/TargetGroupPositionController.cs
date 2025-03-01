using UnityEngine;

public class CameraPivotController : MonoBehaviour
{
    public Transform targetGroupCenter;  // You can reference an empty GameObject whose position is set to the target group's center
    public Vector3 offset;               // Set this in the Inspector to control the pivot’s offset

    void Update()
    {
        if (targetGroupCenter != null)
            transform.position = targetGroupCenter.position + offset;
    }
}
