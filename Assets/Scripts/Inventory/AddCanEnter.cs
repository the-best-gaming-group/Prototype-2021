using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCanEnter : MonoBehaviour
{
    public SceneChangeInvokable sceneChangeInvokablev;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;

        if (gameManager.GetOpen())
        {
            sceneChangeInvokablev.CanEnter = true;
        }
    }
}
