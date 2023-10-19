using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventoryData data;
    [SerializeField] private RectTransform panel;
    [SerializeField] private RectTransform inventoryFrame;
    [SerializeField] private RectTransform inventoryIcon;

    private int inventoryCount;

    private void Start()
    {
        inventoryCount = 0;
    }

    private void Update()
    {
        if (data.items.Count != inventoryCount)
        {
            UpdateInventory(data);
        }
    }

    private void UpdateInventory(InventoryData data)
    {
        // Clear the inventory UI before updating it
        ClearInventory();

        // Iterate through the inventory items and create UI slots for each item
        foreach (InventoryItem item in data.items)
        {
            GameObject inventorySlot = Instantiate(inventoryFrame.gameObject, panel);
            inventorySlot.gameObject.SetActive(true);
            inventorySlot.transform.GetChild(0).GetComponent<Image>().sprite = item.itemIcon;

            // Add a button component to the inventory slot
            Button button = inventorySlot.AddComponent<Button>();
            button.onClick.AddListener(() => OnInventorySlotClick(item));
        }

        panel.sizeDelta = new Vector2(panel.sizeDelta.x, inventoryFrame.sizeDelta.y * data.items.Count);
        inventoryCount = data.items.Count;
    }


    /*
    private void UpdateInventory(InventoryData data)
    {
        if (inventoryCount < data.items.Count)
        {
            // Add new items to the inventory UI
            for (int i = inventoryCount; i < data.items.Count; i++)
            {
                GameObject inventorySlot = Instantiate(inventoryFrame.gameObject, panel);
                inventorySlot.gameObject.SetActive(true);
                inventorySlot.transform.GetChild(0).GetComponent<Image>().sprite = data.items[i].itemIcon;

                // Add a button component to the inventory slot
                Button button = inventorySlot.AddComponent<Button>();
                button.onClick.AddListener(() => OnInventorySlotClick(data.items[i]));

                panelHeight += inventoryFrame.sizeDelta.y;
            }
            panel.sizeDelta = new Vector2(panel.sizeDelta.x, panelHeight);
        }
        else if (inventoryCount > data.items.Count)
        {
            // Remove items from the inventory UI
            int itemsToRemove = inventoryCount - data.items.Count;
            for (int i = 0; i < itemsToRemove; i++)
            {
                Transform slotTransform = panel.GetChild(panel.childCount - 1); // Get the last child
                Destroy(slotTransform.gameObject); // Remove the GameObject from the UI
                panelHeight -= inventoryFrame.sizeDelta.y;
            }
            panel.sizeDelta = new Vector2(panel.sizeDelta.x, panelHeight);
        }

        panel.gameObject.SetActive(true);
        inventoryCount = data.items.Count;
    }
    */


    private void ClearInventory()
    {
        for (int i = 1; i < panel.childCount; i++)
        {
            Transform child = panel.GetChild(i);
            Destroy(child.gameObject);
        }
    }

    private void OnInventorySlotClick(InventoryItem item)
    {
        // Handle the click event for the inventory slot here
        // You can implement actions such as using the item, displaying item details, etc.
        Debug.Log("Clicked on: " + item.itemName);
    }
}