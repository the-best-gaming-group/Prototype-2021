using UnityEngine;
using UnityEngine.UI;

public class RerollController : Selectable
{
    public Image button;
    private readonly Color enableColor = new(0.3921832f, 0.3925667f, 1);
    private readonly Color disableColor = Color.grey;
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
