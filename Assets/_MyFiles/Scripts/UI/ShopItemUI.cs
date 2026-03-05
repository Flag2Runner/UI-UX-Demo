using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

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

    public void OnSelect(BaseEventData eventData) => transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
    public void OnDeselect(BaseEventData eventData) => transform.localScale = Vector3.one;

    void OnDestroy() => purchaseButton.onClick.RemoveAllListeners();
}