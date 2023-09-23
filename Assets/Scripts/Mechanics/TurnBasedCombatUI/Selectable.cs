using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Selectable: MonoBehaviour
{
    private SpriteRenderer selectSpriteRenderer;
    void Start()
    {
        selectSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        selectSpriteRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Hide() {
        selectSpriteRenderer.enabled = false;
    }

    public void Show() {
        selectSpriteRenderer.enabled = true;
    }
    
    public bool IsShowing() {
        return selectSpriteRenderer.enabled;
    }
}
