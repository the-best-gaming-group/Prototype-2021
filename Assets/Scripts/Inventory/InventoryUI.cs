using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventoryData data;
    [SerializeField] private RectTransform panel;
    [SerializeField] private RectTransform inventoryFrame;
    [SerializeField] private RectTransform inventoryIcon;

    public static InventoryUI Instance;
    private int inventoryCount;

    private void Start()
    {
        inventoryCount = 0;
    }

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

    private void Update()
    {
        if (data.items.Count != inventoryCount)
        {
            UpdateInventory(data);
        }

        if (SceneManager.GetActiveScene().name == "MainMenu" || SceneManager.GetActiveScene().name == "Combat Arena")
        {
            Destroy(gameObject);
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