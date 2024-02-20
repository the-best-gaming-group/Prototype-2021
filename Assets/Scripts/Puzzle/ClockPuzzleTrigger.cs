using UnityEngine;

public class ClockPuzzleTrigger : MonoBehaviour
{
    public GameObject clockCanvas;

    private bool playerInTriggerZone = false;
    private bool puzzleActivated = false;


    private void Start()
    {
        clockCanvas.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("HIIIII");
        if (other.CompareTag("Player") && !puzzleActivated)
        {
            // Player entered the trigger zone
            playerInTriggerZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player exited the trigger zone
            playerInTriggerZone = false;
        }
    }

    private void Update()
    {
        // Check for Space key press and player in trigger zone
        if (playerInTriggerZone && Input.GetKeyDown(KeyCode.Space) && !puzzleActivated)
        {
            ActivatePuzzle();
        }
    }

    private void ActivatePuzzle()
    {
        // Show the clock puzzle panel
        clockCanvas.SetActive(true);
        puzzleActivated = true;

        // Optionally, you can pause the game or restrict player movement while the puzzle is active
        // Time.timeScale = 0f; // Pauses the game (optional)

        // Add any additional actions or animations here when the puzzle is activated
    }

    // You can call this function when the player successfully solves the puzzle
    public void PuzzleSolved()
    {
        // Perform any actions you want after the puzzle is solved
        // For example, resume the game
        // Time.timeScale = 1f; // Resumes the game (optional)

        // You might also want to disable the collider to prevent the puzzle from activating again
        // GetComponent<Collider>().enabled = false;

        // Optionally, hide or deactivate the clock puzzle panel
        clockCanvas.SetActive(false);
    }

    public void CloseCanvas()
    {
        puzzleActivated = false;
        clockCanvas.SetActive(false);
    }
}
