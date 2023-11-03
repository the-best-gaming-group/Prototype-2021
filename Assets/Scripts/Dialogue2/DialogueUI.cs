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
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        //Debug.Log("show dialogue");
        //Debug.Log("need condition: " + dialogueObject.needCondition);
        IsOpen = true;
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        if (dialogueObject.needCondition)
        {
            //Debug.Log("need condition!");
            for (int i = 0; i < dialogueObject.Dialogue.Length; i++)
            {
                string dialogue = dialogueObject.Dialogue[i];

                yield return RunTypingEffect(dialogue);

                textLabel.text = dialogue;

                if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses)
                {
                    break;
                }
            }
            //Debug.Log("has response");
            responseHandler.ShowResponses(dialogueObject.Responses, dialogueObject.needCondition);
        }
        else
        {
            //Debug.Log("don't need condition!");
            for (int i = 0; i < dialogueObject.Dialogue.Length; i++)
            {
                string dialogue = dialogueObject.Dialogue[i];

                yield return RunTypingEffect(dialogue);

                textLabel.text = dialogue;

                if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses)
                {
                    break;
                }
                yield return null;
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            }

            if (dialogueObject.HasResponses)
            {
                //Debug.Log("has response");
                responseHandler.ShowResponses(dialogueObject.Responses, dialogueObject.needCondition);
            }
            else
            {
                //Debug.Log("don't have response");
                CloseDialogueBox();
            }
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
        //Debug.Log("close dialogue");
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