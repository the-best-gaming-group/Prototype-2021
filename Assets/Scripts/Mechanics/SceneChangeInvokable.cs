using System.Collections;
using System.Collections.Generic;
using Platformer.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeInvokable : MonoBehaviour, Invokable
{
	[SerializeField] public Animator transitionAnim;
	public string sceneName;
	public bool IsDoor = true;

	IEnumerator ChangeScene()
	{
		Debug.Log("Changing Scene");

		var gm = GameManager.Instance;
		gm.PlayDoorSound[sceneName] = IsDoor;

		transitionAnim.SetTrigger("Start");
		yield return new WaitForSeconds(1);
		SceneManager.LoadScene(sceneName);
		transitionAnim.SetTrigger("End");
	}
	public void Exit()
	{
		Application.Quit();
	}

	public void Invoke()
	{
		StartCoroutine(ChangeScene());
	}
}