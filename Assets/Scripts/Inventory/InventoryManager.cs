using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public InventoryData inventoryData;

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

    /*
    private void CreateInventorySlot(InventoryItem item)
    {
        GameObject slot = Instantiate(inventorySlotPrefab, gridLayoutGroup.transform);
        slot.GetComponent<Image>().sprite = item.itemIcon;
        slot.SetActive(true);
    }
    */
}
