using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellController : MonoBehaviour
{
    public GameManager.Spell spell;
    public Button spellCard;
    
    public int idx;
    public Func<int, string> onClick;
    
    public void SetSpell(GameManager.Spell spell, int idx, Func<int, string> onClick)
    {
        this.spell = spell;
        spellCard = Instantiate(spell.prefabButton, transform);
        spellCard.transform.localPosition = new Vector3(0, 0, 0);
        spellCard.navigation = new Navigation{ mode = Navigation.Mode.None };
        this.idx = idx;
        this.onClick = onClick;
        spellCard.onClick.AddListener(doClick);
    }
    
    public void DoEnable()
    {
        spellCard.interactable = true;
    }

    public void DoDisable()
    {
        spellCard.interactable = false;
    }
    
    public void doClick()
    {
        onClick(idx);
    }
}
