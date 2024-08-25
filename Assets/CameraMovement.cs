using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject player;
    [SerializeField] private float cameraOffsetX = 0f;
    [SerializeField] private float cameraOffsetY = 2.5f;
    [SerializeField] private float cameraOffsetZ = -10f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + new Vector3(cameraOffsetX, cameraOffsetY, cameraOffsetZ);
    }
}
