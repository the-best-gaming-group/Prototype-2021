using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Inventory/Item")]
public class InventoryItem : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public Button itemButtonPrefab;
    public string itemInfo;
}
