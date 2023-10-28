using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectionManager : MonoBehaviour
{
	public GameObject selectionPanel;
	public GameObject selectedPanel;
	public Button spellButtonPrefab;

	private List<Button> spellButtons = new List<Button>();
	private List<Button> selectedSpells = new List<Button>();

	private void Start()
	{
		PopulateSelectionPanel();
	}

	private void PopulateSelectionPanel()
	{
		// Access GameManager.Instance to get the list of spells
		GameManager gameManager = GameManager.Instance;

		// Loop through the spells and create UI elements for each spell in the selection panel
		foreach (GameManager.Spell spell in gameManager.spells)
		{
			Button spellButton = Instantiate(spellButtonPrefab, selectionPanel.transform);
			spellButton.GetComponentInChildren<TextMeshProUGUI>().text = spell.name;
			spellButton.onClick.AddListener(() => MoveToSelectedPanel(spellButton));
			spellButtons.Add(spellButton);
		}
	}

	public void MoveToSelectedPanel(Button spellButton)
	{
		if (selectedSpells.Count < 4)
		{
			spellButton.gameObject.SetActive(false);

			// Add the selected spell to the selectedPanel
			Button selectedSpellClone = Instantiate(spellButtonPrefab, selectedPanel.transform);
			TextMeshProUGUI cloneText = selectedSpellClone.GetComponentInChildren<TextMeshProUGUI>();
			cloneText.text = spellButton.GetComponentInChildren<TextMeshProUGUI>().text;
			selectedSpells.Add(selectedSpellClone);
			selectedSpellClone.onClick.AddListener(() => MoveToSelectionPanel(selectedSpellClone));
		}
	}

	public void MoveToSelectionPanel(Button selectedSpell)
	{
		// Remove the selected spell from the selectedPanel
		selectedSpells.Remove(selectedSpell);
		Destroy(selectedSpell.gameObject);

		// Find the corresponding spell in the selectionPanel and make it visible
		foreach (Button spellButton in spellButtons)
		{
			Debug.Log("spellButton.GetComponentInChildren<TextMeshProUGUI>().text" + spellButton.GetComponentInChildren<TextMeshProUGUI>().text);
			Debug.Log("selectedSpell.GetComponentInChildren<TextMeshProUGUI>().text" + selectedSpell.GetComponentInChildren<TextMeshProUGUI>().text);
			if (spellButton.GetComponentInChildren<TextMeshProUGUI>().text == selectedSpell.GetComponentInChildren<TextMeshProUGUI>().text)
			{
				Debug.Log(spellButton.GetComponentInChildren<TextMeshProUGUI>().text);
				spellButton.gameObject.SetActive(true);
				break;
			}
		}
	}
}