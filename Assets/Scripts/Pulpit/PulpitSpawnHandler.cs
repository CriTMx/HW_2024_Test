using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulpitSpawnHandler : MonoBehaviour
{
    private float minPulpitDestroyTime, maxPulpitDestroyTime; /* Values fetched from */
    private float pulpitSpawnTime;                            /* server json */

    private float destroyTime;          // Time pulpit will stay active for

    private int countPulpitsInScene;    // Tracks number of active pulpits
    private int pulpitSpawnDirection;   // Determines random absolute direction (forward, backward, left, right) to spawn new pulpit

    private bool firstPulpitSpawn;      // Enables initial spawn of first pulpit of the game at specified initial position
    private bool shouldSpawn;           // Determines if spawning is enabled

    [SerializeField] private GameObject pulpitPrefab;   // holds pulpit prefab object
    [SerializeField] private GameObject startBox;       // holds starting platform object

    [SerializeField] private Vector3 pulpitCurSize = new(7f, 1f, 7f);   // Size of current pulpit, initially 7x1x7
    [SerializeField] private Vector3 pulpitNextSize = new(7f, 1f, 7f);  // Size of next pulpit, initially 7x1x7, can add code to spawn random sized pulpits
    [SerializeField] private Vector3 pulpitInitialPos = new Vector3(0f, 1f, 0f);   // Initial position of first pulpit of the game

    [SerializeField] private float minPulpitSizeRange = 4f;     // Minimum size for random-sized pulpit generation
    [SerializeField] private float maxPulpitSizeRange = 8f;     // Maximum size for random-sized pulpit generation

    [SerializeField] private float animationTime = 0.2f;

    [SerializeField] private int maxPulpitsInScene = 2; // Determines maximum pulpits in the game


    private Vector3 pulpitCurPos;   // Keeps track of position of current pulpit
    private Vector3 pulpitNextPos;  // Holds calculated value for position of next pulpit to spawn


    private void Awake()
    {
        // Add method to data fetch success event
        GameDataHandler.OnDataFetchSuccess += InitializePulpitData;
        PlayerDeath.OnPlayerDeath += DisableSpawning;
    }

    void Start()
    {
        // Initialize count and first spawn
        countPulpitsInScene = 0;
        shouldSpawn = true;
        firstPulpitSpawn = true;

        // Stop currently running coroutines just to be safe
        StopAllCoroutines();

        // Delayed spawn startup, lets the object catch up with json fetching
        Invoke("DeferredSpawnStartup", 1f);
    }

    void DeferredSpawnStartup()
    {
        Destroy(startBox);
        StartCoroutine(SpawnerCoroutine());
    }

    public IEnumerator SpawnerCoroutine()
    {
        while (shouldSpawn)
        {
            // Spawn new pulpit within spawn duration if spawning more is still allowed
            if (countPulpitsInScene < maxPulpitsInScene)
                SpawnPulpit();
            yield return new WaitForSeconds(pulpitSpawnTime);
        }
    }

    IEnumerator DestroyerCoroutine(GameObject pulpit, float destroyTime)
    {
        // Wait for destroyTime seconds before destroying pulpits
        yield return new WaitForSeconds(destroyTime);

        /* Animation disabled due to 
         * it breaking the pulpit spawning*/


        /* 
        // Run shrinking animation before destroying the pulpit
        Vector3 curSize = pulpit.transform.localScale;
        Vector3 deathSize = Vector3.zero;
        float curTime = 0f;

        while (curTime <= animationTime)
        {
            // Linearly interpolate pulpit size to zero gradually over the animation time
            pulpit.transform.localScale = Vector3.Lerp(curSize, deathSize, curTime / animationTime);
            curTime += Time.deltaTime;
            yield return null;
        }
        */

        // Destroy pulpit once animation is complete
        Destroy(pulpit);

        // Update active pulpits count
        countPulpitsInScene--;
    }

    private void SpawnPulpit()
    {
        GameObject pulpitInstance;
        if (firstPulpitSpawn)
        {
            // Create a new first pulpit at defined origin
            pulpitInstance = 
                Instantiate(pulpitPrefab, pulpitInitialPos, pulpitPrefab.transform.rotation);

            // This if-block will no longer be accessed throughout the rest of the game
            firstPulpitSpawn = false;

            // Update current position with that of latest pulpit
            pulpitCurPos = pulpitInitialPos; 
        }

        else
        {
            pulpitSpawnDirection = Random.Range(0, 4);  // Choose between 1 of 4 possible adjacent sides at random

            pulpitNextPos = pulpitSpawnDirection switch // Decides which adjacent side to spawn the next platform at
            {
                0 => pulpitCurPos + new Vector3(0f, 0f, (pulpitCurSize.z / 2) + (pulpitNextSize.z / 2)), // Next pulpit spawns in front of current
                1 => pulpitCurPos - new Vector3(0f, 0f, (pulpitCurSize.z / 2) + (pulpitNextSize.z / 2)), // Next platform spawns behind current
                2 => pulpitCurPos - new Vector3((pulpitCurSize.x / 2) + (pulpitNextSize.x / 2), 0f, 0f), // Next platform spawns to the left
                3 => pulpitCurPos + new Vector3((pulpitCurSize.x / 2) + (pulpitNextSize.x / 2), 0f, 0f), // Next platform spawns to the right
                _ => pulpitCurPos + new Vector3(0f, 0f, (pulpitCurSize.z / 2) + (pulpitNextSize.z / 2))  // Default case (front spawn)
            };

            // Create new pulpit at random adjacent position
            pulpitInstance =
                Instantiate(pulpitPrefab, pulpitNextPos, pulpitPrefab.transform.rotation);

            // Update current position with that of latest pulpit
            pulpitCurPos = pulpitNextPos;
        }

        // Set new pulpit size (if random size every new pulpit)
        pulpitInstance.transform.localScale = pulpitNextSize;
        // Update number of active pulpits
        countPulpitsInScene++;

        // Initialize destroyTime and start Destroy coroutine
        destroyTime = Random.Range(minPulpitDestroyTime, maxPulpitDestroyTime);
        // Set instance's destroyTime to generated destroyTime for display purpose
        pulpitInstance.GetComponent<PulpitBehavior>().destroyTime = destroyTime;

        StartCoroutine(DestroyerCoroutine(pulpitInstance, destroyTime));
    }

    void DisableSpawning()
    {
        shouldSpawn = false;
    }

    private void OnDisable()
    {
        /* Stop all the coroutines and 
         * reset the object when the
         * scene changes or reloads, 
         * not doing this results in 
         * some unexpected buggy behavior */

        StopAllCoroutines();
    }

    private void OnDestroy()
    {
        // Remove method from action event
        GameDataHandler.OnDataFetchSuccess -= InitializePulpitData;
        PlayerDeath.OnPlayerDeath -= DisableSpawning;
    }

    private void InitializePulpitData()
    {
        // Initialize all fetched json data for the spawn conditions
        minPulpitDestroyTime = GameDataHandler.MinPulpitDestroyTime;
        maxPulpitDestroyTime = GameDataHandler.MaxPulpitDestroyTime;
        pulpitSpawnTime = GameDataHandler.PulpitSpawnTime;
    }
}
