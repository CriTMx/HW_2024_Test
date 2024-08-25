using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private float animationTime = 0.1f;
    [SerializeField] private float shrinkRate = 0.3f;
    
    void Start()
    {
        
    }

    void Update()
    {
        if (gameObject.transform.position.y <= 0)
            StartCoroutine(PlayerDeathAnimationCoroutine());
    }

    IEnumerator PlayerDeathAnimationCoroutine()
    {
        while (gameObject.transform.localScale.x > 0f)
        {
            yield return new WaitForSeconds(animationTime);
            gameObject.transform.localScale -= new Vector3(shrinkRate, shrinkRate, shrinkRate);
        }
        Destroy(gameObject);
    }
}
