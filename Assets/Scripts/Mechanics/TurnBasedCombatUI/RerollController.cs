using UnityEngine;
using UnityEngine.UI;

public class RerollController : MonoBehaviour
{
    Button button;
    public void Start()
    {
        button = transform.Find("Button").GetComponent<Button>();
    }
    public void Disable()
    {
        button.interactable = false;
    }
    
    public void Enable()
    {
        button.interactable = true;
    }
}
