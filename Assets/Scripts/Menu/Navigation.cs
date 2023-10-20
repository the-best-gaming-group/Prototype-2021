using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Navigation : MonoBehaviour
{
    //public bool hasSlider;

    public Button[] buttons; // Array to store your buttons
    private int selectedButtonIndex = 0; // Index of the currently selected button

    /*
    public Slider volumeSlider; // Reference to the volume slider in the Unity Editor
    public int sliderIndex;
    private bool volumeActive = false;
    private readonly float sliderStep = 0.1f; // The step by which the slider value changes
    */

    private void Start()
    {
        // Set the first button as selected when the MainMenu scene starts
        SelectButton(selectedButtonIndex);
    }

    private void Update()
    {
        // Use W and S keys to navigate through the buttons
        if (Input.GetKeyDown(KeyCode.W))
        {
            // Move to the previous button
            selectedButtonIndex -= 1;
            if (selectedButtonIndex < 0)
            {
                selectedButtonIndex = 0;
            }
            SelectButton(selectedButtonIndex);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            // Move to the next button
            selectedButtonIndex += 1;
            if (selectedButtonIndex >= buttons.Length)
            {
                selectedButtonIndex = buttons.Length - 1;
            }
            SelectButton(selectedButtonIndex);
        }

        // Handle button press when Enter key is pressed
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            // Call the corresponding button's onClick event
            buttons[selectedButtonIndex].onClick.Invoke();
        }

        /*
        if (volumeActive)
        {
            // Use A and D keys to control the slider
            if (Input.GetKeyDown(KeyCode.A))
            {
                // Decrease slider value with a step
                volumeSlider.value = Mathf.Clamp01(volumeSlider.value - sliderStep);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                // Increase slider value with a step
                volumeSlider.value = Mathf.Clamp01(volumeSlider.value + sliderStep);
            }
        }
        */
    }

    // Method to select a button
    private void SelectButton(int index)
    {
        // Deselect all buttons
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }

        // Select the specified button
        buttons[index].Select();
        /*
        if (hasSlider && index == sliderIndex)
        {
            volumeActive = true;
        }
        else
        {
            volumeActive = false;
        }
        */
    }
}