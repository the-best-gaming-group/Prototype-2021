using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Platformer.Mechanics;
using System.Linq;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance; // Singleton instance

	public GameObject enemyToSpawn; // Store the collided enemy to spawn in the combat scene
	private int playerHealth;
	public Stack<string> scenes = new ();
	public Checkpoint.EnemySpawns EnemySpawns = new ();
	public Checkpoint.PlayDoorSound PlayDoorSound = new();
	public Checkpoint.PlayerPos PlayerPos = new();
	public string SceneName => SceneManager.GetActiveScene().name;
	private string enemyUID;
	private Checkpoint Checkpoint;
	public SceneChangeInvokable sceneChange;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
			DontDestroyOnLoad(sceneChange.transitionAnim);
			scenes.Push(SceneName);
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
	
	public void PrepareForCombatSceneEnter(string newScene, Vector3 playerPos, string enemyUID)
	{
		if (Checkpoint == null) SaveCheckpoint();
        if (!scenes.TryPeek(out string outSceneStack) || !SceneName.Equals(outSceneStack))
        {
			scenes.Push(SceneName);
        }
		scenes.Push(newScene);
		PlayerPos[SceneName] = playerPos;
		this.enemyUID = enemyUID;
    }

	public void PrepareForSceneEnter(string newScene)
	{
        if (!scenes.TryPeek(out string outSceneStack) || !SceneName.Equals(outSceneStack))
        {
			scenes.Push(SceneName);
        }
		scenes.Push(newScene);
    }
	
	public string PrepareForReturnFromCombat()
	{
		// Remove combat scene
		scenes.Pop();
		// Get name of previous scene
		var prevScene = scenes.Peek();
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
			scenes,
			EnemySpawns,
			PlayDoorSound,
			PlayerPos,
			SceneName
		);
	}
	
	public void LoadCheckpoint()
	{
		playerHealth = Checkpoint.playerHealth;
		scenes = new Stack<string>(Checkpoint.scenes);
		EnemySpawns = Checkpoint.enemySpawns;
		PlayDoorSound = Checkpoint.playDoorSound;
		PlayerPos  = Checkpoint.playerPos;
		sceneChange.sceneName = Checkpoint.SceneName;
		sceneChange.Invoke();
	}

}
