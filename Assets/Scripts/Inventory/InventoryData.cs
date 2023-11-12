using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/InventoryData")]
public class InventoryData : ScriptableObject
{
    public List<InventoryItem> items = new List<InventoryItem>();

    public void AddItem(InventoryItem item)
    {
        items.Add(item);
    }

    public void RemoveItem(InventoryItem item)
    {
        items.Remove(item);
    }

    public void ClearItem()
    {
        items.Clear();
    }
}
