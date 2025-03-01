using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public Sprite popupSprite;
    public bool inTriggerRange = false;

    private void OnTriggerEnter(Collider other)
    {
        inTriggerRange = true;

        if (other.CompareTag("Player1") || other.CompareTag("Player2")) // Ensure Player1 has the tag "Player"
        {
            GameObject player = other.gameObject;
            Transform popupIcon = player.transform.Find("PopupIcon");

            int popupIconLength = popupIcon.childCount;
            // Debug.Log("PopupIcon has " + popupIconLength + " children.");

            if (popupIcon != null)
            {
                foreach (Transform child in popupIcon)
                {
                    if (child.CompareTag("PopupImagePlaceholder"))
                    {
                        // Debug.Log("Found placeholder object");
                        SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                        if (spriteRenderer != null)
                        {
                            spriteRenderer.sprite = popupSprite;
                        }
                    }
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        inTriggerRange = false;
    }
}
