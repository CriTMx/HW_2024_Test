using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulpitSpawnHandler : MonoBehaviour
{
    private float minPulpitDestroyTime, maxPulpitDestroyTime; /* Values fetched from */
    private float pulpitSpawnTime;                            /* server json */

    private float destroyTime;

    private int countPulpitsInScene;
    private int pulpitSpawnDirection;

    private bool firstPulpitSpawn;

    [SerializeField] private GameObject pulpitPrefab;

    [SerializeField] private Vector3 pulpitCurSize = new(9f, 1f, 9f);
    [SerializeField] private Vector3 pulpitNextSize = new(9f, 1f, 9f);
    [SerializeField] private Vector3 pulpitInitialPos = Vector3.zero;

    private Vector3 pulpitCurPos;
    private Vector3 pulpitNextPos;

    [SerializeField] private int maxPulpitsInScene = 2;

    private void Awake()
    {
        GameDataHandler.OnDataFetchSuccess += InitializePulpitData;
    }

    void Start()
    {
        countPulpitsInScene = 0;
        firstPulpitSpawn = true;

        StartCoroutine(SpawnerCoroutine());
    }

    void Update()
    {

    }

    IEnumerator SpawnerCoroutine()
    {
        while (true)
        {
            if (countPulpitsInScene < maxPulpitsInScene)
                SpawnPulpit();
            yield return new WaitForSeconds(pulpitSpawnTime);
        }
    }

    IEnumerator DestroyerCoroutine(GameObject pulpit, float destroyTime)
    {
        yield return new WaitForSeconds(destroyTime);

        Destroy(pulpit);
        countPulpitsInScene--;
    }

    private void SpawnPulpit()
    {
        GameObject pulpitInstance;
        if (firstPulpitSpawn)
        {
            // Create a new pulpit at defined origin
            pulpitInstance = 
                Instantiate(pulpitPrefab, pulpitInitialPos, pulpitPrefab.transform.rotation);

            firstPulpitSpawn = false;
            pulpitCurPos = pulpitInitialPos;
        }

        else
        {
            pulpitSpawnDirection = Random.Range(0, 4);

            pulpitNextPos = pulpitSpawnDirection switch // Decides which adjacent side to spawn the next platform at
            {
                0 => pulpitCurPos + new Vector3(0f, 0f, (pulpitCurSize.z/2)+(pulpitNextSize.z/2)), // Next pulpit spawns in front of current
                1 => pulpitCurPos - new Vector3(0f, 0f, (pulpitCurSize.z/2)+(pulpitNextSize.z)/2), // Next platform spawns behind current
                2 => pulpitCurPos - new Vector3((pulpitCurSize.x/2)+(pulpitNextSize.x/2), 0f, 0f), // Next platform spawns to the left
                3 => pulpitCurPos + new Vector3((pulpitCurSize.x/2)+(pulpitNextSize.x/2), 0f, 0f), // Next platform spawns to the right
            };

            // Create new pulpit at random adjacent position
            pulpitInstance =
                Instantiate(pulpitPrefab, pulpitNextPos, pulpitPrefab.transform.rotation);

            pulpitCurPos = pulpitNextPos;
        }

        // Set new pulpit size
        pulpitInstance.transform.localScale = pulpitNextSize;
        countPulpitsInScene++;

        destroyTime = Random.Range(minPulpitDestroyTime, maxPulpitDestroyTime);
        StartCoroutine(DestroyerCoroutine(pulpitInstance, destroyTime));
    }

    private void OnDestroy()
    {
        GameDataHandler.OnDataFetchSuccess -= InitializePulpitData;
    }

    private void InitializePulpitData()
    {
        minPulpitDestroyTime = GameDataHandler.MinPulpitDestroyTime;
        maxPulpitDestroyTime = GameDataHandler.MaxPulpitDestroyTime;
        pulpitSpawnTime = GameDataHandler.PulpitSpawnTime;
    }
}
