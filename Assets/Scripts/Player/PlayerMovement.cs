using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private float moveX, moveZ;
    private float playerSpeed;

    private void Awake()
    {
        GameDataHandler.OnDataFetchSuccess += InitializePlayerData;
    }

    void Start()
    {
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

        // Add motion to the player
        transform.Translate(playerSpeed * Time.deltaTime * moveDir, Space.World); 
    }

    private void OnDestroy()
    {
        GameDataHandler.OnDataFetchSuccess -= InitializePlayerData;
    }

    private void InitializePlayerData()
    {
        playerSpeed = GameDataHandler.PlayerSpeed;
    }
}
