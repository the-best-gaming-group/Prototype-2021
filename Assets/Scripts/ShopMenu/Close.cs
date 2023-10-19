using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Close : MonoBehaviour
{
	public GameObject canvasToClose;
	public ShopManager shopManager;

	public void CloseCanvas()
	{
		if (canvasToClose != null)
		{
			canvasToClose.SetActive(false);
			shopManager.IsOpen = false;
		}
	}
}
