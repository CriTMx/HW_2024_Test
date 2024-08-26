using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class PlayerDeath : MonoBehaviour
{
    public static Action OnPlayerDeath;         // Custom event to invoke to inform other scripts that the player has died

    [SerializeField] private float animationTime = 1.5f;    // Shrinking animation upon death, this determines the duration
    private float curTime = 0f;                             // Stores current time to compare with

    void Update()
    {
        if (gameObject.transform.position.y <= 0)           // If the player falls below y=0, kill player
            StartCoroutine(PlayerDeathAnimationCoroutine());
    }

    // Coroutine to execute animation before player object is destroyed
    IEnumerator PlayerDeathAnimationCoroutine()
    {
        Vector3 curSize = gameObject.transform.localScale;  // Stores player's scale
        Vector3 deathSize = Vector3.zero;                   // Final scale that will be reached

        while (curTime <= animationTime)    // Animate within this duration
        {
            // Linearly interpolate between current size and zero size, gradually starting from 0 till 1
            gameObject.transform.localScale = Vector3.Lerp(curSize, deathSize, curTime/animationTime);

            curTime += Time.deltaTime;  // Update current time
            yield return null;
        }

        Destroy(gameObject);        // Destroy the player object upon death
        OnPlayerDeath?.Invoke();    // Invoke OnPlayerDeath event for other scripts to use
    }
}
