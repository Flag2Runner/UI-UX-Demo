using _MyFiles.Scripts.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _MyFiles.Scripts.UI
{
    public class ShopItemUI : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        public ItemData item;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI costText;
        public Image iconDisplay;
        public Button purchaseButton;

        public System.Action<ItemData> OnPurchaseClicked;

        void Start()
        {
            if (nameText != null) nameText.text = item.itemName;
            if (costText != null) costText.text = $"{item.costAmount} {item.costCurrency.displayName}";
            if (iconDisplay != null && item.itemIcon != null) iconDisplay.sprite = item.itemIcon;
            purchaseButton.onClick.AddListener(() => OnPurchaseClicked?.Invoke(item));
        }
        
        // Inside ShopItemUI.cs
        public void Setup(ItemData newItemData)
        {
            item = newItemData; // Your ScriptableObject reference
            nameText.text = item.itemName;
            costText.text = $"{item.costAmount} : {item.costCurrency}";
            iconDisplay.sprite = item.itemIcon;
        }

        public void OnSelect(BaseEventData eventData) => transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        public void OnDeselect(BaseEventData eventData) => transform.localScale = Vector3.one;

        void OnDestroy() => purchaseButton.onClick.RemoveAllListeners();
    }
}