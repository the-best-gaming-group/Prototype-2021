using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item")]
public class InventoryItem : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;

    public static implicit operator InventoryItem(InventoryData v)
    {
        throw new NotImplementedException();
    }
}
