using UnityEngine;

public class Clock : MonoBehaviour
{
    public Transform minute, hour;
    public float rotationSpeed = 200f;
    public float targetHourAngle = -30f; // Target angle for 10:30
    public float targetMinuteAngle = 180f; // Target angle for 10:30

    private bool isWinningConditionMet = false;

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

    //private void CheckWinningCondition()
    //{
    //    // Check if the clock hands are close to the target angles for 10:30
    //    float currentHourAngle = hour.localEulerAngles.z;
    //    float currentMinuteAngle = minute.localEulerAngles.z;

    //    if (Mathf.Abs(currentHourAngle - targetHourAngle) < 5f && Mathf.Abs(currentMinuteAngle - targetMinuteAngle) < 5f)
    //    {
    //        Debug.Log("Congratulations! You've won!");
    //        // Set the winning text or perform any other winning actions here

    //        // Optionally, you can stop the clock hands from moving further
    //        isWinningConditionMet = true;
    //    }
    //    else
    //    {
    //        Debug.Log("Not yet at 10:30. Keep adjusting!");
    //    }
    //}

    private void CheckWinningCondition()
    {
        float currentHourAngle = GetNormalizedAngle(hour.localEulerAngles.z);
        float currentMinuteAngle = GetNormalizedAngle(minute.localEulerAngles.z);

        float targetHourAngle = GetNormalizedAngle(43f); // Target angle for 10:30
        float targetMinuteAngle = GetNormalizedAngle(180f); // Target angle for 10:30

        if (Mathf.Abs(currentHourAngle - targetHourAngle) < 5f && Mathf.Abs(currentMinuteAngle - targetMinuteAngle) < 5f)
        {
            Debug.Log("Congratulations! You've won!");
            // Set the winning text or perform any other winning actions here

            // Optionally, you can stop the clock hands from moving further
            isWinningConditionMet = true;
        }
        else
        {
            Debug.Log("Not yet at 10:30. Keep adjusting!");
        }
    }

    private float GetNormalizedAngle(float angle)
    {
        // Adjust the angle to be in the range [0, 360)
        return (angle + 360f) % 360f;
    }

}
