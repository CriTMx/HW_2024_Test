using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PulpitAnimator : MonoBehaviour
{
    public static event Action<PulpitBehavior> OnShrinkComplete;

    [SerializeField] private float animationDuration = 0.5f;

    public float destroyTime;
    private float remainingTime;
    private Vector3 initialScale = PulpitSpawnHandler.PulpitFixedSize;
    private Vector3 targetScale = Vector3.zero;

    private Vector3 sizeDown;

    private float elapsedTime;
    private float shrinkProgress;

    public bool isAnimationRunning;

    private void OnDisable()
    {
        transform.localScale = initialScale;
        remainingTime = destroyTime;
        elapsedTime = 0f;
        shrinkProgress = 0f;
        /*StopCoroutine(Shrink());*/
    }

    void Start()
    {

    }

    public void InitializePulpit()
    {
        transform.localScale = initialScale;
        remainingTime = destroyTime;
        elapsedTime = 0f;
        shrinkProgress = 0f;

        Invoke(nameof(StartShrinkAnimation), destroyTime - animationDuration);
    }

    void StartShrinkAnimation()
    {
        StartCoroutine(Shrink());
    }

    IEnumerator Shrink()
    {
        elapsedTime = 0;
        
        while (elapsedTime <= animationDuration)
        {
            elapsedTime += Time.deltaTime;
            shrinkProgress = elapsedTime / animationDuration;
            transform.localScale = Vector3.Lerp(initialScale, targetScale, shrinkProgress);
            yield return null;
        }
        transform.localScale = targetScale;
        OnShrinkComplete?.Invoke(this.gameObject.GetComponent<PulpitBehavior>());

    }

    
}
