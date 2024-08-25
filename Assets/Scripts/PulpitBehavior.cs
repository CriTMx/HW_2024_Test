using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PulpitBehavior : MonoBehaviour
{
    public float destroyTime;
    [SerializeField] private TextMeshProUGUI timerText;

    void Start()
    {
        /* Choose random delay between 
         * given inclusive range values,
         * to destroy the Pulpit after */

        timerText.text = destroyTime.ToString() + "s"; // Initialize text display of Pulpit timer to destroyTime seconds
    }

    void Update()
    {
        destroyTime -= Time.deltaTime;
        timerText.text = destroyTime.ToString() + "s";
    }

}
