using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PulpitBehavior : MonoBehaviour
{
    public float destroyTime;
    public float remainingLifetime;
    public bool hasContactedPlayer = false; // Boolean that tracks whether this pulpit instance has touched player before

    [SerializeField] private TextMeshProUGUI timerText; // TMP text component to display remaining alive time of pulpit

    private void Start()
    {
        remainingLifetime = destroyTime;
    }

    void Update()
    {
        remainingLifetime -= Time.deltaTime;  // Subtract deltaTime from remaining alive time of pulpit
        timerText.text = remainingLifetime.ToString("#.#") + "s"; // Update remaining time display of the pulpit
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*If player touches this pulpit once,
         * set the property to true for the
         * rest of the alive duration of the pulpit.
         * This way, score updates only once
         for each pulpit touched by player. */

        if (collision.gameObject.CompareTag("Player")) 
            hasContactedPlayer = true;
    }

    private void OnDisable()
    {
        hasContactedPlayer = false;
        remainingLifetime = destroyTime;
    }
}
