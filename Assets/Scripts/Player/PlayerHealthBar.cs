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

    private bool isPlayer; // Indicate if it's the player

    private void Start()
    {
        isPlayer = CompareTag("Player"); // Check if this is the player

        // Check if the initial health value is already set in PlayerPrefs
        if (isPlayer && PlayerPrefs.HasKey("InitialHealth"))
        {
            currentHealth = PlayerPrefs.GetInt("InitialHealth");
        }
        else
        {
            // If not set or not the player, initialize health to the maximum
            currentHealth = maxHealth;

            if (isPlayer)
            {
                // Store the initial health value only for the player
                PlayerPrefs.SetInt("InitialHealth", currentHealth);
            }
        }

        healthBar.SetHealth(currentHealth);
    }

    // Rest of the script remains the same...

    public int TakeDamage(int damage, bool isPlayer)
    {
        Debug.Log("Taking damage: " + damage);
        currentHealth = damage >= currentHealth ? 0 : (currentHealth - damage);

        // Store the updated health in PlayerPrefs only for the player
        if (isPlayer)
        {
            PlayerPrefs.SetInt("InitialHealth", currentHealth);
            PlayerPrefs.Save();
        }

        // Update the health bar
        healthBar.SetHealth(currentHealth);

        return currentHealth;
    }

    public void HPManager(int damage)
	{
		TakeDamage(damage, true);
	}

}
