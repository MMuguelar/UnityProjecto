using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using System.Globalization;
using SimpleJSON;
using UnityEngine.Networking;

public class DateSearchNodeontrol : MonoBehaviour
{

    public TMPro.TMP_InputField from;
    public TMPro.TMP_InputField to;

    private string isoFrom;
    private string isoTo;
    
    public void validateDate()
    {
        string fromText = from.text;
        string toText = to.text;

        DateTime fromDate;
        DateTime toDate;

        if (DateTime.TryParseExact(from.text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out fromDate)
        && DateTime.TryParseExact(to.text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out toDate))
        {
            if (fromDate > toDate)
            {
                Debug.Log("ToDate cannot be greater than FromDate");
                from.text = "";
                to.text = "";
            }
            else
            {
                Debug.Log("Valid date");
                isoFrom = fromDate.ToString("yyyy-MM-ddT00:00:01Z");
                isoTo = toDate.ToString("yyyy-MM-ddT23:59:59Z");
                from.text = "";
                to.text = "";
                requestDate();
            }
        }
        else
        {
            Debug.Log("Invalid Date");
            from.text = ""; 
            to.text = "";
        }

    }

    public void requestDate()
    {
        string mac = SharedData.Mac;
        Debug.Log("Test: " + mac);
        Debug.Log("From: " + isoFrom + "To: " + isoTo);

        string url = SAI.SDK.API.host + SAI.SDK.Nodes.GetNodeInfo;

        var data = new
        {
            mac_node = mac,
            from_ = isoFrom,
            to = isoTo,
            limit = 2,
            offset = 0
        };

        // Serializar el objeto JSON
        string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(data);

        using (UnityWebRequest webRequest = new UnityWebRequest(url, "GET"))
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



}
