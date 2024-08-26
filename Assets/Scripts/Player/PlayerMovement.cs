using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private float moveX, moveZ;     // Floats to store axis input data
    private float playerSpeed;      // Player speed data obtained from GameDataHandler script

    private void Awake()
    {
        GameDataHandler.OnDataFetchSuccess += InitializePlayerData;
    }

    void Start()
    {
        // Initialize input data to zero when game starts
        moveX = 0f;
        moveZ = 0f;
    }

    void Update()
    {
        // Detect horizontal and vertical axis inputs to move the player
        moveX = Input.GetAxis("Horizontal"); 
        moveZ = Input.GetAxis("Vertical"); 

        // Create a vec3 with required move parameters
        Vector3 moveDir = new(moveX, 0f, moveZ);

        // Rotate move direction by 45 degrees for the orthographic view 
        moveDir = Quaternion.Euler(0f, 45f, 0f) * moveDir;

        // Add motion to the player
        transform.Translate(playerSpeed * Time.deltaTime * moveDir, Space.World); 
    }

    private void OnDestroy()
    {
        GameDataHandler.OnDataFetchSuccess -= InitializePlayerData;
    }

    private void InitializePlayerData()
    {
        // Obtain the player speed data from GameDataHandler
        playerSpeed = GameDataHandler.PlayerSpeed;
    }
}
