using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RuneController : Hoverable
{
    public Image i;
    public TextMeshProUGUI x;

    public void Awake() {
        i = transform.Find("Rune").gameObject.GetComponent<Image>();
        x = transform.Find("Rolling").gameObject.GetComponent<TextMeshProUGUI>();
        x.enabled = false;
    }
    
    void Update() {
    }
    
    public void ChangeColor(Color c)
    {
        i.color = c;
    }
    
    public void Toggle()
    {
        x.enabled ^= true;
    }
}
