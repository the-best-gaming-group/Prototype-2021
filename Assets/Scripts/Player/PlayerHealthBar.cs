using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthBar : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;
	private RectTransform canvasRectTransform;

	void Start()
    {
        currentHealth = GameManager.Instance.playerHealth;
		// Check if we are in the CombatScene
		if (SceneManager.GetActiveScene().name == "CombatScene")
        {
            // If in the CombatScene, set healthBar to currentHealth
            //Debug.Log("Combat Scene rn, current health is "+ currentHealth);
            healthBar.SetHealth(currentHealth);
        }
        else
        {
            //Debug.Log("Set currentHealth to maxHealth when starting the game.");
            currentHealth = maxHealth;
            // If not in the CombatScene, set healthBar to maxHealth
            healthBar.SetMaxHealth(maxHealth);
        }
    }

    void Update()
    {
        // Continuously update the health bar to reflect current health
        //Debug.Log("Update health " + currentHealth);
        healthBar.SetHealth(currentHealth);

	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //Debug.Log("Player collided with an enemy.");
            TakeDamage(20);
        }
    }

    public int TakeDamage(int damage)
    {
        //Debug.Log("Taking damage: " + damage);
        currentHealth = damage >= currentHealth ? 0 : (currentHealth - damage);

        // Store the updated health in the GameManager
        GameManager.Instance.playerHealth = currentHealth;

        // Update the health bar
        healthBar.SetHealth(currentHealth);

        return currentHealth;
    }

    public void HpManager(int value)
    {
        TakeDamage(value);
    }
}
