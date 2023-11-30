using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroTransition : MonoBehaviour
{

    public string sceneToChange;
    // Method to be called when the animation is complete
    public void OnAnimationComplete()
    {
        // Load the next scene
        SceneManager.LoadScene(sceneToChange);
    }
}
