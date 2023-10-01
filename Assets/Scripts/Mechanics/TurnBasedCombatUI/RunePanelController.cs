using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using UnityEngine;

public class RunePanelController : Selectable
{
    private const int numChildren = 7;
    public Selectable[] selects = new Selectable[numChildren];
    public readonly bool[] rerolls = new bool[6];
    public RuneController[] runes = new RuneController[numChildren-1];
    private TextMeshProUGUI text;
    private RerollController rrc;
    private readonly string twoRollsLeft = "2 Rerolls Left";
    private readonly string oneRollLeft = "1 Reroll Left";
    private readonly string noRollseft = "No Rerolls Left";
    private int rollsLeft = 2;
    private int currentlySelected = 0;
    // Start is called before the first frame update
    void Start()
    {
        rrc = transform.Find("Reroll").GetComponent<RerollController>();
        text = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        text.text = twoRollsLeft;
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    public void ChangeSelect(int i)
    {
        selects[currentlySelected].Hide();
        currentlySelected = i;
        selects[currentlySelected].Show();
    }
    
    public void LoseFocus()
    {
        selects[currentlySelected].Hide();
    }

    public void GainFocus()
    {
        selects[currentlySelected].Show();
    }
    
    public void DoReroll(Dictionary<Rune, Color> runeColorMap, ResourceHandler resourceHandler)
    {
        bool noneSelected = true;
        for (int i = 0; i < rerolls.Length; i++)
        {
            if (rerolls[i])
            {
                noneSelected = false;
            }
        }
        if (noneSelected)
        {
            return;
        }

        WipeRerolls();
        ColorRunes(runeColorMap, resourceHandler);
        if (text.text.Equals(twoRollsLeft))
        {
            text.text = oneRollLeft;
            rollsLeft -= 1;
        }
        else if (text.text.Equals(oneRollLeft))
        {
            text.text = noRollseft;
            rollsLeft -= 1;
            rrc.Disable();
        }
    }
    
    public void ToggleRune(int i)
    {
        if (rollsLeft > 0)
        {
            rerolls[i] ^= true;
            runes[i].Toggle();
        }
    }
    
    public void WipeRerolls()
    {
        for (int i = 0; i < 6; i++)
        {
            if (rerolls[i])
            {
                runes[i].Toggle();
                rerolls[i] = false;
            }
        }
    }

        public void ColorRunes(Dictionary<Rune, Color> runeColorMap, ResourceHandler resourceHandler)
        {
            for (int i = 0; i < 6; i++)
            {
                runes[i].ChangeColor(runeColorMap[resourceHandler.runes[i]]);
            }
        }

}
