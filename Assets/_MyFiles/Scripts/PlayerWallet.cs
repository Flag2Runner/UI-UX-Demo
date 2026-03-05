using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CurrencyBalance
{
    public Currency currencyType;
    public int amount;
}

public class PlayerWallet : MonoBehaviour
{
    [Header("Player's Money")]
    public List<CurrencyBalance> balances = new List<CurrencyBalance>();

    public bool TryPurchase(ItemData item)
    {
        CurrencyBalance balance = balances.Find(b => b.currencyType == item.costCurrency);
        if (balance != null && balance.amount >= item.costAmount)
        {
            balance.amount -= item.costAmount;
            return true;
        }
        return false;
    }
}