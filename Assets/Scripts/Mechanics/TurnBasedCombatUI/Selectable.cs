using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Selectable: MonoBehaviour
{
    public Image selectImage;
    void Start()
    {
        Hide();
    }
    
    void Awake() {
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Hide() {
        selectImage.enabled = false;
    }

    public void Show() {
        selectImage.enabled = true;
    }
    
    public bool IsShowing() {
        return selectImage.enabled;
    }
}
