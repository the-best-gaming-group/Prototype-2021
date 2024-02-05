using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Unity.VisualScripting;

public class SpellsManager : MonoBehaviour
{
	public GameObject spellsPanel;
	public static bool GameIsPaused = false;
	private List<Button> spellButtons = new List<Button>();
	public SpellsDetailsPanel spellDetailsPanelPrefab;

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
		spellsPanel.SetActive(false);
		Time.timeScale = 1f;
		GameIsPaused = false;
	}

	void Pause()
	{
		spellsPanel.SetActive(true);
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
	}

	private void ClearPanel()
	{
		foreach (Button button in spellButtons)
		{
			Destroy(button.gameObject);
		}

		spellButtons.Clear();
	}

	private void ShowDetailsPanel(GameManager.Spell spell)
	{
		// Instantiate the details panel if not already instantiated
		if (spellDetailsPanel == null)
		{
			spellDetailsPanel = Instantiate(spellDetailsPanelPrefab);
		}

		// Show details in the panel
		spellDetailsPanel.ShowDetails(spell);

		// Set the panel as active
		spellDetailsPanel.gameObject.SetActive(true);
	}

}
