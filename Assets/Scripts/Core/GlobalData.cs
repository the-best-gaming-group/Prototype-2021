using System.Collections;
using System.Collections.Generic;
using Platformer.Mechanics;
using UnityEngine;
using Platformer.Core;

[RequireComponent(typeof(Invokable))]
public class GlobalData : MonoBehaviour
{
    public static Health playerHealth;
    public Invokable invokable;
    int waitTime = 5;
    // Start is called before the first frame update
    void Start()
    {
        invokable = GetComponent<Invokable>();
        playerHealth = GetComponent<Health>();
        playerHealth.maxHP = 100;
        while (playerHealth.currentHP < 100) {
            playerHealth.Increment();
        }
        Debug.Log("Player health is: " + playerHealth.currentHP.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        if (waitTime > 0) {
            waitTime -= 1;
        }
        else {
            invokable.Invoke();
        }
    }
    
    public static void InitHealth() {
        if (playerHealth == null) {
            playerHealth = new Health();
        }
        playerHealth.maxHP = 100;
        while (playerHealth.currentHP < 100) {
            playerHealth.Increment();
        }
        Debug.Log("Player health is: " + playerHealth.currentHP.ToString());
    }
}
