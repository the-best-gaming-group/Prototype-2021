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
	[SerializeField] private int playerHealth;
	public Checkpoint.SpawnsDict Spawns = new ();
	public Checkpoint.PlayDoorSoundDict PlayDoorSound = new();
	public Checkpoint.PlayerPosDict PlayerPos = new();
	public string SceneName => SceneManager.GetActiveScene().name;
	private string prevScene;
	private string enemyUID;
	public Checkpoint Checkpoint = null;
	public SceneChangeInvokable sceneChange;
	public string SaveFilePath;
	private bool tryingLoadingCheckpoint = false;
	private const int CAN_SPAWN = -1;
	private const int CANT_SPAWN = 0;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
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
		Debug.Log("Player Health is: " + playerHealth);
		return playerHealth;		
	}
	
	public void RegisterRoomSpawner(RoomSpawner res)
	{
		Spawns.TryAdd(SceneName, new ());
		Spawns[SceneName].TryAdd(res.uID, CAN_SPAWN);
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
		Spawns[prevScene][enemyUID] = CANT_SPAWN;
		return prevScene;
	}
	
	public bool CanSpawn(string uID)
	{
		return Spawns[SceneName][uID] == CAN_SPAWN;
	}
	
	public int SavedDialogueIdx(string uID)
	{
		return Spawns[SceneName][uID];
	}
	
	public void SaveDialogue(string uID, int idx, Vector3 pos)
	{
		PlayerPos[SceneName] = pos;
		Spawns[SceneName][uID] = idx;
		SaveCheckpoint();
	}
	
	public void SaveCheckpoint()
	{
		Checkpoint = new Checkpoint(
			playerHealth,
			Spawns,
			PlayDoorSound,
			PlayerPos,
			SceneName
		);
		// Debug.Log("When saving checkpoint playerHP = " + playerHealth);
		SaveFileManager.WriteToSaveFile(SaveFilePath, Checkpoint);
	}
	
	public void LoadCheckpoint()
	{
		if (Checkpoint.SceneName == "")
		{
			return;
		}
		playerHealth = Checkpoint.playerHealth;
		// Debug.Log("Hp when loading checkpoint" + playerHealth);
		Spawns = Checkpoint.spawns;
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
