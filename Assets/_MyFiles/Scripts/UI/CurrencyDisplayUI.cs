using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurrencyDisplayUI : MonoBehaviour
{
    public Image currencyIcon;
    public TextMeshProUGUI amountText;

    public void Setup(Currency currencyData, int amount)
    {
        if (currencyIcon != null && currencyData.icon != null) currencyIcon.sprite = currencyData.icon;
        if (amountText != null) amountText.text = amount.ToString();
    }
}