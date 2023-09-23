using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class RunePanelController : MonoBehaviour
{
    private const int numChildren = 7;
    public Selectable[] selects = new Selectable[numChildren];
    private int currentlySelected = 0;
    // Start is called before the first frame update
    void Start()
    {
        Selectable[] childSelects = GetComponentsInChildren<Selectable>();
        for (int i = 0; i < childSelects.Length; i++) {
            Debug.Log("We got here! " + i);
            selects[i] = childSelects[i];
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
}
