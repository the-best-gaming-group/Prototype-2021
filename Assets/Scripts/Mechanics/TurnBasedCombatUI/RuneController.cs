using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;

public class RuneController : Selectable
{
    public Image i;

    public void Awake() {
        i = transform.Find("Rune").gameObject.GetComponent<Image>();
    }
    
    void Update() {
    }
    
    public void ChangeColor(Color c)
    {
        i.color = c;
    }
}
