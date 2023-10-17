using Platformer.Mechanics;
using UnityEngine;

public class DialogueActivator : MonoBehaviour, IInteractable
{

    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private GameObject visualCue;
    [SerializeField] AudioSource diaSound;

    public void UpdateDialogueObject(DialogueObject dialogueObject)
    {
        this.dialogueObject = dialogueObject;
    }

    private void Awake()
    {
        visualCue.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out GhostController player))
        {
            visualCue.SetActive(true);
            player.Interactable = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out GhostController player))
        {
            if (player.Interactable is DialogueActivator dialogueActivator && dialogueActivator == this)
            {
                visualCue.SetActive(false);
                player.Interactable = null;
            }
        }
    }

    public void Interact(GhostController player)
    {
        if (TryGetComponent(out DialogueResponseEvents responseEvents) && responseEvents.DialogueObject == dialogueObject)
        {
            player.DialogueUI.AddResponseEvents(responseEvents.Events);
        }
        player.DialogueUI.ShowDialogue(dialogueObject);
        if(diaSound != null)
        {
            diaSound.Play();
        }
        
    }
}
