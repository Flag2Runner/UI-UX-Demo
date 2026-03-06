using _MyFiles.Scripts.UI;
using StarterAssets; // Required to access the StarterAssetsInputs script!
using UnityEngine;

namespace _MyFiles.Scripts
{
    public class ShopInteractable : MonoBehaviour
    {
        public ShopUIManager shopUI;
        private PlayerWallet _activePlayer = null;
        private StarterAssetsInputs _playerInputs = null; // New reference for the input system

        void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            
            _activePlayer = other.GetComponent<PlayerWallet>();
            _playerInputs = other.GetComponent<StarterAssetsInputs>(); // Grab the inputs!
        }

        void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            
            _activePlayer = null;
            _playerInputs = null; // Always clear references when they leave
            shopUI.CloseShop();
        }

        void Update()
        {
            // We now check if the player is present AND if the New Input System registered an interact press
            if (_activePlayer && _playerInputs && _playerInputs.interact)
            {
                // CRITICAL: Consume the input! 
                // If we don't set this back to false, the shop will try to open every single frame.
                _playerInputs.interact = false; 

                shopUI.OpenShop(_activePlayer);
            }
        }
    }
}