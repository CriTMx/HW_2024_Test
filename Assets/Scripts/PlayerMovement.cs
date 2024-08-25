using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rigidBody;

    private float moveX, moveZ;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        moveX = 0f;
        moveZ = 0f;
    }

    void Update()
    {
        moveX = Input.GetAxis("Horizontal"); // Detect A or D key to move along X-axis
        moveZ = Input.GetAxis("Vertical"); // Detect W or S key to move along Z-axis

        transform.Translate(moveX * Time.deltaTime, 0f, moveZ * Time.deltaTime);
    }
}
