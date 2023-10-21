using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	private Button ContinueButton;
	public void Start()
	{
		ContinueButton = transform.Find("Continue").GetComponent<Button>();
		bool saveFileExists = File.Exists(GameManager.Instance.SaveFilePath);
		if (!saveFileExists)
		{
			ContinueButton.interactable = saveFileExists;
			ContinueButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
		}
	}

	public void NewGame()
	{
		GameManager.Instance.NewGame();
	}
	public void Continue()
	{
		GameManager.Instance.Continue();
		transform.Find("Loading").GetComponent<TextMeshProUGUI>().enabled = true;
		ContinueButton.interactable = false;
		transform.Find("New Game").GetComponent<Button>().interactable = false;
	}
	public void QuitGame()
	{
		Debug.Log("Quit");
		Application.Quit();
	}
}
