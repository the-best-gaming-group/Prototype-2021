using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class RunePanelController : Selectable
{
    private const int numChildren = 7;
    public Selectable[] selects = new Selectable[numChildren];
    public RuneController[] runes = new RuneController[numChildren-1];
    private int currentlySelected = 0;
    // Start is called before the first frame update
    void Start()
    {
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
}
