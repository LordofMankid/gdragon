using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public int currentCurrency = 1000;
    public TextMeshProUGUI currencyText;

    void Start()
    {
        UpdateCurrencyUI();
    }

    public void DeductCurrency(int amount)
    {
        if (currentCurrency >= amount)
        {
            currentCurrency -= amount;
            UpdateCurrencyUI();
        }
        else
        {
            Debug.Log("Not enough currency!");
        }
    }

    public void AddCurrency(int amount)
    {
        currentCurrency += amount;
        UpdateCurrencyUI();
    }

    private void UpdateCurrencyUI()
    {
        if (currencyText != null)
        {
            currencyText.text = "Coins: " + currentCurrency;
        }
        else
        {
            Debug.LogError("CurrencyText UI not assigned!");
        }
    }
}
