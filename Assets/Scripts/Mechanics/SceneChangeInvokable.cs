using System.Collections;
using System.Collections.Generic;
using Platformer.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeInvokable : MonoBehaviour, Invokable
{
	public string sceneName;
	public void ChangeScene()
	{
		SceneManager.LoadScene(sceneName);
	}
	public void Exit()
	{
		Application.Quit ();
	}
	
	public void Invoke() {
		ChangeScene();
	}
}