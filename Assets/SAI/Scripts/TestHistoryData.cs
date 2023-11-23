using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


[System.Serializable]
public class NodeInfoRequest
{
    public string mac_node;
}

//[System.Serializable]
//public class TelemetryData
//{
//    public string date;
//    public TelemetryDetail detail;
//}

//[System.Serializable]
//public class TelemetryDetail
//{
//    public string name;
//    public int grav_x;
//    public int grav_y;
//    public int grav_z;
//    public string userID;
//    public int altitude;
//    public int latitude;
//    public int longitude;
//    public int board_temp;
//    public int lin_accel_x;
//    public int lin_accel_y;
//    public int lin_accel_z;
//}

//[System.Serializable]
//public class TelemetryDataWrapper
//{
//    public List<TelemetryData> telemetry_data;
//}

public class TestHistoryData : MonoBehaviour
{
    public string host = "http://127.0.0.1:8000";
    public string getNodeInfo = "/api/getnodeinfo/";
    //public string savePath = "Assets/JSON/TestJsonHistory.json"; // Ruta donde se guardará el archivo JSON

    //public TextMeshProUGUI telemetryText; // Referencia al objeto de texto en Unity

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(test());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator test()
    {
        string macNodeValue = "a0a382c63c94";

        string urlWithQueryString = host + getNodeInfo;


        string jsonData = JsonConvert.SerializeObject(macNodeValue, Formatting.Indented);


        UnityWebRequest request = UnityWebRequest.Get(urlWithQueryString);

        byte[] array = null;
        array = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(array);
        request.uploadHandler.contentType = "application/json";
        request.SetRequestHeader("spaceaisess", "7tgiqqv8b9t6t25xfookq06i8cvkezld");
        //request.SetRequestHeader("csrfToken", SAI.subsystems.api.csrfToken);
        //request.SetRequestHeader("X-CSRFToken", SAI.subsystems.api.csrfToken);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Debug.Log("Request " + url + " " + jsonData + " Result OK: " + request.downloadHandler.text);
            // Parse the JSON response (if applicable)
            // You may need to deserialize it using JsonUtility or another library
            // For example: YourResponseClass response = JsonUtility.FromJson<YourResponseClass>(request.downloadHandler.text);
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("Response: " + jsonResponse);
        }
        else
        {
            //  Debug.Log("Request " + url + " " + jsonData + " Result Error: " + request.error + " " + request.downloadHandler.text);
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("fail" + jsonResponse);
        }
    }
}