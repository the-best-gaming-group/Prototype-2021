using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroTransition : MonoBehaviour
{
    // Method to be called when the animation is complete
    public void OnAnimationComplete()
    {
        Debug.Log("hi");
        // Load the next scene
        SceneManager.LoadScene("Main Scene 1");
    }
}
