using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteDoor : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        string objectName = gameObject.name;

        if (objectName == "Door5_condition" && gameManager.GetOpen())
        {
            Destroy(gameObject);
        }
    }
}
