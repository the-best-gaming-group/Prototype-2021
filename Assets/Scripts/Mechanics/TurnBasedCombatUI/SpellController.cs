using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellController : MonoBehaviour
{
    private readonly Color enableColor = new (0.7495804f, 0.9622642f, 0.8802516f);
    private readonly Color disableColor = Color.grey;

    public Image image;
    public TextMeshProUGUI text;
    public bool IsEnabled => image.enabled;
    public Image[] runes;
    // Start is called before the first frame update
    void Awake()
    {
        image = transform.Find("Button").GetComponent<Image>();
        DoEnable();
        runes = transform.Find("Runes").GetComponentsInChildren<Image>();
        foreach (var rune in runes)
        {
            rune.enabled = false;
        }
        text = image.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DoDisable()
    {
        image.color = disableColor;
    }
    
    public void DoEnable()
    {
        image.color = enableColor;
    }
    
    public void SetCost(Spell s)
    {
        var rcm = RunePanelController.runeSpriteMap;
        int[] costLeft = new int[4];
        Array.Copy(s.cost, costLeft, 4);
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (costLeft[j]-- > 0)
                {
                    runes[i].enabled = true;
                    /* TODO: runes[i].sprite = rcm[(Rune)j]; */
                    break;
                }
            }
        }
    }
}
