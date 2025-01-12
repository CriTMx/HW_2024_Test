using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulpitAnimator : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private float animationStartTime = 0.5f;
    public float destroyTime;
    private float remainingTime;
    private Vector3 initialScale = PulpitSpawnHandler.PulpitFixedSize;
    private Vector3 sizeDown;

    private void OnDisable()
    {
        transform.localScale = initialScale;
        remainingTime = destroyTime;
    }

    void Start()
    {
        transform.localScale = initialScale;
        remainingTime = destroyTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (remainingTime > 0)
            remainingTime -= Time.deltaTime;

        if (remainingTime > 0 && remainingTime <= animationStartTime)
        {
            sizeDown = Vector3.one * Time.deltaTime / animationStartTime;
            sizeDown.x *= transform.localScale.x;
            sizeDown.y = 0f;
            sizeDown.z *= transform.localScale.z;

            transform.localScale -= sizeDown;
        }
    }
}
