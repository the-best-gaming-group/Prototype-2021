using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public SceneChangeInvokable DoorToOpen;

    public DialogueObject dialogueNo;
    public InventoryItem news1;
    public InventoryItem news2;
    public InventoryItem news3;
    public DialogueUI dialogueUI;
    public GameObject toDestroy;

    private bool hasNews1;
    private bool hasNews2;
    private bool hasNews3;
    private bool condition;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void AddItem(InventoryItem item)
    {
        gameManager.AddItem(item);
    }

    public void RemoveItem(InventoryItem item)
    {
        gameManager.RemoveItem(item);
    }

    public void CheckNewspaper()
    {
        List<InventoryItem> items = gameManager.GetItmes();

        hasNews1 = false;
        hasNews2 = false;
        hasNews3 = false;
        //for (int i = 0; i < inventoryData.items.Count; i++)
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemName == "News1")
            {
                hasNews1 = true;
            }
            else if (items[i].itemName == "News2")
            {
                hasNews2 = true;
            }
            else if (items[i].itemName == "News3")
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

        if (condition)
        {
            RemoveItem(news1);
            RemoveItem(news2);
            RemoveItem(news3);
            DoorToOpen.CanEnter = true;
            gameManager.OpenDoor();
            Destroy(toDestroy);
        }
        else
        {
            dialogueUI.ShowDialogue(dialogueNo); 
        }

    }
}