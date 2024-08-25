using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking; // To handle web requests

public class FetchGameData : MonoBehaviour
{
    private string dataObject;

    [SerializeField]
    private string serverURL = "https://s3.ap-south-1.amazonaws.com/superstars.assetbundles.testbuild/doofus_game/doofus_diary.json";

    public static float PlayerSpeed;
    public static float MinPulpitDestroyTime, MaxPulpitDestroyTime, PulpitSpawnTime;

    void Start()
    {
        StartCoroutine(GetGameDataFromServer());
    }

    
    void Update()
    {
        
    }

    IEnumerator GetGameDataFromServer()
    {
        using UnityWebRequest server = UnityWebRequest.Get(serverURL); // Establish connection to the server
        yield return server.SendWebRequest(); 

        if (server.result == UnityWebRequest.Result.ConnectionError || server.result == UnityWebRequest.Result.ProtocolError)
            Debug.Log(server.error); // Log error if server responds with connection error or protocol error

        else
        {
            dataObject = server.downloadHandler.text; // Fetch JSON data from server as a string
            GameData gameData = JsonUtility.FromJson<GameData>(dataObject);

            // Set game data values to store until Game exits
            PlayerSpeed = gameData.player_data.speed;
            MinPulpitDestroyTime = gameData.pulpit_data.min_pulpit_destroy_time;
            MaxPulpitDestroyTime = gameData.pulpit_data.max_pulpit_destroy_time;
            PulpitSpawnTime = gameData.pulpit_data.pulpit_spawn_time;
        }
    }
}

[System.Serializable]
public class GameData
{
    public PlayerData player_data;
    public PulpitData pulpit_data;
}

[System.Serializable]
public class PlayerData
{
    public float speed;
}

[System.Serializable]
public class PulpitData
{
    public float min_pulpit_destroy_time;
    public float max_pulpit_destroy_time;
    public float pulpit_spawn_time;
}
