using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RerollController : Selectable
{
    public Image button;
    private Color enableColor = Color.blue;
    private Color disableColor = Color.grey;
    public void Awake()
    {
        button = transform.Find("Button").GetComponent<Image>();
    }
    public void Disable()
    {
        button.color = disableColor;
    }
    
    public void Enable()
    {
        button.color = enableColor;
    }
}
