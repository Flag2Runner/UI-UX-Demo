using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _MyFiles.Scripts.UI
{
    public class GameStateManager : MonoBehaviour
    {
        // A Singleton so any script in your game can easily access it
        public static GameStateManager Instance { get; private set; }

        private PlayerInput _playerInput;
        private StarterAssetsInputs _starterInputs;

        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(this);
            else Instance = this;

            // Find the player's input components
            _playerInput = FindFirstObjectByType<PlayerInput>();
            _starterInputs = FindFirstObjectByType<StarterAssetsInputs>();
        }

        // Call this from anywhere to switch between Gameplay and UI!
        public void SetUIState(bool isUIOpen)
        {
            if (isUIOpen)
            {
                Time.timeScale = 0f; // Pause game
                _playerInput.SwitchCurrentActionMap("UI"); // Switch to UI controls
            
                // Tell the First Person Controller to stop moving the camera
                _starterInputs.cursorLocked = false;
                _starterInputs.cursorInputForLook = false; 
            
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Time.timeScale = 1f; // Resume game
                _playerInput.SwitchCurrentActionMap("Player"); // Switch back to Player controls
            
                // Give control back to the First Person Character
                _starterInputs.cursorLocked = true;
                _starterInputs.cursorInputForLook = true;
            
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}