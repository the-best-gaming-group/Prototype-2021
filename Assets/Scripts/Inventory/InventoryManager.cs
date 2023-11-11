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
    private bool condition;

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
        }
        if (hasNews1 && hasNews2)
        {
            condition = true;
        }
        else
        {
            condition = false;
        }
    }

    public void completeNews(DialogueObject dialogueYes)
    {
        if (condition)
        {
            dialogueObject = dialogueYes;
        }
    }
    public void notCompleteNews(DialogueObject dialogueNo)
    {
        if (!condition)
        {
            dialogueObject = dialogueNo;
        }
    }
    public void responseDialogue(DialogueUI dialogueUI)
    {
        dialogueUI.ShowDialogue(dialogueObject);
    }

    public void newspaperReward()
    {
        if (condition)
        {
            Debug.Log("get 50 coins");
        }
    }

    public void newspaperRemove1(InventoryItem item)
    {
        if (condition)
        {
            RemoveItem(item);
        }
    }
    public void newspaperRemove2(InventoryItem item)
    {
        if (condition)
        {
            RemoveItem(item);
        }
    }

    public void finished(GameObject toDestroy)
    {
        if (condition)
        {
            Destroy(toDestroy);
        }
    }
}