using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


public class PulpitSpawnHandler : MonoBehaviour
{
    private float minPulpitDestroyTime, maxPulpitDestroyTime; // Values fetched from
    private float pulpitSpawnTime;                            // server json 

    private float destroyTime;          // Time pulpit will stay active for

    private int countPulpitsInScene;    // Tracks number of active pulpits
    private int pulpitSpawnPosChoice;   // Determines random absolute direction (forward, backward, left, right) to spawn new pulpit

    private bool firstPulpitSpawn;      // Enables initial spawn of first pulpit of the game at specified initial position
    private bool shouldSpawn;           // Determines if spawning is enabled

    [SerializeField] private PulpitBehavior pulpitPrefab;   // holds pulpit prefab object
    [SerializeField] private GameObject startBox;       // holds starting platform object


    public static Vector3 PulpitFixedSize = new(9f, 1f, 9f);
    [SerializeField] private Vector3 pulpitCurSize = new(9f, 1f, 9f);               // Size of current pulpit, initially 7x1x7
    [SerializeField] private Vector3 pulpitNextSize = new(9f, 1f, 9f);              // Size of next pulpit, initially 7x1x7, can add code to spawn random sized pulpits
    [SerializeField] private Vector3 pulpitInitialPos = new Vector3(0f, 1f, 0f);    // Initial position of first pulpit of the game

    [SerializeField] private int maxPulpitsInScene = 2; // Determines maximum pulpits in the game


    private Vector3 pulpitCurPos;   // Keeps track of position of current pulpit
    private Vector3 pulpitNextPos;  // Holds calculated value for position of next pulpit to spawn

    private GameObjectPool<PulpitBehavior> pulpitPool;

    [Inject]
    public void Construct(GameObjectPool<PulpitBehavior> _pulpitPool)
    {
        pulpitPool = _pulpitPool;
    }


    private void Awake()
    {
        // Add method to data fetch success event
        GameDataHandler.OnDataFetchSuccess += InitializePulpitData;
        PlayerDeath.OnPlayerDeath += DisableSpawning;
        PulpitAnimator.OnShrinkComplete += DestroyPulpit;
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


    private void InitializePulpitData()
    {
        // Initialize all fetched json data for the spawn conditions
        minPulpitDestroyTime = GameDataHandler.MinPulpitDestroyTime;
        maxPulpitDestroyTime = GameDataHandler.MaxPulpitDestroyTime;
        pulpitSpawnTime = GameDataHandler.PulpitSpawnTime;
    }

    void DeferredSpawnStartup()
    {
        // Destroy the starting platform and drop player onto the first pulpit
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

    private void SpawnPulpit()
    {
        GameObject pulpitInstance;

        /*pulpitNextSize =
            new Vector3(
                Random.Range(minPulpitSizeRange, maxPulpitsInScene),
                1f,
                Random.Range(minPulpitSizeRange, maxPulpitsInScene));*/

        if (firstPulpitSpawn)
        {
            // Create a new first pulpit at defined origin
            pulpitInstance = pulpitPool.Get(pulpitInitialPos).gameObject;

            // This if-block will no longer be accessed throughout the rest of the game until next restart
            firstPulpitSpawn = false;

            // Update current position with that of latest pulpit
            pulpitCurPos = pulpitInitialPos;
        }

        else
        {
            pulpitNextPos = GetRandomSpawnPosition();

            // Create new pulpit at random adjacent position
            pulpitInstance = pulpitPool.Get(pulpitNextPos).gameObject;

            // Update current pulpit size with that of the latest one
            pulpitCurSize = pulpitNextSize;

            // Update current position with that of latest pulpit
            pulpitCurPos = pulpitNextPos;
        }

        // Initialize destroyTime
        SetDestroyTime(pulpitInstance);

        // Set new pulpit size (if random size every new pulpit)
        pulpitInstance.transform.localScale = pulpitNextSize;
        pulpitCurSize = pulpitNextSize;

        // Update number of active pulpits
        countPulpitsInScene++;
    }

    void SetDestroyTime(GameObject _pulpitInstance)
    {
        destroyTime = Random.Range(minPulpitDestroyTime, maxPulpitDestroyTime);

        // Set instance's destroyTime to generated destroyTime for display purpose
        _pulpitInstance.GetComponent<PulpitBehavior>().destroyTime = destroyTime;
        _pulpitInstance.GetComponent<PulpitAnimator>().destroyTime = destroyTime;

        _pulpitInstance.GetComponent<PulpitAnimator>().InitializePulpit();
    }

    Vector3 GetRandomSpawnPosition()
    {
        /* Dynamically determine spawn position
         * of the next pulpit based on size 
         * of the latest pulpit and size of the
         * next pulpit to spawn */

        float pulpitNextPosZ = (pulpitCurSize.z / 2) + (pulpitNextSize.z / 2);
        float pulpitNextPosX = (pulpitCurSize.x / 2) + (pulpitNextSize.x / 2);

        Vector3[] spawnPosChoices = new Vector3[] 
        {
            pulpitCurPos + new Vector3(0f, 0f, pulpitNextPosZ),
            pulpitCurPos - new Vector3(0f, 0f, pulpitNextPosZ),
            pulpitCurPos - new Vector3(pulpitNextPosX, 0f, 0f),
            pulpitCurPos + new Vector3(pulpitNextPosX, 0f, 0f),

        };
        pulpitSpawnPosChoice = Random.Range(0, spawnPosChoices.Length);  // Choose between 1 of 4 possible adjacent sides at random

        return spawnPosChoices[pulpitSpawnPosChoice];
    }

    void DestroyPulpit(PulpitBehavior _pulpit)
    {
        /*StartCoroutine(DestroyerCoroutine(_pulpit, destroyTime));*/

        // Destroy pulpit once animation is complete
        pulpitPool.Return(_pulpit);

        // Update active pulpits count
        countPulpitsInScene--;
    }

    IEnumerator DestroyerCoroutine(GameObject pulpit, float destroyTime)
    {
        // Wait for destroyTime seconds before destroying pulpits
        yield return new WaitForSeconds(destroyTime);

    }


    /* Random sized pulpit spawning not being
     * used currently as it is taking a lot
     * of time to test the game difficulty
     based on different sized pulpits */

    

    void DisableSpawning()
    {
        /* Call this function when the player 
         * dies to avoid continuous spawning
         * of pulpits after death */

        shouldSpawn = false;
    }

    private void OnDestroy()
    {
        // Remove method from action event
        GameDataHandler.OnDataFetchSuccess -= InitializePulpitData;
        PlayerDeath.OnPlayerDeath -= DisableSpawning;
    }
}
