using Platformer.Mechanics;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public InventoryData inventoryData;
    public SceneChangeInvokable DoorToOpen;

    private bool hasNews1;
    private bool hasNews2;
    private bool hasNews3;
    private bool condition;

    private DialogueObject dialogueObject;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void AddItem(InventoryItem item)
    {
        inventoryData.AddItem(item);
    }

    public void RemoveItem(InventoryItem item)
    {
        inventoryData.RemoveItem(item);
    }

    public void CheckNewspaper()
    {
        Debug.Log("checking...");
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
        if (hasNews1 && hasNews2 && hasNews3)
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

    public void newspaperRemove(InventoryItem item)
    {
        if (condition)
        {
            RemoveItem(item);
        }
    }

    public void OpenDoor()
    {
        if (condition)
        {
            DoorToOpen.CanEnter = true;
            gameManager.OpenDoor();
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