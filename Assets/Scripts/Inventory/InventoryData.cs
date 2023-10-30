using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/InventoryData")]
public class InventoryData : ScriptableObject
{
    public List<InventoryItem> items = new List<InventoryItem>();
    private bool hasNews1;

    public void AddItem(InventoryItem item)
    {
        items.Add(item);
    }

    public void RemoveItem(InventoryItem item)
    {
        items.Remove(item);
    }

    //public void CheckNewspaper()
    //{
    //    hasNews1 = false;
    //    for (int i = 0; i < items.Count; i++)
    //    {
    //        Debug.Log("checking item");
    //        if (items[i].itemName == "News1")
    //        {
    //            hasNews1 = true;
    //        }
    //    }
    //    if (hasNews1 == true)
    //    {
    //        Debug.Log("has news 1");
    //    }
    //    else
    //    {
    //        Debug.Log(" don't have news 1");
    //    }
    //}
}
