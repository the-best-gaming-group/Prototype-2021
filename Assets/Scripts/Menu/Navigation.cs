using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Navigation : MonoBehaviour
{
    public Button[] buttons; // Array to store your buttons
    private int selectedButtonIndex = 0; // Index of the currently selected button

    public bool hasSlider;
    public Slider volumeSlider; // Reference to the volume slider in the Unity Editor
    public int sliderIndex;
    private bool volumeActive = false;    

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

        if (volumeActive)
        {
            // Use A and D keys to control the slider
            if (Input.GetKeyDown(KeyCode.A))
            {
                // Decrease volume
                volumeSlider.value -= 5.0f; // You can adjust the step as needed
                if (volumeSlider.value < -80.0f)
                {
                    volumeSlider.value = -80.0f;
                }
                // Call the OnSliderValueChanged method to update volume and PlayerPrefs
                volumeSlider.GetComponent<VolumeSlider>().OnSliderValueChanged(volumeSlider.value);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                // Increase volume
                volumeSlider.value += 5.0f; // You can adjust the step as needed
                if (volumeSlider.value > 1.0f)
                {
                    volumeSlider.value = 1.0f;
                }
                // Call the OnSliderValueChanged method to update volume and PlayerPrefs
                volumeSlider.GetComponent<VolumeSlider>().OnSliderValueChanged(volumeSlider.value);
            }
        }
    }

    // Method to select a button
    private void SelectButton(int index)
    {
        // If the menu has volume setting
        if (hasSlider && index == sliderIndex)
        {
            volumeActive = true;
        }
        else
        {
            volumeActive = false;
        }

        // Deselect all buttons
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }

        // Select the specified button
        buttons[index].Select();
    }
}
