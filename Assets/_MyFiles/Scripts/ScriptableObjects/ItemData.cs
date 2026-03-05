using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Economy/Item")]
public class ItemData : ScriptableObject
{
    [Header("Basic Info")]
    public string itemName;
    public Sprite itemIcon;

    [Header("Cost")]
    public int costAmount;
    public Currency costCurrency;
}