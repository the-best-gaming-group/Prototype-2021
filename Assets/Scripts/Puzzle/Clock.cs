using UnityEngine;

public class Clock : MonoBehaviour
{
    public Transform minute, hour;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("click");
            minute.Rotate(Vector3.back, 30);
            hour.Rotate(Vector3.back, 2.5f);
        }
    }
}
