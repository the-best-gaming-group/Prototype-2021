using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryManager Instance;

    [SerializeField] private RectTransform panel;
    [SerializeField] private RectTransform inventoryFrame;
    [SerializeField] private RectTransform inventoryIcon;

    private GameManager gameManager;
    private int inventoryCount;

    private void Start()
    {
        gameManager = GameManager.Instance;
        inventoryCount = 0;
        Debug.Log("InventoryUI start");
        Debug.Log(inventoryCount);
    }

    private void Update()
    {
        List<InventoryItem> items = gameManager.GetItmes();
        //Debug.Log(inventoryCount);
        //Debug.Log(items.Count);
        if (items.Count != inventoryCount)
        {
            UpdateInventory(items);
        }
    }

    private void UpdateInventory(List<InventoryItem> items)
    {
        // Clear the inventory UI before updating it
        ClearInventory();

        // Iterate through the inventory items and create UI slots for each item
        foreach (InventoryItem item in items)
        {
            GameObject inventorySlot = Instantiate(inventoryFrame.gameObject, panel);
            inventorySlot.gameObject.SetActive(true);
            inventorySlot.transform.GetChild(0).GetComponent<Image>().sprite = item.itemIcon;

            // Add a button component to the inventory slot
            Button button = inventorySlot.AddComponent<Button>();
            button.onClick.AddListener(() => OnInventorySlotClick(item));
        }

        panel.sizeDelta = new Vector2(panel.sizeDelta.x, inventoryFrame.sizeDelta.y * items.Count);
        inventoryCount = items.Count;
    }


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