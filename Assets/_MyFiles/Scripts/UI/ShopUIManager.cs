using System;
using System.Collections.Generic;
using _MyFiles.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

namespace _MyFiles.Scripts.UI
{
    public class ShopUIManager : MonoBehaviour
    {
        [Header("UI References")]
        public CurrencyDisplayUI walletDisplayPrefab;
        public Transform walletUIContainer;
        public Transform shopContentContainer;
        
        [Header("Shop Generation")]
        public ShopItemUI itemPrefab; // Drag your ItemUI prefab here!
        public ItemData[] itemsToSell; // Designers drop ScriptableObjects in this list

        //track spawned items to prevent duplicates
        private readonly Dictionary<ItemData, ShopItemUI> _spawnedItems = new Dictionary<ItemData, ShopItemUI>();
    
        //keep a list of the actual UI buttons so we know what to select for the controller
        private readonly List<ShopItemUI> _activeItemButtons = new List<ShopItemUI>();

        [Header("Audio")]
        public AudioSource audioSource;
        public AudioClip successSound;
        public AudioClip errorSound;

        [Header("InputReference")]
        private PlayerInput _playerInput;
        
        private ShopItemUI[] _allShopItems;

        private readonly Dictionary<Currency, CurrencyDisplayUI> _spawnedWalletUI = new Dictionary<Currency, CurrencyDisplayUI>();
        private PlayerWallet _currentPlayerWallet;

        void Awake()
        {
            _playerInput = Object.FindFirstObjectByType<PlayerInput>();
            if (_playerInput == null) Debug.LogError("PlayerInput component not found!");
        
            ShowCursorAndUnLock();
        
            // CACHE IT HERE: We find them exactly once when the game loads.
            _allShopItems = shopContentContainer.GetComponentsInChildren<ShopItemUI>(true);
        
            foreach (ShopItemUI itemUI in _allShopItems) 
            {
                itemUI.OnPurchaseClicked += AttemptPurchase;
            }
        
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            ShowCursorAndUnLock();
            _playerInput.SwitchCurrentActionMap("UI");
            Time.timeScale = 0f; // Paused the game when in UI.
        }

        private void OnDisable()
        {
            HideCursorAndLock();
            _playerInput.SwitchCurrentActionMap("Player");
            Time.timeScale = 1f; //UnPaused the game when out of UI.
        }

        public void OpenShop(PlayerWallet player)
        {
            _currentPlayerWallet = player;
            gameObject.SetActive(true);
            RefreshWalletUI();
        
            // Spawn the items!
            GenerateShopItems();

            // Dynamically select the very first spawned item for the controller
            if (_activeItemButtons.Count > 0)
            {
                EventSystem.current.SetSelectedGameObject(_activeItemButtons[0].purchaseButton.gameObject);
            }
        }
        public void CloseShop()
        {
            gameObject.SetActive(false);
            _currentPlayerWallet = null;
        }
        
        private void GenerateShopItems()
        {
            foreach (ItemData itemToSell in itemsToSell)
            {
                // If we already spawned this item, skip it!
                if (_spawnedItems.ContainsKey(itemToSell)) continue;

                // 1. Spawn the prefab inside the Content Container
                ShopItemUI newItemUI = Instantiate(itemPrefab, shopContentContainer);

                // 2. Inject the ScriptableObject data into the UI
                newItemUI.Setup(itemToSell);

                // 3. Subscribe to the purchase event
                newItemUI.OnPurchaseClicked += AttemptPurchase;

                // 4. Add it to our trackers
                _spawnedItems.Add(itemToSell, newItemUI);
                _activeItemButtons.Add(newItemUI);
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void RefreshWalletUI()
        {
            foreach (CurrencyBalance balance in _currentPlayerWallet.balances)
            {
                if (_spawnedWalletUI.TryGetValue(balance.currencyType, out CurrencyDisplayUI existingUI))
                {
                    existingUI.Setup(balance.currencyType, balance.amount);
                }
                else
                {
                    // GUARD CLAUSE: Prevent the game from crashing if the prefab is missing!
                    if (!walletDisplayPrefab)
                    {
                        Debug.LogError("CRITICAL: Wallet Display Prefab is missing in the Inspector! Cannot spawn UI.");
                        return; // Stop running this method, but keep the game alive!
                    }

                    CurrencyDisplayUI newDisplay = Instantiate(walletDisplayPrefab, walletUIContainer);
                    newDisplay.Setup(balance.currencyType, balance.amount);
                    _spawnedWalletUI.Add(balance.currencyType, newDisplay);
                }
            }
        }

        private void AttemptPurchase(ItemData purchasedItem)
        {
            if (_currentPlayerWallet == null) return;
            if (_currentPlayerWallet.TryPurchase(purchasedItem))
            {
                audioSource.PlayOneShot(successSound);
                RefreshWalletUI();
            }
            else audioSource.PlayOneShot(errorSound);
        }

        //Called to show the Ui and Unlock the mouse
        public void ShowCursorAndUnLock()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        //Called to hide the UI and Lock the mouse back to the center.
        public void HideCursorAndLock()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}