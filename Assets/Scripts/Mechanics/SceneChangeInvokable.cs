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
	public bool CanEnter;

    IEnumerator ChangeScene()
	{
		Debug.Log("Changing Scene");

		var gm = GameManager.Instance;
		gm.PlayDoorSound[sceneName] = IsDoor;

		transitionAnim.SetTrigger("Start");
		yield return new WaitForSeconds(1);
		SceneManager.LoadScene(sceneName);
		Resources.UnloadUnusedAssets();
        transitionAnim.SetTrigger("End");
	}
	public void Exit()
	{
		Application.Quit();
	}

	public void Invoke()
	{
		string currentScene = SceneManager.GetActiveScene().name;
		if (currentScene == "Main Scene 1" || currentScene == "Main Scene 2")
        {
			if (CanEnter)
            {
				StartCoroutine(ChangeScene());
			}
		}
        else
        {
			StartCoroutine(ChangeScene());
		}
	}
}