using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class SpellsManager : MonoBehaviour
{
	public GameObject panel;
	public GameObject spellsPanel;
	public GameObject spellDetailsPanel;
	public GameObject inventoryPanel;
	public GameObject inventoryDetailsPanel;
	public static bool GameIsPaused = false;
	private List<Button> spellButtons = new List<Button>();
	private List<Button> inventoryButtons = new List<Button>();


	void Start()
	{
		panel.SetActive(false);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.I))
		{
			if (GameIsPaused)
			{
				Resume();
				ClearPanel();
			}
			else
			{
				Pause();
				PopulatePanel();
			}
		}
	}

	public void Resume()
	{
		panel.SetActive(false);
		Time.timeScale = 1f;
		GameIsPaused = false;
		spellDetailsPanel.gameObject.SetActive(false);
		inventoryDetailsPanel.gameObject.SetActive(false);

	}

	void Pause()
	{
		panel.SetActive(true);
		Time.timeScale = 0f;
		GameIsPaused = true;
	}

	private void PopulatePanel()
	{
		GameManager gameManager = GameManager.Instance;

		foreach (GameManager.Spell spell in gameManager.spells)
		{
			if (gameManager.AvailableSpells.TryGetValue(spell.name, out bool available) && available)
			{
				Button spellButton = Instantiate(spell.prefabButton, spellsPanel.transform);
				spellButtons.Add(spellButton);

				spellButton.onClick.AddListener(() => ShowDetailsPanel(spell));
			}
		}

		List<string> items = gameManager.GetItmes();

		foreach (var item in items)
		{
			
		}
	}


	private void ClearPanel()
	{
		foreach (Button button in spellButtons)
		{
			Destroy(button.gameObject);
		}

		spellButtons.Clear();

		foreach (Button button in inventoryButtons)
		{
			Destroy(button.gameObject);
		}

		inventoryButtons.Clear();
	}

	private void ShowDetailsPanel(GameManager.Spell spell)
	{
		SpellsDetailsPanel detailsPanel = spellDetailsPanel.GetComponent<SpellsDetailsPanel>();
		if (detailsPanel != null)
		{
			detailsPanel.gameObject.SetActive(true);
			detailsPanel.ShowDetails(spell);
		}
		else
		{
			Debug.LogError("SpellsDetailsPanel component not found on spellDetailsPanel GameObject.");
		}
	}

}
