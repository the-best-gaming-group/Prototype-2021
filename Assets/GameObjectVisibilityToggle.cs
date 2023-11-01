using UnityEngine;

public class GameObjectVisibilityToggle : MonoBehaviour
{
	public GameObject SelectionManager;

	private void Start()
	{
		SelectionManager.SetActive(false); // Start with the GameObject inactive
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Z))
		{
			Debug.Log("active");
			SelectionManager.SetActive(!SelectionManager.activeSelf);
		}
	}
}