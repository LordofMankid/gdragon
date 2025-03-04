using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public Canvas CanvasA;
    public Canvas CanvasB;
    public Button sellButton; // Reference to the Sell button
    public Button cancelButton; // Reference to the Cancel button
    public Button sellButtonB;
    public Button cancelButtonB;

    private Stall attachedStall;

    void Start()
    {
        Transform parent = transform.parent;

        if (parent != null)
        {
            attachedStall = parent.GetComponentInChildren<Stall>();
            if (attachedStall == null)
            {
                Debug.LogError("No Stall for this UI");
            }
        }

        // Set up button listeners
        sellButton.onClick.AddListener(OnSellButtonClicked);
        sellButtonB.onClick.AddListener(OnSellButtonClicked);
        cancelButton.onClick.AddListener(OnCancelButtonClicked);
        cancelButtonB.onClick.AddListener(OnCancelButtonClicked);


        this.Hide();
    }

    public void Show()
    {
        if (CanvasA != null) CanvasA.gameObject.SetActive(true);
        if (CanvasB != null) CanvasB.gameObject.SetActive(true);

        // Set initial focus to the Sell button
        EventSystem.current.SetSelectedGameObject(sellButton.gameObject);
    }

    public void Hide()
    {
        if (CanvasA != null) CanvasA.gameObject.SetActive(false);
        if (CanvasB != null) CanvasB.gameObject.SetActive(false);
    }

    private void OnSellButtonClicked()
    {
        Debug.Log("hi");
        attachedStall.HandleSale();
    }

    private void OnCancelButtonClicked()
    {
        this.Hide();
    }
}