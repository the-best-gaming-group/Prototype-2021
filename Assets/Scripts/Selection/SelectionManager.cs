using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Unity.VisualScripting;

public class SelectionManager : MonoBehaviour
{
	public GameObject selectionPanel;
	public GameObject selectedPanel;
	public GameObject selectionCanvas;
	public GameObject selectedCanvas;
	public static bool GameIsPaused = false;

	private List<Button> spellButtons = new List<Button>();
	private List<GameManager.Spell> selectedSpells = new();

	private void Start()
	{
		Pause();
		PopulateSelectionPanel();
	}

	public void Resume()
	{
		if (selectedSpells.Count == 4)
		{
			selectedCanvas.SetActive(false);
			selectionCanvas.SetActive(false);
			Time.timeScale = 1f;
			GameIsPaused = false;
			var battleSystem = GameObject.Find("BattleSystem").GetComponent<BattleSystem>();
			var spellsAsStrings = selectedSpells.Select(spell => spell.name).ToArray();
            battleSystem.SetupSpells(spellsAsStrings);
			battleSystem.Resume();
		}
	}

	void Pause()
	{
		Time.timeScale = 0f;
		GameIsPaused = true;
	}

	private void PopulateSelectionPanel()
	{
		GameManager gameManager = GameManager.Instance;

		foreach (GameManager.Spell spell in gameManager.spells)
		{
			if (gameManager.AvailableSpells.TryGetValue(spell.name, out bool available) && available)
			{
				Button spellButton = Instantiate(spell.prefabButton, selectionPanel.transform);
				spellButton.onClick.AddListener(() => MoveToSelectedPanel(spell, spellButton));
				spellButtons.Add(spellButton);
			}
		}
	}

	public void MoveToSelectedPanel(GameManager.Spell spell, Button pressedButton)
	{
		if (selectedSpells.Count < 4)
		{
			Button spellButtonPrefab = spell.prefabButton;
			pressedButton.gameObject.SetActive(false);

			Button selectedSpellClone = Instantiate(spellButtonPrefab, selectedPanel.transform);
			selectedSpells.Add(spell);
			selectedSpellClone.onClick.AddListener(() => MoveToSelectionPanel(spell, selectedSpellClone));
		}
	}

	public void MoveToSelectionPanel(GameManager.Spell spell, Button pressedButton)
	{
		selectedSpells.Remove(spell);
		Destroy(pressedButton.gameObject);

		foreach (Button spellButton in spellButtons)
		{
			if (spellButton.name.Equals(pressedButton.name))
			{
				spellButton.gameObject.SetActive(true);
				break;
			}
		}
	}
}
