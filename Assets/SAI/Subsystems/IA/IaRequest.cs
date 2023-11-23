using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using Newtonsoft.Json;



public class IaRequest : MonoBehaviour
{
    public string rGetListIaProfile { get { return SAI.SDK.API.host + routeGetListProfileIa; } }
    public string rGetListMemoryIa { get { return SAI.SDK.API.host + routeGetMemoryTypeIa; } }
    public string rGetListPrivacyIa { get { return SAI.SDK.API.host + routeGetPrivacyIa; } }
    public string rGetListBaseModelIa { get { return SAI.SDK.API.host + routeGetBaseModelIa; } }

    public string routeGetListProfileIa = "/api/getprofileslist/";
    public string routeGetMemoryTypeIa = "/api/memorytype/";
    public string routeGetPrivacyIa = "/api/privacytype/";
    public string routeGetBaseModelIa = "/api/basemodel";
    public void GetIAProfile()
    {
        string baseUrl = "https://apiv2.sos.space/api/aiprofile";
        string deleteProfile = "aiprofile_id=" + "18"; //SharedDateIaProfile.profileID.ToString();


        string parameter = deleteProfile;
        string url = baseUrl + "?" + parameter;

        string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject("");

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
                Debug.LogError("Error Json: " + webRequest.downloadHandler.text);
            }
            else
            {
                string jsonResponse = webRequest.downloadHandler.text;
                Debug.Log("JSON Response:\n" + jsonResponse);

                LogicIA logicIA = FindObjectOfType<LogicIA>();
                logicIA.IAProfileResponse(jsonResponse);
            }
        }
    }
    public void DeleteIAProfile()
    {
        string baseUrl = "https://apiv2.sos.space/api/aiprofile";
        string deleteProfile = "aiprofile_id=" + SharedDateIaProfile.profileID.ToString();


        string parameter = deleteProfile;
        string url = baseUrl + "?" + parameter;

        Debug.Log("Que llega aqui: " + url);

        string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject("");

        using (UnityWebRequest webRequest = new UnityWebRequest(url, "DELETE"))
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
                Debug.LogError("Error Json: " + webRequest.downloadHandler.text);
            }
            else
            {
                string jsonResponse = webRequest.downloadHandler.text;
                Debug.Log("JSON Response:\n" + jsonResponse);
            }
        }
    }

    public void GetIAProfileList()
    {
        //string endpoint = SAI.subsystems.api.routeGetBaseModelIa;

        string baseUrl = "https://apiv2.sos.space/api/getprofileslist";
        string getList = "owner_aiprofile=True&shared_aiprofile=True&public_aiprofile=True"; // + SharedDateIaProfile.profileID.ToString();

        string url = baseUrl + "?" + getList;
        string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject("");

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
                Debug.LogError("Error Json: " + webRequest.downloadHandler.text);
            }
            else
            {
                string jsonResponse = webRequest.downloadHandler.text;
                Debug.Log("JSON Response:\n" + jsonResponse);

                IaPrefabs logicIA = FindObjectOfType<IaPrefabs>();
                logicIA.GetListIAProfile(jsonResponse);
            }
        }
    }


    public void GetBaseModel()
    {
        //string endpoint = SAI.subsystems.api.routeGetBaseModelIa;

        string url = "https://apiv2.sos.space/api/basemodel";

        string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject("");

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
                Debug.LogError("Error Json: " + webRequest.downloadHandler.text);
            }
            else
            {
                string jsonResponse = webRequest.downloadHandler.text;
                Debug.Log("JSON Response:\n" + jsonResponse);

                LogicIA logicIA = FindObjectOfType<LogicIA>();
                logicIA.BaseModelResponse(jsonResponse);
            }
        }
    }


    public void GetMemoryType()
    {
        string url = "https://apiv2.sos.space/api/memorytype";

        string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject("");

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
                Debug.LogError("Error Json: " + webRequest.downloadHandler.text);
            }
            else
            {
                string jsonResponse = webRequest.downloadHandler.text;
                Debug.Log("JSON Response:\n" + jsonResponse);

                LogicIA logicIA = FindObjectOfType<LogicIA>();
                logicIA.MemoryTypeResponse(jsonResponse);

            }
        }
    }

    public void GetPrivacyType()
    {
        string url = "https://apiv2.sos.space/api/privacytype";

        string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject("");

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
                Debug.LogError("Error Json: " + webRequest.downloadHandler.text);
            }
            else
            {
                string jsonResponse = webRequest.downloadHandler.text;
                Debug.Log("JSON Response:\n" + jsonResponse);

                LogicIA logicIA = FindObjectOfType<LogicIA>();
                logicIA.PrivacyTypeResponse(jsonResponse);
            }
        }
    }

    public void AIProfile(object infoIA, string method)
    {
        string url = "https://apiv2.sos.space/api/aiprofile";

        string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(infoIA);

        Debug.Log("Test Json Put: " + jsonData);

        using (UnityWebRequest webRequest = new UnityWebRequest(url, method))
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
                Debug.LogError("Error Json: " + webRequest.downloadHandler.text);
            }
            else
            {
                string jsonResponse = webRequest.downloadHandler.text;
                Debug.Log("JSON Response:\n" + jsonResponse);
            }
        }
    }

   
}
