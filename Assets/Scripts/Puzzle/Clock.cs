using UnityEngine;
using TMPro;
using System.Collections;

public class Clock : MonoBehaviour
{
    public Transform minute, hour;
    public float rotationSpeed = 200f;
    public float targetHourAngle = -30f; // Target angle for 10:30
    public float targetMinuteAngle = 180f; // Target angle for 10:30

    private bool isWinningConditionMet = false;

    private AudioSource audioSource;

    public TextMeshProUGUI resultText; // Reference to the UI Text component

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!isWinningConditionMet)
        {
            if (Input.GetKey(KeyCode.E))
            {
                RotateClockwise();
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                RotateCounterclockwise();
            }

            // Check winning condition when pressing the "check" button (for example, Space key)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CheckWinningCondition();
            }
        }
    }

    private void RotateClockwise()
    {
        minute.Rotate(Vector3.back, rotationSpeed * Time.deltaTime);
        hour.Rotate(Vector3.back, rotationSpeed * 0.083f * Time.deltaTime);
    }

    private void RotateCounterclockwise()
    {
        minute.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        hour.Rotate(Vector3.forward, rotationSpeed * 0.083f * Time.deltaTime);
    }

    private void CheckWinningCondition()
    {
        float currentHourAngle = GetNormalizedAngle(hour.localEulerAngles.z);
        float currentMinuteAngle = GetNormalizedAngle(minute.localEulerAngles.z);

        float targetHourAngle = GetNormalizedAngle(43f); // Target angle for 10:30
        float targetMinuteAngle = GetNormalizedAngle(180f); // Target angle for 10:30

        if (Mathf.Abs(currentHourAngle - targetHourAngle) < 5f && Mathf.Abs(currentMinuteAngle - targetMinuteAngle) < 5f)
        {
            audioSource.Stop();
            audioSource.Play();
            resultText.text = "Congratulations! This is the right time!";
            isWinningConditionMet = true;
        }
        else
        {
            resultText.text = "Not yet at the right time. Keep adjusting.";
            StartCoroutine(ClearResultTextAfterDelay(2f));
        }
    }

    private float GetNormalizedAngle(float angle)
    {
        // Adjust the angle to be in the range [0, 360)
        return (angle + 360f) % 360f;
    }

    private IEnumerator ClearResultTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        resultText.text = "";
    }
}
