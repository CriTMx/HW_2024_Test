using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private int playerScore = 0;

    void Start()
    {
        scoreText.text = "Score: " + playerScore.ToString();
    }

    void Update()
    {
        scoreText.text = "Score: " + playerScore.ToString();
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
}
