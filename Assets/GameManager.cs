using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance; // Singleton instance

	public GameObject enemyToSpawn; // Store the collided enemy to spawn in the combat scene
	private int playerHealth;

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
	
	public void SetPlayerHealth(int i)
	{
		playerHealth = i;
	}
	
	public int GetPlayerHealth()
	{
		return playerHealth;		
	}

}
