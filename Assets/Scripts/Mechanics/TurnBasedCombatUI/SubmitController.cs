using UnityEngine;
using UnityEngine.UI;

public class SubmitController : MonoBehaviour
{
    [SerializeField] Button button;
    public void Awake()
    {
        button = transform.Find("Button").GetComponent<Button>();
    }
    public void DoDisable()
    {
        button.interactable = false;
    }
    
    public void DoEnable()
    {
        button.interactable = true;
    }
}
