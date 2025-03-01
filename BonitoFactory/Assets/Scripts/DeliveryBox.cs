using UnityEngine;

public class DeliveryBox : MonoBehaviour
{
    private int fishCount;

    public void SetFishCount(int count)
    {
        fishCount = count;
        Debug.Log("Delivery Box spawned with " + fishCount + " fish.");
    }
}
