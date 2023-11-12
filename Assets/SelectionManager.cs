// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;
// using System.Linq;

// public class SelectionManager : MonoBehaviour
// {
// 	public GameObject selectionPanel;
// 	public GameObject selectedPanel;
// 	public GameObject selectionCanvas;
// 	public Button spellButtonPrefab;
// 	public static bool GameIsPaused = false;

// 	private List<Button> spellButtons = new List<Button>();
// 	private List<Button> selectedSpells = new List<Button>();

// 	private void Start()
// 	{
// 		StartCoroutine(Waitforanimation());
// 		Pause();
// 		PopulateSelectionPanel();
// 	}

// 	IEnumerator Waitforanimation()
// 	{
// 		yield return new WaitForSeconds(0.5f);
// 	}


// 	public void Resume()
// 	{
// 		if (selectedSpells.Count == 4)
// 		{
// 			selectionCanvas.SetActive(false);
// 			Time.timeScale = 1f;
// 			GameIsPaused = false;
// 			var battleSystem = GameObject.Find("BattleSystem").GetComponent<BattleSystem>();
// 			var spellsAsStrings = selectedSpells.Select(spell => spell.GetComponentInChildren<TextMeshProUGUI>().text).ToArray();
//             battleSystem.SetupSpells(spellsAsStrings);
// 		}
// 	}

// 	void Pause()
// 	{
// 		selectionCanvas.SetActive(true);
// 		Time.timeScale = 0f;
// 		GameIsPaused = true;
// 	}

// 	private void PopulateSelectionPanel()
// 	{
// 		// Access GameManager.Instance to get the list of spells
// 		GameManager gameManager = GameManager.Instance;

// 		// Loop through the spells and create UI elements for each spell in the selection panel
// 		foreach (GameManager.Spell spell in gameManager.spells)
// 		{
// 			Button spellButton = Instantiate(spellButtonPrefab, selectionPanel.transform);
// 			spellButton.GetComponentInChildren<TextMeshProUGUI>().text = spell.name;
// 			spellButton.onClick.AddListener(() => MoveToSelectedPanel(spellButton));
// 			spellButtons.Add(spellButton);
// 		}
// 	}

// 	public void MoveToSelectedPanel(Button spellButton)
// 	{
// 		if (selectedSpells.Count < 4)
// 		{
// 			spellButton.gameObject.SetActive(false);

// 			// Add the selected spell to the selectedPanel
// 			Button selectedSpellClone = Instantiate(spellButtonPrefab, selectedPanel.transform);
// 			TextMeshProUGUI cloneText = selectedSpellClone.GetComponentInChildren<TextMeshProUGUI>();
// 			cloneText.text = spellButton.GetComponentInChildren<TextMeshProUGUI>().text;
// 			selectedSpells.Add(selectedSpellClone);
// 			selectedSpellClone.onClick.AddListener(() => MoveToSelectionPanel(selectedSpellClone));
// 		}
// 	}

// 	public void MoveToSelectionPanel(Button selectedSpell)
// 	{
// 		// Remove the selected spell from the selectedPanel
// 		selectedSpells.Remove(selectedSpell);
// 		Destroy(selectedSpell.gameObject);

// 		// Find the corresponding spell in the selectionPanel and make it visible
// 		foreach (Button spellButton in spellButtons)
// 		{
// 			Debug.Log("spellButton.GetComponentInChildren<TextMeshProUGUI>().text" + spellButton.GetComponentInChildren<TextMeshProUGUI>().text);
// 			Debug.Log("selectedSpell.GetComponentInChildren<TextMeshProUGUI>().text" + selectedSpell.GetComponentInChildren<TextMeshProUGUI>().text);
// 			if (spellButton.GetComponentInChildren<TextMeshProUGUI>().text == selectedSpell.GetComponentInChildren<TextMeshProUGUI>().text)
// 			{
// 				Debug.Log(spellButton.GetComponentInChildren<TextMeshProUGUI>().text);
// 				spellButton.gameObject.SetActive(true);
// 				break;
// 			}
// 		}
// 	}
// }


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//public class SelectionManager : MonoBehaviour
//{
//	public GameObject selectionPanel;
//	public GameObject selectedPanel;
//	public GameObject selectionCanvas;
//	public Button spellButtonPrefab;
//	public static bool GameIsPaused = false;

//	private List<Button> spellButtons = new List<Button>();
//	private List<Button> selectedSpells = new List<Button>();

//	private void Start()
//	{
//		StartCoroutine(Waitforanimation());
//		Pause();
//		PopulateSelectionPanel();
//	}

//	IEnumerator Waitforanimation()
//	{
//		yield return new WaitForSeconds(0.5f);
//	}


//	public void Resume()
//	{
//		if (selectedSpells.Count == 4)
//		{
//			selectionCanvas.SetActive(false);
//			Time.timeScale = 1f;
//			GameIsPaused = false;
//		}
//	}

//	void Pause()
//	{
//		selectionCanvas.SetActive(true);
//		Time.timeScale = 0f;
//		GameIsPaused = true;
//	}

//	private void PopulateSelectionPanel()
//	{
//		// Access GameManager.Instance to get the list of spells
//		GameManager gameManager = GameManager.Instance;

//		// Loop through the spells and create UI elements for each spell in the selection panel
//		foreach (GameManager.Spell spell in gameManager.spells)
//		{
//			Button spellButton = Instantiate(spellButtonPrefab, selectionPanel.transform);
//			spellButton.GetComponentInChildren<TextMeshProUGUI>().text = spell.name;
//			spellButton.onClick.AddListener(() => MoveToSelectedPanel(spellButton));
//			spellButtons.Add(spellButton);
//		}
//	}

//	public void MoveToSelectedPanel(Button spellButton)
//	{
//		if (selectedSpells.Count < 4)
//		{
//			spellButton.gameObject.SetActive(false);

//			// Add the selected spell to the selectedPanel
//			Button selectedSpellClone = Instantiate(spellButtonPrefab, selectedPanel.transform);
//			TextMeshProUGUI cloneText = selectedSpellClone.GetComponentInChildren<TextMeshProUGUI>();
//			cloneText.text = spellButton.GetComponentInChildren<TextMeshProUGUI>().text;
//			selectedSpells.Add(selectedSpellClone);
//			selectedSpellClone.onClick.AddListener(() => MoveToSelectionPanel(selectedSpellClone));
//		}
//	}

//	public void MoveToSelectionPanel(Button selectedSpell)
//	{
//		// Remove the selected spell from the selectedPanel
//		selectedSpells.Remove(selectedSpell);
//		Destroy(selectedSpell.gameObject);

//		// Find the corresponding spell in the selectionPanel and make it visible
//		foreach (Button spellButton in spellButtons)
//		{
//			Debug.Log("spellButton.GetComponentInChildren<TextMeshProUGUI>().text" + spellButton.GetComponentInChildren<TextMeshProUGUI>().text);
//			Debug.Log("selectedSpell.GetComponentInChildren<TextMeshProUGUI>().text" + selectedSpell.GetComponentInChildren<TextMeshProUGUI>().text);
//			if (spellButton.GetComponentInChildren<TextMeshProUGUI>().text == selectedSpell.GetComponentInChildren<TextMeshProUGUI>().text)
//			{
//				Debug.Log(spellButton.GetComponentInChildren<TextMeshProUGUI>().text);
//				spellButton.gameObject.SetActive(true);
//				break;
//			}
//		}
//	}
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class SelectionManager : MonoBehaviour
{
	public GameObject selectionPanel;
	public GameObject selectedPanel;
	public GameObject selectionCanvas;
	public List<Button> spellButtonPrefabs; // List of spell button prefabs
	public static bool GameIsPaused = false;

	private List<Button> spellButtons = new List<Button>();
	private List<Button> selectedSpells = new List<Button>();

	private void Start()
	{
		Pause();
		PopulateSelectionPanel();
	}

	public void Resume()
	{
		if (selectedSpells.Count == 4)
		{
			selectionCanvas.SetActive(false);
			Time.timeScale = 1f;
			GameIsPaused = false;
			var battleSystem = GameObject.Find("BattleSystem").GetComponent<BattleSystem>();
			var spellsAsStrings = selectedSpells.Select(spell => spell.GetComponentInChildren<TextMeshProUGUI>().text).ToArray();
            battleSystem.SetupSpells(spellsAsStrings);
		}
	}

	void Pause()
	{
		selectionCanvas.SetActive(true);
		Time.timeScale = 0f;
		GameIsPaused = true;
	}

	private void PopulateSelectionPanel()
	{
		GameManager gameManager = GameManager.Instance;

		foreach (GameManager.Spell spell in gameManager.spells)
		{
			Button spellButtonPrefab = spellButtonPrefabs.Find(prefab => prefab.name == spell.name + "Button");
			if (spellButtonPrefab != null)
			{
				Button spellButton = Instantiate(spellButtonPrefab, selectionPanel.transform);
				spellButton.onClick.AddListener(() => MoveToSelectedPanel(spellButton));
				spellButtons.Add(spellButton);
			}
		}
	}

	public void MoveToSelectedPanel(Button spellButton)
	{
		if (selectedSpells.Count < 4)
		{
			Button spellButtonPrefab = spellButtonPrefabs.Find(prefab => prefab.name + "(Clone)" == spellButton.name );

			if (spellButtonPrefab != null)
			{
				spellButton.gameObject.SetActive(false);

				Button selectedSpellClone = Instantiate(spellButtonPrefab, selectedPanel.transform);
				selectedSpells.Add(selectedSpellClone);
				selectedSpellClone.onClick.AddListener(() => MoveToSelectionPanel(selectedSpellClone));
			}
			else
			{
				Debug.LogError("Spell button prefab not found for: " + spellButton.name);
			}
		}
	}

	public void MoveToSelectionPanel(Button selectedSpell)
	{
		selectedSpells.Remove(selectedSpell);
		Destroy(selectedSpell.gameObject);

		foreach (Button spellButton in spellButtons)
		{
			if (spellButton.name == selectedSpell.name)
			{
				spellButton.gameObject.SetActive(true);
				break;
			}
		}
	}
}
