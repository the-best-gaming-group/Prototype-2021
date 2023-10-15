using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Platformer.Mechanics;
using System.Linq;
using Platformer.Core;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance; // Singleton instance

	public GameObject enemyToSpawn; // Store the collided enemy to spawn in the combat scene
	private int playerHealth;
	public Checkpoint.EnemySpawns EnemySpawns = new ();
	public Checkpoint.PlayDoorSound PlayDoorSound = new();
	public Checkpoint.PlayerPos PlayerPos = new();
	public string SceneName => SceneManager.GetActiveScene().name;
	private string prevScene;
	private string enemyUID;
	private Checkpoint Checkpoint;
	public SceneChangeInvokable sceneChange;
	private string saveFilePath;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
			DontDestroyOnLoad(sceneChange.transitionAnim);
			saveFilePath = Application.dataPath + "/save_file.txt";
			// This is just a PoC, this will need to be mapped to menu buttons
			// Uncomment to see the load checkpoint work!
			// LoadCheckpoint();
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
	
	public void RegisterRoomEnemySpawner(RoomEnemySpawner res)
	{
		EnemySpawns.TryAdd(SceneName, new ());
		EnemySpawns[SceneName].TryAdd(res.uID, true);
	}
	
	public void PrepareForCombatSceneEnter(Vector3 playerPos, string enemyUID)
	{
		if (Checkpoint == null) SaveCheckpoint();
		PlayerPos[SceneName] = playerPos;
		this.enemyUID = enemyUID;
		prevScene = SceneName;
    }

	public string PrepareForReturnFromCombat()
	{
		EnemySpawns[prevScene][enemyUID] = false;
		return prevScene;
	}
	
	public bool CanSpawn(string uID)
	{
		return EnemySpawns[SceneName][uID];
	}
	
	public void SaveCheckpoint()
	{
		Checkpoint = new Checkpoint(
			playerHealth,
			EnemySpawns,
			PlayDoorSound,
			PlayerPos,
			SceneName
		);
		var json = JsonUtility.ToJson(Checkpoint);
		var res = SaveFileManager.WriteToSaveFile(saveFilePath, JsonUtility.ToJson(Checkpoint));
	}
	
	public void LoadCheckpoint()
	{
		if (Checkpoint == null)
		{
			SaveFileManager.ReadFromSaveFile(saveFilePath, out string json);
			Checkpoint = JsonUtility.FromJson<Checkpoint>(json);
		}
		playerHealth = Checkpoint.playerHealth;
		EnemySpawns = Checkpoint.enemySpawns;
		PlayDoorSound = Checkpoint.playDoorSound;
		PlayerPos  = Checkpoint.playerPos;
		sceneChange.sceneName = Checkpoint.SceneName;
		sceneChange.Invoke();
	}

}
