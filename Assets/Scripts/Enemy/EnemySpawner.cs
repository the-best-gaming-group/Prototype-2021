using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	public GameObject[] enemyPrefabs; // Array of enemy prefabs to choose from

	void Start()
	{
        // Check if GameManager has an enemy to spawn
        string objectToSpawn = PlayerPrefs.GetString("ObjectToSpawn", "Skeleton NLA w_Lights");

        // Loop through the available enemy prefabs and find the one that matches the stored name
        GameObject enemyToSpawn = null;
		if (objectToSpawn != null)
		{
			foreach (GameObject enemyPrefab in enemyPrefabs)
			{
				if (enemyPrefab.name.ToLower().Contains(objectToSpawn.ToLower()))
				{
					enemyToSpawn = enemyPrefab;
					break;
				}
			}
		}

		if (enemyToSpawn != null)
		{
            // Instantiate the enemy
            var enemyObjTransform = GameObject.Find("Enemi").transform;

            Instantiate(enemyToSpawn, transform.position, transform.rotation);
        }
		else
		{
			// Handle the case where the enemy prefab with the stored name is not found
			Debug.LogError("Enemy prefab with name '" + objectToSpawn + "' not found!");
		}
	}
}
