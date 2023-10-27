//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//public class SelectionManager : MonoBehaviour
//{
//    public GameObject selectionPanel;
//    public GameObject selectedPanel;
//    public Button selectButton;
//    public Button doneButton;
//    public Button spellButtonPrefab;

//    void Start()
//    {
//        PopulateSelectionPanel();
//    }

//    void PopulateSelectionPanel()
//    {
//        // Access GameManager.Instance to get the list of spells
//        GameManager gameManager = GameManager.Instance;

//        // Loop through the spells and create UI elements for each spell in the selection panel
//        foreach (GameManager.Spell spell in gameManager.spells)
//        {
//            Debug.Log(spell.name);
//            // Create a UI button for the spell
//            Button spellButton = Instantiate(spellButtonPrefab, selectionPanel.transform);
//            spellButton.GetComponentInChildren<TextMeshProUGUI>().text = spell.name;
//            Debug.Log(spellButton.GetComponentInChildren<TextMeshProUGUI>().text);

//            // Add an onClick listener to select the spell when the button is clicked
//            //spellButton.onClick.AddListener(() => SelectSpell(spell));
//        }
//    }



//}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectionManager : MonoBehaviour
{
    public GameObject selectionPanel;
    public GameObject selectedPanel;
    public Button selectButton;
    public Button doneButton;
    public Button spellButtonPrefab;

    private List<Button> spellButtons = new List<Button>();
    private List<Button> selectedSpells = new List<Button>();

    private void Start()
    {
        PopulateSelectionPanel();
        selectButton.interactable = false; // Initially, the select button is disabled.
        doneButton.onClick.AddListener(SelectDone);
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
            spellButton.onClick.AddListener(() => SelectSpell(spellButton));
            spellButtons.Add(spellButton);
        }
    }

    public void SelectSpell(Button spellButton)
    {
        if (selectedSpells.Contains(spellButton))
        {
            // Deselect the spell if it's already selected
            Debug.Log("Contain spell so remove in the list" + spellButton.GetComponentInChildren<TextMeshProUGUI>().text);
            selectedSpells.Remove(spellButton);
            // You can customize the appearance of deselected buttons here.
            
        }
        else if (selectedSpells.Count < 4)
        {
            // Select the spell if it's not already selected and there's room for more selections
            Debug.Log("Add spell in the list " + spellButton.GetComponentInChildren<TextMeshProUGUI>().text);
            selectedSpells.Add(spellButton);
            // You can customize the appearance of selected buttons here.
        }

        // Enable the select button if there are selected spells, otherwise, keep it disabled
        selectButton.interactable = selectedSpells.Count > 0;
    }

    public void SelectDone()
    {
        // Move the selected buttons from selectionPanel to selectedPanel
        foreach (Button selectedSpellButton in selectedSpells)
        {
            selectedSpellButton.transform.SetParent(selectedPanel.transform);
        }

        // Clear the list of selected spells
        selectedSpells.Clear();

        // Disable the select button after the selection is done
        selectButton.interactable = false;
    }
}
