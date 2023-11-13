using System;
using System.Collections;
using System.Collections.Generic;
using Platformer.Mechanics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Lumin;
using UnityEngine.UI;
using static Platformer.Mechanics.TurnbasedDialogHandler;

public class SelectableButton : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] Button buttonButton;
    [SerializeField] Image buttonImage;
    [SerializeField] Image selectImage;
    public ButtonType buttonType;
    private Action onClick;
    [SerializeField] TurnbasedDialogHandler tdh;
    // Start is called before the first frame update
    public bool IsInteractable => buttonButton.interactable;
    public void Awake()
    {
        selectImage.enabled = false;
    }
    
    public void Start()
    {
        tdh.RegisterSelectableButton(this);
    }

    // Update is called once per frame
    public void Update()
    {
        
    }
    
    public void Toggle()
    {
        buttonButton.interactable ^= true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(transform.gameObject);
        selectImage.enabled = true;
    }
    
    public void OnSelect(BaseEventData eventData)
    {
        selectImage.enabled = true;
        tdh.SetSelectedButton(this);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selectImage.enabled = false;
    }
    
    public void SetAction(Action a)
    {
        onClick = a;
    }
    
    public void DoClick()
    {
        onClick();
    }
    
    public void EnableSelectImage()
    {
        if (selectImage != null)
        {
            selectImage.enabled = true;
        }
    }
    public void DisableSelectImage()
    {
        if (selectImage != null)
        {
            selectImage.enabled = false;
        }
    }
}
