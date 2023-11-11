using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResponseHandler : MonoBehaviour
{
    [SerializeField] private RectTransform responseBox;
    [SerializeField] private RectTransform responseButtonTemplate;
    [SerializeField] private RectTransform responseContainer;

    private DialogueUI dialogueUI;
    private ResponseEvent[] responseEvents;
    private List<GameObject> tempResponseButtons = new List<GameObject>();
    private int selectedIndex = 0;

    private void Start()
    {
        dialogueUI = GetComponent<DialogueUI>();
    }

    public void AddResponseEvents(ResponseEvent[] responseEvents)
    {
        this.responseEvents = responseEvents;
    }

    private void Update()
    {
        if (dialogueUI.IsOpen)
        {
            ChangeSelectedIndex(0);
            // Check for keyboard input to navigate through response buttons
            if (Input.GetKeyDown(KeyCode.A))
            {
                ChangeSelectedIndex(-1);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                ChangeSelectedIndex(1);
            }
            else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                if (selectedIndex >= 0 && selectedIndex < tempResponseButtons.Count)
                {
                    // Simulate button click when Enter or Space key is pressed
                    tempResponseButtons[selectedIndex].GetComponent<Button>().onClick.Invoke();
                }
            }
        }
    }

    private void ChangeSelectedIndex(int change)
    {
        // Change the selected index based on the input
        selectedIndex += change;
        if (selectedIndex < 0)
        {
            selectedIndex = 0;
        }
        else if (selectedIndex >= tempResponseButtons.Count)
        {
            selectedIndex = tempResponseButtons.Count - 1;
        }

        // Update text style for all response buttons
        for (int i = 0; i < tempResponseButtons.Count; i++)
        {
            TMP_Text text = tempResponseButtons[i].GetComponentInChildren<TMP_Text>();
            if (i == selectedIndex)
            {
                // Highlight the selected button by making the text bold
                text.fontStyle = FontStyles.Bold;
                text.fontSize = 35;
            }
            else
            {
                // Reset the text style for other buttons
                text.fontStyle = FontStyles.Normal;
                text.fontSize = 30;
            }
        }
    }


    public void ShowResponses(Response[] responses, bool needCondition)
    {
        float responseBoxWidth = 0;

        for (int i = 0; i < responses.Length; i++)
        {
            Response response = responses[i];
            int responseIndex = i;

            GameObject responseButton = Instantiate(responseButtonTemplate.gameObject, responseContainer);
            responseButton.gameObject.SetActive(true);
            responseButton.GetComponent<TMP_Text>().text = response.ResponseText;
            responseButton.GetComponent<Button>().onClick.AddListener(() => OnPickedResponse(response, responseIndex, needCondition));

            tempResponseButtons.Add(responseButton);

            responseBoxWidth += responseButtonTemplate.sizeDelta.x;
            //Debug.Log("ShowResponses end");
        }

        responseBox.sizeDelta = new Vector2(responseBoxWidth, responseBox.sizeDelta.y);
        responseBox.gameObject.SetActive(true);
    }

    private void OnPickedResponse(Response response, int responseIndex, bool needCondition)
    {
        if (needCondition)
        {
            responseBox.gameObject.SetActive(false);
            foreach (GameObject button in tempResponseButtons)
            {
                Destroy(button);
            }
            tempResponseButtons.Clear();
            dialogueUI.CloseDialogueBox();
            responseEvents[responseIndex].OnPickedResponse?.Invoke();
            responseEvents = null;
        }
        else
        {
            //Debug.Log("response on click");
            responseBox.gameObject.SetActive(false);
            foreach (GameObject button in tempResponseButtons)
            {
                Destroy(button);
            }
            tempResponseButtons.Clear();

            //Debug.Log("event check");
            if (responseEvents != null && responseIndex <= responseEvents.Length)
            {
                //Debug.Log("event detecked");
                responseEvents[responseIndex].OnPickedResponse?.Invoke();
            }

            responseEvents = null;

            if (response.DialogueObject)
            {
                dialogueUI.ShowDialogue(response.DialogueObject);
            }
            else
            {
                //Debug.Log("no following dialogue");
                dialogueUI.CloseDialogueBox();
            }
        }
    }
}
