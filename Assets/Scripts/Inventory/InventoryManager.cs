using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public InventoryData inventoryData;

    private bool hasNews1;
    //public GridLayoutGroup gridLayoutGroup; // Reference to the GridLayoutGroup UI component
    //public GameObject inventorySlotPrefab; // Reference to the Inventory Slot prefab

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(InventoryItem item)
    {
        inventoryData.AddItem(item);
        //CreateInventorySlot(item);
    }

    public void RemoveItem(InventoryItem item)
    {
        inventoryData.RemoveItem(item);
    }

    public void CheckNewspaper()
    {
        hasNews1 = false;
        for (int i = 0; i < inventoryData.items.Count; i++)
        {
            if (inventoryData.items[i].itemName == "News1")
            {
                hasNews1 = true;
            }
        }
        if (hasNews1 == true)
        {
            Debug.Log("YES");
        }
        else
        {
            Debug.Log("NO");
        }
    }
}
