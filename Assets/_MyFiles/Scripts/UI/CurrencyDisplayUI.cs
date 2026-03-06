using _MyFiles.Scripts.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _MyFiles.Scripts.UI
{
    public class CurrencyDisplayUI : MonoBehaviour
    {
        public Image currencyIcon;
        public TextMeshProUGUI amountText;

        public void Setup(Currency currencyData, int amount)
        {
            if (currencyIcon && currencyData.icon) currencyIcon.sprite = currencyData.icon;
            if (amountText) amountText.text = amount.ToString();
        }
    }
}