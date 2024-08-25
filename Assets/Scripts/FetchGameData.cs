using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking; // To handle web requests

public class FetchGameData : MonoBehaviour
{
    private string dataObject;

    [SerializeField]
    private string serverURL = "https://s3.ap-south-1.amazonaws.com/superstars.assetbundles.testbuild/doofus_game/doofus_diary.json";

    void Start()
    {
        StartCoroutine(GetGameDataFromServer());
    }

    
    void Update()
    {
        
    }

    IEnumerator GetGameDataFromServer()
    {
        using (UnityWebRequest server = UnityWebRequest.Get(serverURL))
        {
            yield return server.Send();

            if (server.isNetworkError || server.isHttpError)
                Debug.Log(server.error);
            else
            {
                dataObject = server.downloadHandler.text;
                Debug.Log(dataObject);
            }
        }
    }
}
