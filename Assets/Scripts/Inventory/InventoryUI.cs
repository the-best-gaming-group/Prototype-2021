using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private RectTransform panel;
    [SerializeField] private RectTransform inventoryFrame;
    [SerializeField] private InventoryData data;

    private int inventoryCount;
    private float panelHeight;

    private void Start()
    {
        inventoryCount = 0;
        panelHeight = 0;
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
        //ClearInventory();
        
        // if inventory added
        if (inventoryCount < data.items.Count)
        {
            for (int i = inventoryCount; i < data.items.Count; i++)
            {
                GameObject inventorySlot = Instantiate(inventoryFrame.gameObject, panel);
                inventorySlot.gameObject.SetActive(true);
                inventorySlot.GetComponent<Image>().sprite = data.items[i].itemIcon;

                // Add a button component to the inventory slot
                Button button = inventorySlot.AddComponent<Button>();
                button.onClick.AddListener(() => OnInventorySlotClick(data.items[i]));

                panelHeight += inventoryFrame.sizeDelta.y;
            }
            panel.sizeDelta = new Vector2(panel.sizeDelta.x, panelHeight);
        }
        /*
        else
        {
            panelHeight -= inventoryFrame.sizeDelta.y;
            panel.sizeDelta = new Vector2(panel.sizeDelta.x, panelHeight);
        }
        */
        /*
        foreach (InventoryItem item in data.items)
        {
            GameObject inventorySlot = Instantiate(inventoryFrame.gameObject, panel);
            inventorySlot.gameObject.SetActive(true);
            inventorySlot.GetComponent<Image>().sprite = item.itemIcon;

            // Add a button component to the inventory slot
            Button button = inventorySlot.AddComponent<Button>();
            button.onClick.AddListener(() => OnInventorySlotClick(item));

            panelHeight += inventoryFrame.sizeDelta.y;
        }
        */

        panel.gameObject.SetActive(true);
        inventoryCount = data.items.Count;
    }

    private void ClearInventory()
    {
        foreach (Transform child in panel.transform)
        {
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















/*
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private RectTransform panel;
    [SerializeField] private RectTransform inventoryFrame;
    [SerializeField] private InventoryData data;

    private void ShowInventory(InventoryData data)
    {
        throw new NotImplementedException();
    }

    public void ShowInventory(InventoryItem[] items)
    {
        float panelHeight = 0;

        for (int i = 0; i < items.Length; i++)
        {
            InventoryItem item = items[i];
            int ItemIndex = i;

            GameObject inventorySlot = Instantiate(inventoryFrame.gameObject, panel);
            inventorySlot.gameObject.SetActive(true);
            inventorySlot.GetComponent<Image>().sprite = item.itemIcon;
            // inventorySlot.GetComponent<Button>().onClick.AddListener(() => OnPickedResponse(response, ItemIndex));

            panelHeight += inventoryFrame.sizeDelta.y;
        }

        panel.sizeDelta = new Vector2(panel.sizeDelta.x, panelHeight);
        panel.gameObject.SetActive(true);
    }
}
*/