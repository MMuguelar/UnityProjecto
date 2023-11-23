using Newtonsoft.Json; // Libreria requerida para Serializar
 // Libreria requerida para Debug
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Transactions;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using static NodeControl;
using System.Runtime.InteropServices.WindowsRuntime;

/// <summary>
/// API Interaction
/// </summary>

public class API : MonoBehaviour
{


    




    // Host
    public string host = "https://apiv2.sos.space";

    // Routes (se migrarian Todas)
    
    //public string routeSignIn = "/api/login/";
    //public string routeSignUp = "/api/singup/";
    //public string routeLogout = "/api/logout/";
    //public string routeReset = "/api/reset/";
    public string routeDevice = "/api/device/"; // Migrada a Devices
    public string routeBookNode = "/api/booknode/";
    public string routeGetNodeList = "/api/getnodelist/";
    public string routeGetNodeInfo = "/api/getnodeinfo/"; // Migrada a Nodes
    public string routeGetWallet = "/api/getwallet/"; // Migrada a Wallet
    public string routeNodeCreation = "/api/nodecreation/"; // Migrada a Nodes
    public string routeGetClusterList = "/api/getuserclusters/";
    public string routeGetNodeListCluster = "/api/getnodelistcluster";
    public string routeGetClusterTransactionList = "getclustertransactionslist";   
   // public string routeValidateNodeExists = "validateNodeExists"; // migrada a devices
    public string routeNodeSensor = "/api/nodesensor";
    public string routeAddNodeSensor = "addNodeSensor";
    public string routeNodeType = "nodetype";
    
    public string routeAddCluster = "/api/cluster";



    
    public delegate void ApiCallBack(UnityWebRequest responseData);

    //public string rLogin { get { return host + routeSignIn; }  }
    //public string rLogout { get { return host + routeLogout; } }
    //public string rRegister { get { return host + routeSignUp; } }
    public string rDevice { get { return host + routeDevice; } }
    public string rBookNode { get { return host + routeBookNode; } }
    public string rGetNodeList { get { return host + routeGetNodeList; } }
    public string rGetNodeInfo { get { return host + routeGetNodeInfo; } }
    public string rGetWallet { get { return host + routeGetWallet; } }
    public string rNodeCreation { get { return host + routeNodeCreation; } }
    public string rGetClusterList { get { return host + routeGetClusterList; } }
    public string rGetNodeListCluster { get { return host + routeGetNodeListCluster; } }
    public string rNodeSensor { get { return host + routeNodeSensor; } }
    public string rAddNodeSensor { get { return host + routeAddNodeSensor; } }

    public string rGetClusterTransactionlist { get { return host + routeGetClusterTransactionList; } }
    //public string rValidateNodeExists { get { return host + routeValidateNodeExists; } }
    public string rAddCluster { get { return host + routeAddCluster; } }
 
    //public string rGetClusterTransactionlist { get { return host + routeGetClusterTransactionList; } }
    //public string rValidateNodeExists { get { return host + routeValidateNodeExists; } }
    public string rNodeType {  get { return host + routeNodeType; } }

    

    // User Tokens
    //public string SessionKey = string.Empty;
    //public string csrfToken = string.Empty;




 
    #region GenericApiCalls
    public string GenericGetRequest(string url)
    {
        Debug.Log($"Generic Get Request to {url}");






        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            // Agrega la clave como un encabezado personalizado
            www.SetRequestHeader("Content-Type", "application/json");
            if(SAI.SDK.Login.SessionKey != string.Empty) www.SetRequestHeader("spaceaisess", SAI.SDK.Login.SessionKey);
            string security = "{}";

            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(security);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();

            string jsonRequestBody = JsonConvert.SerializeObject(security);

            www.SendWebRequest();

            while (!www.isDone)
            {
                // Espera a que la solicitud se complete
            }

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                string result = www.downloadHandler.text;
                Debug.LogError("API.CS Error: " + www.error + result);
                SAI.SDK.errorHandler.ShowPopupJson(result);
                return "";
            }
            else
            {

                string jsonResponse = www.downloadHandler.text;
                Debug.Log("API.CS Response: " + jsonResponse);

                return jsonResponse;
            }
        }
    }

    public string GenericPostRequest(string url, string jsonData)
    {
        print($"Generic Post Request : URL={url} JSONDATA={jsonData}");

        //SAI.subsystems.logger.LogToFile($"Corutina iniciada", "Log.txt");
        UnityWebRequest request = UnityWebRequest.PostWwwForm(url, jsonData);

        byte[] array = null;
        array = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(array);
        request.uploadHandler.contentType = "application/json";
        if(SAI.SDK.Login.SessionKey != String.Empty)
        {
            print("Adding Header spaceaisess"+ SAI.SDK.Login.SessionKey);
            request.SetRequestHeader("spaceaisess", SAI.SDK.Login.SessionKey);
        }
        
        //SAI.subsystems.logger.LogToFile($"Agregando Header Seesion Key : {SAI.subsystems.api.sessionKey}", "Log.txt");

        if (SAI.SDK.Login.csrfToken != String.Empty)
        {
            request.SetRequestHeader("csrfToken", SAI.SDK.Login.csrfToken);
            request.SetRequestHeader("X-CSRFToken", SAI.SDK.Login.csrfToken);
            //SAI.subsystems.logger.LogToFile($"Agregando Header csrfToken : {SAI.subsystems.api.csrfToken}", "Log.txt");
        }


        request.SendWebRequest();
        print($"Request Enviada a {url} DATA:{jsonData}");
        while (!request.isDone)
        {
            //  wait
        }

        //SAI.subsystems.logger.LogToFile($"Request Enviada a {url}", "Log.txt");

        if (request.result != UnityWebRequest.Result.Success)
        {

            string responseText = request.downloadHandler.text;
            SAI.SDK.errorHandler.ShowPopupJson(responseText);

            SAI.SDK.logger.LogToFile($"Request Response BAD : {responseText}", "Log.txt");


            Debug.LogError("Error HTTP: " + request.responseCode + " " + responseText); // Codigo de respuesta HTTP
            
            
            if (!string.IsNullOrEmpty(request.error))
            {
                Debug.LogError("Mensaje de error: " + request.error); // Mensaje de error proporcionado por Unity
            }

            if (!string.IsNullOrEmpty(responseText))
            {
                Debug.LogError("Contenido del error: " + responseText); // Mensaje de error proporcionado por la API
            }

            print("Request Failed, sending to callback" + responseText);
            return "";
        }
        else
        {
            string responseText = request.downloadHandler.text;
            SAI.SDK.logger.LogToFile($"Request Response OK : {responseText}", "Log.txt");
            print("Request Success, sending to callback" + responseText);
            return responseText;
        }
    }

    public string GenericPutRequest(string url, string jsonData)
    {
        print($"Generic Post Request : URL={url} JSONDATA={jsonData}");

        //SAI.subsystems.logger.LogToFile($"Corutina iniciada", "Log.txt");
        UnityWebRequest request = UnityWebRequest.Put(url, jsonData);

        byte[] array = null;
        array = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(array);
        request.uploadHandler.contentType = "application/json";
        if (SAI.SDK.Login.SessionKey != String.Empty)
        {
            print("Adding Header spaceaisess" + SAI.SDK.Login.SessionKey);
            request.SetRequestHeader("spaceaisess", SAI.SDK.Login.SessionKey);
        }

        //SAI.subsystems.logger.LogToFile($"Agregando Header Seesion Key : {SAI.subsystems.api.sessionKey}", "Log.txt");

        if (SAI.SDK.Login.csrfToken != String.Empty)
        {
            request.SetRequestHeader("csrfToken", SAI.SDK.Login.csrfToken);
            request.SetRequestHeader("X-CSRFToken", SAI.SDK.Login.csrfToken);
            //SAI.subsystems.logger.LogToFile($"Agregando Header csrfToken : {SAI.subsystems.api.csrfToken}", "Log.txt");
        }


        request.SendWebRequest();
        print($"Request Enviada a {url} DATA:{jsonData}");
        while (!request.isDone)
        {
            //  wait
        }

        //SAI.subsystems.logger.LogToFile($"Request Enviada a {url}", "Log.txt");

        if (request.result != UnityWebRequest.Result.Success)
        {

            string responseText = request.downloadHandler.text;
            SAI.SDK.errorHandler.ShowPopupJson(responseText);

            SAI.SDK.logger.LogToFile($"Request Response BAD : {responseText}", "Log.txt");


            Debug.LogError("Error HTTP: " + request.responseCode + " " + responseText); // Codigo de respuesta HTTP


            if (!string.IsNullOrEmpty(request.error))
            {
                Debug.LogError("Mensaje de error: " + request.error); // Mensaje de error proporcionado por Unity
            }

            if (!string.IsNullOrEmpty(responseText))
            {
                Debug.LogError("Contenido del error: " + responseText); // Mensaje de error proporcionado por la API
            }

            print("Request Failed, sending to callback" + responseText);
            return "";
        }
        else
        {
            string responseText = request.downloadHandler.text;
            SAI.SDK.logger.LogToFile($"Request Response OK : {responseText}", "Log.txt");
            print("Request Success, sending to callback" + responseText);
            return responseText;
        }
    }
    #endregion









    /*

    
    public void SendFormPostRequest(string url,WWWForm form,ApiCallBack callback)
    {

        print($"Posting to {form.ToString()} to {url} and calling {callback} for results");

        StartCoroutine(co_SendFormPostRequest(url, form, callback));
    }
    private IEnumerator co_SendFormPostRequest(string url, WWWForm form,ApiCallBack callBack)
    {
        WWWForm formWWW = form;
        UnityWebRequest request = UnityWebRequest.Post(url, form);
        request.SetRequestHeader("spaceaisess", SAI.SDK.API.sessionKey);
        request.SetRequestHeader("csrfToken", SAI.SDK.API.csrfToken);
        request.SetRequestHeader("X-CSRFToken", SAI.SDK.API.csrfToken);
        yield return request.SendWebRequest();
        if(request.result != UnityWebRequest.Result.Success)
        {
            

            Debug.LogError("Error HTTP: " + request.responseCode); // Codigo de respuesta HTTP

            if (!string.IsNullOrEmpty(request.error))
            {
                Debug.LogError("Mensaje de error: " + request.error); // Mensaje de error proporcionado por Unity
            }

            if (!string.IsNullOrEmpty(request.downloadHandler.text))
            {
                Debug.LogError("Contenido del error: " + request.downloadHandler.text); // Mensaje de error proporcionado por la API
            }
                        
            callBack(request);
        }
        else
        {
            print("Request success, sending to callback");
            callBack(request);
        }

    }

    
    public void SendJsonPostRequest(string url,string data,ApiCallBack callback)
    {
        //SAI.subsystems.logger.LogToFile($"Iniciando Corutina SendJsonPostRequest", "Log.txt");
        StartCoroutine(SendPostRequest(url, data,callback));
    }

    // Main CoRoutine
    private IEnumerator SendPostRequest(string url, string jsonData, ApiCallBack callBack)
    {

        //SAI.subsystems.logger.LogToFile($"Corutina iniciada", "Log.txt");
        UnityWebRequest request = UnityWebRequest.Post(url, jsonData);

        byte[] array = null;
        array = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(array);
        request.uploadHandler.contentType = "application/json";
        request.SetRequestHeader("spaceaisess", SAI.SDK.API.sessionKey);
        //SAI.subsystems.logger.LogToFile($"Agregando Header Seesion Key : {SAI.subsystems.api.sessionKey}", "Log.txt");

        if (SAI.SDK.API.csrfToken != String.Empty)
        {
            request.SetRequestHeader("csrfToken", SAI.SDK.API.csrfToken);
            request.SetRequestHeader("X-CSRFToken", SAI.SDK.API.csrfToken);
            //SAI.subsystems.logger.LogToFile($"Agregando Header csrfToken : {SAI.subsystems.api.csrfToken}", "Log.txt");
        }
      

        yield return request.SendWebRequest();

        //SAI.subsystems.logger.LogToFile($"Request Enviada a {url}", "Log.txt");
        print($"Request Enviada a {url} DATA:{jsonData}");
        if (request.result != UnityWebRequest.Result.Success)
        {

            string responseText = request.downloadHandler.text;

            SAI.SDK.logger.LogToFile($"Request Response BAD : {responseText}", "Log.txt");


            Debug.LogError("Error HTTP: " + request.responseCode + responseText); // Codigo de respuesta HTTP
            SAI.SDK.errorHandler.ShowPopupJson(responseText);

            if (!string.IsNullOrEmpty(request.error))
            {
                Debug.LogError("Mensaje de error: " + request.error); // Mensaje de error proporcionado por Unity
            }

            if (!string.IsNullOrEmpty(responseText))
            {
                Debug.LogError("Contenido del error: " + responseText); // Mensaje de error proporcionado por la API
            }

            print("Request Failed, sending to callback" + responseText);
            callBack(request);
        }
        else
        {
            string responseText = request.downloadHandler.text;
            SAI.SDK.logger.LogToFile($"Request Response OK : {responseText}", "Log.txt");
            print("Request Success, sending to callback");
            callBack(request);
        }
    }

    
    public void SendGetRequest(string url,ApiCallBack callback)
    {
        
        StartCoroutine(co_SendGetRequest(url,callback));
    }

    private IEnumerator co_SendGetRequest(string url,ApiCallBack callback)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("spaceaisess", SAI.SDK.API.sessionKey);

        print($"OLD FUNCTION * Sending Request to {url}  with following header : spaceaisess = {SAI.SDK.API.sessionKey}");
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            print("OLD * Request Failed" + request.error + request.downloadHandler.text);
            callback(request);
        
        }
        else
        {

            print($"OLD * Request OK : {request.downloadHandler.text}");
            callback(request);
            
        }
    }

    */

    #region MostWanted
    /*
    public void Logout()
    {
        //Deslogeamos

        string url = SAI.SDK.API.rLogout;
        
        SAI.SDK.API.SendJsonPostRequest(url, "{}", logoutCallback);
        sessionKey = "";
        csrfToken = "";
    }
    */
    /*
    public void logoutCallback(UnityWebRequest response)
    {
        if (response.result != UnityWebRequest.Result.Success)
        {
            print("Logout Error");
        }
        else
        {
            print("Logout Success");
        }
    }
    */
    #endregion


    /*
    /// <summary>
    /// Example of Parsing
    /// </summary>
    /// <param name="data"> Item to Parse, in this case a list with items</param>
    private void RegisterDevicesUWR(string url ,List<NodeHQElement> data)
    {
        url = SAI.SDK.API.host + SAI.SDK.API.routeBookNode;
        

        // Main Object to be Json Serialized
        List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();

        // Sub Objects
        foreach (NodeHQElement element in data)
        {
            if (element.toggle.isOn)
            {
                Dictionary<string, object> requestData = new Dictionary<string, object>
                {
                    {"node", element.nodeCode},
                    {"quantity", element.nodeQuantity}
                };

                dataList.Add(requestData);
            }
        }
       
        //string temporal = "[{ \"device\":1,\"quantity\":1}]";
        string jsonData = JsonConvert.SerializeObject(dataList, Formatting.Indented);

        StartCoroutine(SendPostRequest(url, jsonData, callbackFunction));
    }

  
   private void callbackFunction(UnityWebRequest response)
    {

        if (response.result != UnityWebRequest.Result.Success)
        {
            print("Request Failed");           

        }
        else
        {
            print("Request OK"+ response.downloadHandler.text);           

        }
    }


    
    public void SendPutRequest(string url, string data, ApiCallBack callback)
    {
        SAI.SDK.logger.LogToFile($"Iniciando Corutina SendJsonPostRequest", "Log.txt");
        StartCoroutine(Co_SendPutRequest(url, data, callback));
    }
    private IEnumerator Co_SendPutRequest(string url, string jsonData, ApiCallBack callBack)
    {

        SAI.SDK.logger.LogToFile($"Corutina iniciada", "Log.txt");
        UnityWebRequest request = UnityWebRequest.Put(url, jsonData);

        byte[] array = null;
        array = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(array);
        request.uploadHandler.contentType = "application/json";
        request.SetRequestHeader("spaceaisess", SAI.SDK.API.sessionKey);
        SAI.SDK.logger.LogToFile($"Agregando Header Seesion Key : {SAI.SDK.API.sessionKey}", "Log.txt");

        if (SAI.SDK.API.csrfToken != String.Empty)
        {
            request.SetRequestHeader("csrfToken", SAI.SDK.API.csrfToken);
            request.SetRequestHeader("X-CSRFToken", SAI.SDK.API.csrfToken);
            SAI.SDK.logger.LogToFile($"Agregando Header csrfToken : {SAI.SDK.API.csrfToken}", "Log.txt");
        }


        yield return request.SendWebRequest();

        SAI.SDK.logger.LogToFile($"Request Enviada a {url}", "Log.txt");
        if (request.result != UnityWebRequest.Result.Success)
        {

            string responseText = request.downloadHandler.text;

            SAI.SDK.logger.LogToFile($"Request Response BAD : {responseText}", "Log.txt");


            Debug.LogError("Error HTTP: " + request.responseCode); // Codigo de respuesta HTTP

            if (!string.IsNullOrEmpty(request.error))
            {
                Debug.LogError("Mensaje de error: " + request.error); // Mensaje de error proporcionado por Unity
            }

            if (!string.IsNullOrEmpty(responseText))
            {
                Debug.LogError("Contenido del error: " + responseText); // Mensaje de error proporcionado por la API
            }

            print("Request Failed: " + responseText);
            callBack(request);
        }
        else
        {
            string responseText = request.downloadHandler.text;
            SAI.SDK.logger.LogToFile($"Request Response OK : {responseText}", "Log.txt");
            print("Request Success: " + responseText);
            callBack(request);
        }
    }

    */
}