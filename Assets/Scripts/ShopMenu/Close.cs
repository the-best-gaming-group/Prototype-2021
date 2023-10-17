using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Close : MonoBehaviour
{
	public GameObject canvasToClose;

	public void CloseCanvas()
	{
		if (canvasToClose != null)
		{
			canvasToClose.SetActive(false);
		}
	}
}
