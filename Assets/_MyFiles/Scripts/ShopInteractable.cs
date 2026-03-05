using UnityEngine;

public class ShopInteractable : MonoBehaviour
{
    public ShopUIManager shopUI;
    private PlayerWallet activePlayer = null;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            activePlayer = other.GetComponent<PlayerWallet>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            activePlayer = null;
           shopUI.CloseShop();
        }
    }

    void Update()
    {
        if (activePlayer != null && Input.GetKeyDown(KeyCode.E))
        {
           shopUI.OpenShop(activePlayer);
        }
    }
}