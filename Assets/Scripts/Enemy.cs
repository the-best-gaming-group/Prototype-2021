using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 50;
    public int currentHealth;
    public int damage = 10;
    public string enemyType; // You can specify enemy types here.

    private void Start()
    {
        currentHealth = maxHealth;
    }

    // Add other methods for enemy behavior as needed.
}
