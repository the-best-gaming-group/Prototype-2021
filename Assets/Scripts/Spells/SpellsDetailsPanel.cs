using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
public class SpellsDetailsPanel : MonoBehaviour
{
    public TextMeshProUGUI spellNameText;
    public Image spellImage;
    public TextMeshProUGUI spellCostText;

    public void ShowDetails(GameManager.Spell spell)
    {
        spellNameText.text = spell.name;
        spellImage.sprite = spell.prefabButton.GetComponent<Image>().sprite; // Assuming the image is part of the button
        spellCostText.text = "Cost: " + string.Join("/", spell.cost.Select(c => c.ToString()));
    }
}
