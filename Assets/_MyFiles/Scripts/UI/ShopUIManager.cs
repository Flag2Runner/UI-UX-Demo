using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopUIManager : MonoBehaviour
{
    [Header("UI References")]
    public CurrencyDisplayUI walletDisplayPrefab;
    public Transform walletUIContainer;
    public Transform shopContentContainer;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip successSound;
    public AudioClip errorSound;

    private Dictionary<Currency, CurrencyDisplayUI> spawnedWalletUI = new Dictionary<Currency, CurrencyDisplayUI>();
    private PlayerWallet currentPlayerWallet;

    void Awake()
    {
        ShopItemUI[] allShopItems = shopContentContainer.GetComponentsInChildren<ShopItemUI>();
        foreach (ShopItemUI itemUI in allShopItems) itemUI.OnPurchaseClicked += AttemptPurchase;
        gameObject.SetActive(false);
    }

    public void OpenShop(PlayerWallet player)
    {
        currentPlayerWallet = player;
        gameObject.SetActive(true);
        RefreshWalletUI();

        if (shopContentContainer.childCount > 0)
        {
            EventSystem.current.SetSelectedGameObject(shopContentContainer.GetChild(0).gameObject);
        }
    }

    public void CloseShop()
    {
        gameObject.SetActive(false);
        currentPlayerWallet = null;
    }

    private void RefreshWalletUI()
    {
        foreach (CurrencyBalance balance in currentPlayerWallet.balances)
        {
            if (spawnedWalletUI.TryGetValue(balance.currencyType, out CurrencyDisplayUI existingUI))
            {
                existingUI.Setup(balance.currencyType, balance.amount);
            }
            else
            {
                CurrencyDisplayUI newDisplay = Instantiate(walletDisplayPrefab, walletUIContainer);
                newDisplay.Setup(balance.currencyType, balance.amount);
                spawnedWalletUI.Add(balance.currencyType, newDisplay);
            }
        }
    }

    private void AttemptPurchase(ItemData purchasedItem)
    {
        if (currentPlayerWallet == null) return;
        if (currentPlayerWallet.TryPurchase(purchasedItem))
        {
            audioSource.PlayOneShot(successSound);
            RefreshWalletUI();
        }
        else audioSource.PlayOneShot(errorSound);
    }
}