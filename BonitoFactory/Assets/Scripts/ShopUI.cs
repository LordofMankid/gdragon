using System;
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

    private string submitButton; // Input axis for Submit (e.g., "Submit_P1")
    private string cancelButtonInput; // Input axis for Cancel (e.g., "Cancel_P1")

    public bool isP1 = true;
    public bool isP2 = true;
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

        // Determine which player this UI belongs to
        if (isP1)
        {
            submitButton = "P1_Submit";
            cancelButtonInput = "P1_Cancel";
        }
        else if (isP2)
        {
            submitButton = "P2_Submit";
            cancelButtonInput = "P2_Cancel";
        }
        else
        {
            submitButton = "Submit";
            cancelButtonInput = "Cancel";
        }

        // Set up button listeners
        sellButton.onClick.AddListener(OnSellButtonClicked);
        sellButtonB.onClick.AddListener(OnSellButtonClicked);
        cancelButton.onClick.AddListener(OnCancelButtonClicked);
        cancelButtonB.onClick.AddListener(OnCancelButtonClicked);


        this.Hide();
    }

        private void Update()
    {
        // Handle controller input
        if (Input.GetButtonDown(submitButton))
        {
            // Trigger the currently selected button
            EventSystem.current.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
        }
        else if (Input.GetButtonDown(cancelButtonInput))
        {
            // Trigger the Cancel button
            cancelButton.onClick.Invoke();
        }
    }
    public void Show(bool isPlayer1)
    {
        Debug.Log(isPlayer1);
        isP1 = isPlayer1;

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