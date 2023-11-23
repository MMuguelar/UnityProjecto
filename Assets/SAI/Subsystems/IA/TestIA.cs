using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TestIA : MonoBehaviour
{
    public void getListIa()
    {
        string endpoint = SAI.SDK.iarequest.rGetListIaProfile;

        var requestIA = new
        {
            mac_node = "",
        };

        string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(requestIA);

        using (UnityWebRequest webRequest = new UnityWebRequest(endpoint, "GET"))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("spaceaisess", SAI.SDK.Login.SessionKey);

            byte[] requestDate = System.Text.Encoding.UTF8.GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(requestDate);
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            webRequest.SendWebRequest();

            while (!webRequest.isDone)
            {
                // Wait for the request to complete
            }

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
               webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                string jsonResponse = webRequest.downloadHandler.text;
                Debug.Log("JSON Response:\n" + jsonResponse);
            }
        }
    }

    





    [System.Serializable]
    public class profileIaData
    {
        public string name;
        public config config;
        public string basemodel_code;
        public string memorytype_code;
        public string privacy;
        public string prompt;
        public string ai_description;
        public string intro_message;
        public float temperature;
    }

    [System.Serializable]
    public class config
    {
        public string algo;
    }
}
