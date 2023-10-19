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

	private void Start()
	{
		// Check if the initial health value is already set in PlayerPrefs
		if (PlayerPrefs.HasKey("InitialHealth"))
		{
			currentHealth = PlayerPrefs.GetInt("InitialHealth");
		}
		else
		{
			// If not set, initialize health to the maximum
			currentHealth = maxHealth;
			// Store the initial health value
			PlayerPrefs.SetInt("InitialHealth", currentHealth);
		}

		healthBar.SetHealth(currentHealth);
	}

	private void Update()
	{
		// Continuously update the health bar to reflect current health
		healthBar.SetHealth(currentHealth);
	}
	/*
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Enemy"))
		{
			Debug.Log ("Player collided with an enemy.");
			TakeDamage(10);
		}
	}
	*/

	public int TakeDamage(int damage)
	{
		Debug.Log("Taking damage: " + damage);
		currentHealth = damage >= currentHealth ? 0 : (currentHealth - damage);

		// Store the updated health in PlayerPrefs
		PlayerPrefs.SetInt("InitialHealth", currentHealth);
		PlayerPrefs.Save(); // Make sure to save PlayerPrefs

		// Update the health bar
		healthBar.SetHealth(currentHealth);

		return currentHealth;
	}

	public void HPManager(int damage)
	{
		TakeDamage(damage);
	}

}
