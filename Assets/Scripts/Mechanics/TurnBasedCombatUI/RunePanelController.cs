using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class RunePanelController : Selectable
{
    private const int numChildren = 7;
    public Selectable[] selects;
    private int currentlySelected = 0;
    // Start is called before the first frame update
    void Start()
    {
        selects = new Selectable[numChildren];
        Selectable[] childSelects = GetComponentsInChildren<Selectable>();
        for (int i = 0; i < numChildren; i++) {
            selects[i] = childSelects[i+1]; // +1 because this itself pops up as well
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ChangeSelect(int i) {
        selects[currentlySelected].Hide();
        currentlySelected = i;
        selects[currentlySelected].Show();
    }
    
    public void LoseFocus() {
        selects[currentlySelected].Hide();
    }
    public void GainFocus() {
        selects[currentlySelected].Show();
    }
}
