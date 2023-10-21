using System;
using Platformer.Mechanics;
using UnityEngine;

public class DialogueActivator : RoomSpawner, IInteractable
{

    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private GameObject visualCue;
    [SerializeField] AudioSource diaSound;
    [SerializeField] private int savedDiagIdx = -1;
    [SerializeField] private DialogueObject[] finalDiags;
    private GhostController ghostpc;

    public void UpdateDialogueObject(DialogueObject dialogueObject)
    {
        if (finalDiags.Length > 0 && (savedDiagIdx = Array.IndexOf(finalDiags, dialogueObject)) >= 0)
        {
            GameManager.Instance.SaveDialogue(uID, savedDiagIdx, ghostpc.transform.position);
        }
        this.dialogueObject = dialogueObject;
    }

    private void Awake()
    {
        visualCue.SetActive(false);
    }

    public override void Start()
    {
        base.Start();
        if (finalDiags.Length > 0 && (savedDiagIdx = GameManager.Instance.SavedDialogueIdx(uID)) >= 0)
        {
            dialogueObject = finalDiags[savedDiagIdx];
        }
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
        this.ghostpc = player;
    }
}
