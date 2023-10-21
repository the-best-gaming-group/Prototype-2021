using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Platformer.Mechanics;
using Platformer.Core;
using System.Threading.Tasks;
using System.IO;

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
	public Checkpoint Checkpoint = null;
	public SceneChangeInvokable sceneChange;
	public string SaveFilePath;
	private bool tryingLoadingCheckpoint = false;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
			DontDestroyOnLoad(sceneChange.transitionAnim);
			SaveFilePath = Application.dataPath + "/save_file.txt";
			Task.Run(AsyncGetCheckpoint);
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
		if (Checkpoint.SceneName == "") SaveCheckpoint();
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
		SaveFileManager.WriteToSaveFile(SaveFilePath, Checkpoint);
	}
	
	public void LoadCheckpoint()
	{
		if (Checkpoint.SceneName == "")
		{
			return;
		}
		playerHealth = Checkpoint.playerHealth;
		EnemySpawns = Checkpoint.enemySpawns;
		PlayDoorSound = Checkpoint.playDoorSound;
		PlayerPos  = Checkpoint.playerPos;
		sceneChange.sceneName = Checkpoint.SceneName;
		sceneChange.Invoke();
	}
	
	public void NewGame()
	{
		const string scene = "Main Scene 1";
		Checkpoint = new(100, new(), new(), new(), scene);
		LoadCheckpoint();
	}
	
	public void Continue()
	{
		if (!File.Exists(SaveFilePath))
		{
			Debug.LogError("Tried to load nonexistent checkpoint file");
			return;
		}
		else if (tryingLoadingCheckpoint)
		{
			return;
		}
		StartCoroutine(TryLoadCheckpoint());
	}

	public async void AsyncGetCheckpoint()
	{
		Checkpoint = await SaveFileManager.ReadFromSaveFile(SaveFilePath);
	}
	
	IEnumerator TryLoadCheckpoint()
	{
		tryingLoadingCheckpoint = true;
		for (int i = 0; i < 30; i++)
		{
			if (Checkpoint.SceneName != "")
			{
				LoadCheckpoint();
				break;
			}
			yield return new WaitForSeconds(1f);
		}
		tryingLoadingCheckpoint = false;
	}
}
