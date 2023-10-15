using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonInfo : MonoBehaviour
{
	public int ItemID;
	public TextMeshProUGUI PriceTxt;
	public GameObject ShopManager;

	void Update()
	{
		PriceTxt.text = "Price: $" + ShopManager.GetComponent<ShopManager>().shopItems[2, ItemID].ToString();
	}
}
