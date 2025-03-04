using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public Sprite PopupSprite;
    public Transform PopupObject;

    public bool inTriggerRange = false;

    private void OnTriggerEnter(Collider other)
    {
        inTriggerRange = true;

        if (other.CompareTag("Player1") || other.CompareTag("Player2")) // Ensure Player1 has the tag "Player"
        {
            GameObject player = other.gameObject;
            PopupObject = player.transform.Find("PopupIcon");

            int popupIconLength = PopupObject.childCount;
            foreach (Transform child in PopupObject)
            {
                if (child.CompareTag("PopupImagePlaceholder"))
                {
                    SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                    {
                        spriteRenderer.sprite = PopupSprite;
                    }
                }
            }
            ShowPopup();
        }
    }

    void OnTriggerExit(Collider other)
    {
        inTriggerRange = false;
        HidePopup();
    }

    void ShowPopup()
    {
        if (PopupObject != null)
        {
            PopupObject.gameObject.SetActive(true);
        }
    }

    void HidePopup()
    {
        if (PopupObject != null)
        {
            PopupObject.gameObject.SetActive(false);
        }
    }

}
