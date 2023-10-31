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

	private ShopManager shopManager; // Reference to the ShopManager

	private int itemID; // Item ID for the confirmation

	void Start()
	{
		shopManager = FindObjectOfType<ShopManager>();
		yesButton.onClick.AddListener(YesClicked);
		noButton.onClick.AddListener(NoClicked);
		gameObject.SetActive(false);
	}

	public void OpenConfirmationWindow(string message, int itemID)
	{
		this.itemID = itemID; // Store the item ID for later use
		messageText.text = message;
		gameObject.SetActive(true);
	}

	private void YesClicked()
	{
		// Close the confirmation window
		gameObject.SetActive(false);

		// Reduce the coins and hide the item (you can add the logic here)
		int itemPrice = shopManager.shopItems[2, itemID];
		GameManager.Instance.setCoins(GameManager.Instance.getCoins() - itemPrice);
		shopManager.ConisTXT.text = "Coins: " + GameManager.Instance.getCoins().ToString();

		// shopManager.ConisTXT.text = "Coins: " + shopManager.coins.ToString();
		//GameManager.Instance.AddToInventory(itemID, 1);
		shopManager.DisableItemButton(itemID);

		GameManager.Instance.spells.Add(new GameManager.Spell(shopManager.itemNames[itemID]));
	}

	private void NoClicked()
	{
		// Close the confirmation window
		gameObject.SetActive(false);
	}
}

