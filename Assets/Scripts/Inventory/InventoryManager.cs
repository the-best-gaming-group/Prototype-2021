using Platformer.Mechanics;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public InventoryData inventoryData;

    private bool hasNews1;
    private bool hasNews2;
    private bool hasNews3;

    private DialogueObject dialogueObject;
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
        hasNews2 = false;
        hasNews3 = false;
        for (int i = 0; i < inventoryData.items.Count; i++)
        {
            if (inventoryData.items[i].itemName == "News1")
            {
                hasNews1 = true;
            }
            else if (inventoryData.items[i].itemName == "News2")
            {
                hasNews2 = true;
            }
            else if (inventoryData.items[i].itemName == "News3")
            {
                hasNews3 = true;
            }
        }
        Debug.Log(hasNews1);
        Debug.Log(hasNews2);
        Debug.Log(hasNews3);
    }

    public void completeNews(DialogueObject dialogueYes)
    {
        if (hasNews1 && hasNews2 && hasNews3)
        {
            dialogueObject = dialogueYes;
        }
    }
    public void notCompleteNews(DialogueObject dialogueNo)
    {
        if (!hasNews1 || !hasNews2 || !hasNews3)
        {
            dialogueObject = dialogueNo;
        }
    }
    public void responseDialogue(DialogueUI dialogueUI)
    {
        dialogueUI.ShowDialogue(dialogueObject);
    }
}