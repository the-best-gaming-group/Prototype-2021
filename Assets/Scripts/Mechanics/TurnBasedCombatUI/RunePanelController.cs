using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static Rune;
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
    public bool HasRollsLeft => rollsLeft > 0;
    static public Dictionary<Rune, Color> runeColorMap = new()
    {
        {WATER, Color.blue},
        {FIRE,  Color.red},
        {EARTH, Color.green},
        {AIR,   Color.yellow},
        {USED, Color.black}
    };
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
    
    public void DoReroll(ResourceHandler resourceHandler)
    {
        bool noneSelected = !rerolls.Aggregate((x,y) => x | y);
        if (noneSelected)
        {
            return;
        }

        WipeRerolls();
        ColorRunes(resourceHandler);
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
    
    public void DoReset(ResourceHandler resourceHandler)
    {
        text.text = twoRollsLeft;
        rollsLeft = 2;
        WipeRerolls();
        ColorRunes(resourceHandler);
        rrc.Enable();
    }

    public void ColorRunes(ResourceHandler resourceHandler)
    {
        for (int i = 0; i < 6; i++)
        {
            runes[i].ChangeColor(runeColorMap[resourceHandler.GetRuneTypes()[i]]);
        }
    }
}
