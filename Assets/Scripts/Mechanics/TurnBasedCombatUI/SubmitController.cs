using UnityEngine;
using UnityEngine.UI;

public class SubmitController : Selectable
{
    public Image button;
    private readonly Color enableColor = new (.4f, .4f, 0.9568628f);
    private readonly Color disableColor = Color.grey;
    public bool IsEnabled => button.color == enableColor;
    public void Awake()
    {
        button = transform.Find("Button").GetComponent<Image>();
    }
    public void DoDisable()
    {
        button.color = disableColor;
    }
    
    public void DoEnable()
    {
        button.color = enableColor;
    }
}
