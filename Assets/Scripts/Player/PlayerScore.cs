using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerScore : MonoBehaviour
{
    // To hold player score and game time values for the scoreboard
    private static int playerScore = 0;
    private static float gameTime = 0f;


    void Update()
    {
        // Update game time
        gameTime += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        /* If collision object is a pulpit,
         * and ONLY if the player hasn't 
         * touched the same pulpit instance 
         * before, update player score */

        if (collision.gameObject.CompareTag("Pulpit") && 
            !collision.gameObject.GetComponent<PulpitBehavior>().hasContactedPlayer)
        {
            playerScore++;
        }
    }

    public static int GetScore() => playerScore;
    public static float GetGameTime() => gameTime;
 
}
