using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private GameObject player;             // stores player object to link camera movement with player movement
    [SerializeField] private float cameraOffsetX = 0f;      // Camera offset values to determine fixed position away from player
    [SerializeField] private float cameraOffsetY = 2.5f;
    [SerializeField] private float cameraOffsetZ = -10f;

    private bool isDetached = false;    // to Change when the camera needs to stop following the player

    private void Awake()
    {
        // Add detach method to player death event
        PlayerDeath.OnPlayerDeath += DetachCamera;
    }

    void Update()
    {
        // Only follow player position if player is still alive
        if (!isDetached)
            transform.position = player.transform.position + new Vector3(cameraOffsetX, cameraOffsetY, cameraOffsetZ);
    }

    private void OnDestroy()
    {
        // remove detach method from player death event
        PlayerDeath.OnPlayerDeath -= DetachCamera;
    }

    private void DetachCamera()
    {
        // Stop following player when this is called
        isDetached = true;
    }
}
