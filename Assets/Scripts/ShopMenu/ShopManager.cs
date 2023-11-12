using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
	public int[,] shopItems = new int[5, 5];
	public string[] itemNames = new string[5];
	public float coins;
	public TextMeshProUGUI ConisTXT;
	public Confirmation confirmationWindow;
	public Button[] itemButtons;

	void Start()
	{
		//ID's
		ConisTXT.text = "Coins:" + coins.ToString();
		shopItems[1, 1] = 1;
		shopItems[1, 2] = 2;
		shopItems[1, 3] = 3;
		shopItems[1, 4] = 4;

		//Price
		shopItems[2, 1] = 10;
		shopItems[2, 2] = 20;
		shopItems[2, 3] = 30;
		shopItems[2, 4] = 40;

		//Names
		itemNames[1] = "FireBall";
		itemNames[2] = "Freeze";
		itemNames[3] = "Heal";
		itemNames[4] = "Stun";
	}

	public void Buy(int itemID)
	{
		// Retrieve the price for the item
		int itemPrice = shopItems[2, itemID];

		// Check if the player has enough coins
		if (coins >= itemPrice)
		{
			// Open the confirmation window with the item name and price
			confirmationWindow.OpenConfirmationWindow("Are you sure you want to buy " + itemNames[itemID] + " for $" + itemPrice + "?", itemID);
		}
		else
		{
			Debug.Log("You don't have any coin left.");
		}

	}
	public void DisableItemButton(int itemID)
	{
		if (itemID < itemButtons.Length)
		{
			itemButtons[itemID-1].interactable = false; // Disable the button
		}
	}
}
