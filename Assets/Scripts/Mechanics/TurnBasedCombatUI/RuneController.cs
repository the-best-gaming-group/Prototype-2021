using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static SpellManager;

public class RuneController : MonoBehaviour
{
    public SpriteState spriteState;
    public enum SpriteType {
        REGULAR,
        HOVER,
        SELECTED,
        DISABLED
    }
    [SerializeField] Button button;
    [SerializeField] Image buttonImage;
    [SerializeField] Image selectImage;
    [SerializeField] SpellSprites sprites;
    private bool isSelected;
    
    public void Start()
    {
        button = GetComponentInChildren<Button>();
        spriteState = button.spriteState;
        buttonImage = transform.Find("Button").GetComponent<Image>();
        selectImage = transform.Find("Select").GetComponent<Image>();
    }

    
    public void Toggle()
    {
        isSelected ^= true;
        if (isSelected)
        {
            buttonImage.sprite = sprites.selected;
        }
        else
        {
            buttonImage.sprite = sprites.regular;
        }
    }
    
    public void ChangeSprites(SpellSprites s)
    {
        if (s == null)
        {
            button.interactable = false;
            return;
        }
        button.interactable = true;
        sprites = s;
        buttonImage.sprite = s.regular;
        spriteState.disabledSprite = s.disabled;
        selectImage.sprite = sprites.hover;
    }
}
