using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 100f; // Adjust the speed as needed.

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the movement direction.
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);

        // Normalize the movement vector to prevent faster diagonal movement.
        movement.Normalize();

        // Apply the movement to the Rigidbody.
        rb.velocity = movement * moveSpeed;
    }
}
