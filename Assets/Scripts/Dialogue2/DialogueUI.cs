using System.Collections;
using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text textLabel;
    private List<Action> actionsOnClose = new();

    internal void ShowDialogue()
    {
        throw new NotImplementedException();
    }

    public void AddResponseEvents(ResponseEvent[] responseEvents)
    {
        responseHandler.AddResponseEvents(responseEvents);
    }

    public bool IsOpen { get; private set; }

    private ResponseHandler responseHandler;
    private TypewriterEffect typewritterEffect;

    private void Start()
    {
        typewritterEffect = GetComponent<TypewriterEffect>();
        responseHandler = GetComponent<ResponseHandler>();

        CloseDialogueBox();
        //ShowDialogue(testDialogue);
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        Debug.Log("show");
        IsOpen = true;
        dialogueBox.SetActive(true);
        //Debug.Log(dialogueBox.activeSelf);
        Debug.Log("0" + dialogueBox.activeSelf);
        StartCoroutine(StepThroughDialogue(dialogueObject));
        Debug.Log("1" + dialogueBox.activeSelf);
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        Debug.Log("2"+ dialogueBox.activeSelf);
        for (int i = 0; i < dialogueObject.Dialogue.Length; i++)
        {
            Debug.Log("3" + dialogueBox.activeSelf);
            string dialogue = dialogueObject.Dialogue[i];

            yield return RunTypingEffect(dialogue);

            textLabel.text = dialogue;

            if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses) {
                Debug.Log("4" + dialogueBox.activeSelf);
                break;
            } 
            yield return null;
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            Debug.Log("5" + dialogueBox.activeSelf);
        }

        if (dialogueObject.HasResponses)
        {
            responseHandler.ShowResponses(dialogueObject.Responses);
            Debug.Log("has response!");
        }
        else
        {
            CloseDialogueBox();
            Debug.Log("don't have response: close!");
        }
    }

    private IEnumerator RunTypingEffect(string dialogue)
    {
        typewritterEffect.Run(dialogue, textLabel);
        while (typewritterEffect.IsRunning)
        {
            yield return null;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                typewritterEffect.Stop();
            }
        }
    }

    public void CloseDialogueBox()
    {
        IsOpen = false;
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;
        foreach (var act in actionsOnClose)
        {
            act();
        }
    }
    
    public void RegisterCloseAction(Action act)
    {
        actionsOnClose.Add(act);
    }
}
