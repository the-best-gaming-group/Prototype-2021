using UnityEngine;
using UnityEngine.Playables;

public class TimelineSkip : MonoBehaviour
{
    public PlayableDirector playableDirector; // Reference to your Timeline's PlayableDirector

    void Update()
    {
        // Check for input to skip the timeline
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SkipTimeline();
        }
    }

    // Method to skip the timeline
    void SkipTimeline()
    {
        if (playableDirector != null)
        {
            // Jump to the end
            playableDirector.time = playableDirector.duration-0.1;
        }
    }
}
