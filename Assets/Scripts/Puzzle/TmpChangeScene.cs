using UnityEngine;
using UnityEngine.SceneManagement;

public class TmpChangeScene : MonoBehaviour
{
    public GameObject activeCue;
    public string sceneName;

    private bool playerInTriggerZone = false;
    private bool puzzleSolved = false;


    private void Start()
    {
        activeCue.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("HIIIII");
        activeCue.SetActive(true);
        if (other.CompareTag("Player") && !puzzleSolved)
        {
            // Player entered the trigger zone
            playerInTriggerZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        activeCue.SetActive(false);
        if (other.CompareTag("Player"))
        {
            // Player exited the trigger zone
            playerInTriggerZone = false;
        }
    }

    private void Update()
    {
        // Check for Space key press and player in trigger zone
        if (playerInTriggerZone && Input.GetKeyDown(KeyCode.Space) && !puzzleSolved)
        {
            ActivatePuzzle();
        }
    }

    private void ActivatePuzzle()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
