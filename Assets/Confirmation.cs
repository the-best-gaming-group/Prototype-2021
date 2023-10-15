using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Confirmation : MonoBehaviour
{
	public Button yesButton;
	public Button noButton;
	public TextMeshProUGUI messageText;

	[SerializeField] private Confirmation myConfirmationWindow;

	void Start()
	{
		OpenConfirmationWindow("Are you sure?");
	}

	private void OpenConfirmationWindow(string message)
	{
		myConfirmationWindow.gameObject.SetActive(true);
		myConfirmationWindow.yesButton.onClick.AddListener(YesClicked);
		myConfirmationWindow.noButton.onClick.AddListener(NoClicked);
		myConfirmationWindow.messageText.text = message;
	}

	private void YesClicked()
	{
		myConfirmationWindow.gameObject.SetActive(false);
		Debug.Log("Yes Clicked");
	}

	private void NoClicked()
	{
		myConfirmationWindow.gameObject.SetActive(false);
		Debug.Log("No Clicked");
	}
}
