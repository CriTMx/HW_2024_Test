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
        moveX = Input.GetAxis("Horizontal"); // Detect any horizontal key or button to move along X-axis
        moveZ = Input.GetAxis("Vertical"); // Detect any vertical key or button to move along Z-axis

        transform.Translate(moveX * playerSpeed * Time.deltaTime, 0f, moveZ * playerSpeed * Time.deltaTime);
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
