using Platformer.Mechanics;
using UnityEngine;

public class Close : MonoBehaviour
{
	public GameObject canvasToClose;
	private ShopManager shopManager;

	void Start()
	{
		shopManager = FindObjectOfType<ShopManager>();
	}

	public void CloseCanvas()
	{
		if (canvasToClose != null)
		{
			canvasToClose.SetActive(false);
			shopManager.Resume();
		}
	}
}
