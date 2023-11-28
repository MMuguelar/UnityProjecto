using Newtonsoft.Json;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using Unity.VisualScripting;



namespace SAISDK
{
    public static class LoginSys
    {
        public static string SessionKey = "";
        public static string crsfToken = "";
        public static IEnumerator Login(string email, string password, Action<bool> callback)
        {
            Debug.Log("Loggin In");
            string url = ApiRoutes.Login;
            WWWForm form = new WWWForm();
            form.AddField("email", email);
            form.AddField("password", password);

            using (UnityWebRequest www = UnityWebRequest.Post(url, form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    string JsonError = www.downloadHandler.text;
                    ErrorClass err = JsonUtility.FromJson<ErrorClass>(JsonError);
                    
                    Debug.LogError("Error: " + www.error);
                    EditorUtility.DisplayDialog("Error:", err.error, "Ok");
                    
                    callback(false); // Llama al callback con 'false' para indicar un fallo.
                }
                else
                {

                    // Extraemos la SessionKey del Usuario
                    JSONNode responseJson = JSON.Parse(www.downloadHandler.text);
                    SessionKey = responseJson["session_key"];
                    crsfToken = responseJson["csrftoken"];
                    PlayerPrefs.SetString("sessionKey", SessionKey);
                    PlayerPrefs.SetString("csrfToken", responseJson["csrftoken"]);

                    // La solicitud fue exitosa
                    callback(true); // Llama al callback con 'true' para indicar �xito.
                }
            }
        }
        public static IEnumerator Register(string email, string username, string password, string phone, Action<bool> callback, string city = "", string placeId = "")
        {
            string url = ApiRoutes.Register;

            WWWForm registrationData = new WWWForm();
            registrationData.AddField("email", email);
            registrationData.AddField("username", username);
            registrationData.AddField("password", password);
            registrationData.AddField("phone", phone);
            registrationData.AddField("city", city);
            registrationData.AddField("placeid", placeId);

            using (UnityWebRequest www = UnityWebRequest.Post(url, registrationData))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    string JsonError = www.downloadHandler.text;
                    ErrorClass err = JsonUtility.FromJson<ErrorClass>(JsonError);

                    Debug.LogError("Error: " + www.downloadHandler.text);
                    EditorUtility.DisplayDialog("Error:", err.error, "Ok");
                    callback(false); // Llama al callback con 'false' para indicar un fallo.
                }
                else
                {
                    // La solicitud fue exitosa
                    callback(true); // Llama al callback con 'true' para indicar �xito.
                }
            }

        }
        public static IEnumerator LogOut(Action<bool> callback)
        {
            Debug.Log("Im Logging Out!");
            string url = ApiRoutes.Logout;

            WWWForm form = new WWWForm();

            using (UnityWebRequest www = UnityWebRequest.Post(url, form))
            {
                www.SetRequestHeader("Content-Type", "application/json");
                www.SetRequestHeader("Authorization", "Bearer " + crsfToken);
                www.SetRequestHeader("X-CSRFToken", crsfToken);
                www.SetRequestHeader("csrftoken", crsfToken);
                www.SetRequestHeader("spaceaisess", SessionKey);

                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    string JsonError = www.downloadHandler.text;
                    ErrorClass err = JsonUtility.FromJson<ErrorClass>(JsonError);

                    Debug.Log("Logout Failed : " + www.downloadHandler.text);
                    EditorUtility.DisplayDialog("Error:", err.error, "Ok");
                    
                    callback(false);

                }
                else
                {
                    Debug.Log("Logout OK : " + www.downloadHandler.text);
                    callback(true);
                }
            }
        }
        public static IEnumerator ResetPassword(string email,Action<bool>callback)
        {
            string url = ApiRoutes.PasswordReset;
            var requestData = new
            {
                email = email
            };
            string data = JsonConvert.SerializeObject(requestData);
            yield return ApiCalls.PostRequest(url, data, response => { 
                if(response != string.Empty)
                {
                    callback(true);
                }
                else
                {
                    callback(false);
                }
            
            });
        }

    }
    public static class ClusterSys
    {
        public static IEnumerator GetClusters(Action<List<Cluster>> callback)
        {
            string url = ApiRoutes.GetClusterList;

            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                www.SetRequestHeader("Content-Type", "application/json");
                if (LoginSys.SessionKey != string.Empty) www.SetRequestHeader("spaceaisess", LoginSys.SessionKey);
                string security = "{}";
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(security);
                www.uploadHandler = new UploadHandlerRaw(bodyRaw);
                www.downloadHandler = new DownloadHandlerBuffer();

                string jsonRequestBody = JsonConvert.SerializeObject(security);

                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                {
                    string result = www.downloadHandler.text;
                    Debug.LogError("API.CS Error: " + www.error + result);
                    SAI.SDK.errorHandler.ShowPopupJson(result);
                    callback(null);

                }
                else
                {

                    string jsonResponse = www.downloadHandler.text;
                    ClusterResponse response = JsonUtility.FromJson<ClusterResponse>(jsonResponse);
                    if (response != null)
                    {
                        callback(response.clusters);

                    }
                    else
                    {
                        Debug.LogError("Error al analizar los datos JSON.");
                        callback(null);

                    }
                }

            }
        }
        public static IEnumerator AddCluster(string clusterName, string ClusterDescription, Action<bool> callback)
        {
            string addClusterName = Regex.Replace(clusterName, @"[^\x20-\x7E]", "");
            string addClusterDescription = Regex.Replace(ClusterDescription, @"[^\x20-\x7E]", "");

            var requestData = new
            {
                name = addClusterName,
                description = addClusterDescription
            };

            string jsonConfig = JsonConvert.SerializeObject(requestData);
            string url = ApiRoutes.PostAddCluster;

            yield return ApiCalls.PostRequest(url, jsonConfig, (response =>
             {
                 if (response != null)
                 {
                     // Procesar la respuesta aqu�
                     Debug.Log("Respuesta de la API: " + response);
                     callback(true);
                 }
                 else
                 {
                     // Manejar el error aqu�
                     Debug.LogError("Error al obtener la respuesta de la API.");
                     callback(false);
                 }
             }));

        }
    }
    public static class NodeSys
    {
        public static NodeTypes _nodeTypes = NodeTypes.BC;
        public enum NodeTypes
        {
            BC,
            CO,
            SDR,
            FULL
        }
        public static IEnumerator GetNodeList(Action<List<NodeData>>callback)
        {
            string url = ApiRoutes.GetNodeListEndpoint;
            yield return ApiCalls.GetRequest(url, response => {
                if (response != string.Empty)
                {
                    NodesDataList node = JsonUtility.FromJson<NodesDataList>(response);
                    if (node != null)
                    {
                        callback(node.nodes);
                    }
                    else
                    {
                        Debug.Log("Error getting nodes");
                        callback(null);
                    }
                }
               
            });

        }
        public static IEnumerator GetNodeListFromCluster(Cluster cluster,Action<List<NodeData>>callback)
        {
            string url = ApiRoutes.GetNodeListClusterEndpoint + "?cluster_mac=" + cluster.cluster_mac;
            yield return ApiCalls.GetRequest(url, response => {
            if(response != string.Empty)
                {
                    NodesDataList NodeData = JsonUtility.FromJson<NodesDataList>(response);
                    if(NodeData != null)
                    {
                        callback(NodeData.nodes);
                    }
                    else
                    {
                        Debug.Log("Error getting nodes");
                        callback(null);
                    }
                }
            });
            
        }
        public static IEnumerator AddNode(string name,string mac,string nodetype,string modified,string altitude,string longitude,string latitude,int status,string clustermac, Action<bool> callback)
        {
            NodeCreation nodeCreation = new NodeCreation();
            nodeCreation.name = name;
            nodeCreation.mac = mac;
            nodeCreation.nodetype = nodetype;
            nodeCreation.modified = modified;
            nodeCreation.altitude = altitude;
            nodeCreation.longitude = longitude;
            nodeCreation.latitude = latitude;
            nodeCreation.status = status;
            nodeCreation.cluster = clustermac;

            string url = ApiRoutes.PostAddNode;
            string json = JsonConvert.SerializeObject(nodeCreation, Formatting.Indented);
            yield return ApiCalls.PostRequest(url, json, response => {
            if(response != string.Empty)
                {
                    callback(true);
                }
                else
                {
                    callback(false);
                }
            
            });




        }
        public static IEnumerator AddSensor(string node_mac, string SensorTypeCode, string AddOkValue, string AddMidValue, string AddErrorValue, string AddFormulaValue,Action<bool>callback)
        {
            string addSensorTypeCode = Regex.Replace(SensorTypeCode, @"[^\x20-\x7E]", "");
            int addOkValue = int.Parse(Regex.Replace(AddOkValue, @"[^\x20-\x7E]", ""));
            int addMidValue = int.Parse(Regex.Replace(AddMidValue, @"[^\x20-\x7E]", ""));
            int addErrorValue = int.Parse(Regex.Replace(AddErrorValue, @"[^\x20-\x7E]", ""));
            string addFormulaValue = Regex.Replace(AddFormulaValue, @"[^\x20-\x7E]", "");

            SensorConfiguration newConfig = new SensorConfiguration
            {
                status = new SensorStatus
                {
                    ok = addOkValue,
                    mid = addMidValue,
                    error = addErrorValue
                },
                formula = addFormulaValue
            };

            var requestData = new
            {
                node_mac = node_mac,
                sensortype_code = addSensorTypeCode,
                config = newConfig
            };

            string jsonConfig = JsonConvert.SerializeObject(requestData);           

            string url = ApiRoutes.PostAddSensor;

            yield return ApiCalls.PostRequest(url, jsonConfig, response => {

                if (response != string.Empty)
                {
                    callback(true);
                }
                else
                {
                    callback(false);
                }
            });
            
        }
        public static IEnumerator EditSensor(string sensorId, string OkValue, string MidValue, string ErrorValue, string FormulaValue,Action<bool> callback)
        {
            int okValue = int.Parse(Regex.Replace(OkValue, @"[^\x20-\x7E]", ""));
            int midValue = int.Parse(Regex.Replace(MidValue, @"[^\x20-\x7E]", ""));
            int errorValue = int.Parse(Regex.Replace(ErrorValue, @"[^\x20-\x7E]", ""));
            string formulaValue = Regex.Replace(FormulaValue, @"[^\x20-\x7E]", "");

            SensorConfiguration newConfig = new SensorConfiguration
            {
                status = new SensorStatus
                {
                    ok = okValue,
                    mid = midValue,
                    error = errorValue
                },
                formula = formulaValue
            };

            string jsonConfig = JsonConvert.SerializeObject(new { id = int.Parse(Regex.Replace(sensorId, @"[^\x20-\x7E]", "")), config = newConfig });

            

            string url = ApiRoutes.GetPostPutNodeSensor;

            yield return ApiCalls.PutRequest(url, jsonConfig, response => {
                if (response != string.Empty)
                {
                    callback(true);
                }
                else
                {
                    callback(false);
                }
            });           

        }
        public static IEnumerator AddNewDevice(string deviceName, string mac, string cluster_mac, int nodeType, string latitude, string longitude,Action<bool>callback)
        {

            _nodeTypes = (NodeTypes)nodeType;
            string url = ApiRoutes.PostAddNode;
            newDevice device = new newDevice();
            device.mac = mac;
            device.name = deviceName;
            device.nodetype = _nodeTypes.ToString();
            device.op_system = "TO-DO"; //SAI.SDK.Devices.GetDeviceSO(); // Change This
            Debug.Log("Change this once SDK is Done");
            device.cluster = cluster_mac;
            device.latitude = latitude;
            device.longitude = longitude;
            device.altitude = "";

            string json = JsonConvert.SerializeObject(device, Formatting.Indented);


            yield return ApiCalls.PostRequest(url, json, response => {
                if (response != string.Empty) callback(true);
                else callback(false);
            });          

        }
    }
    public static class ApiCalls
    {

        public static IEnumerator PostRequest(string url, string jsonData, Action<string> callback)
        {
            Debug.Log($"Generic Post Request : URL={url} JSONDATA={jsonData}");

            //SAI.subsystems.logger.LogToFile($"Corutina iniciada", "Log.txt");
            UnityWebRequest request = UnityWebRequest.PostWwwForm(url, jsonData);

            byte[] array = null;
            array = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(array);
            request.uploadHandler.contentType = "application/json";
            if (LoginSys.SessionKey != String.Empty)
            {                
                request.SetRequestHeader("spaceaisess", LoginSys.SessionKey);
            }

            //SAI.subsystems.logger.LogToFile($"Agregando Header Seesion Key : {SAI.subsystems.api.sessionKey}", "Log.txt");

            if (LoginSys.crsfToken != String.Empty)
            {
                request.SetRequestHeader("csrfToken", LoginSys.crsfToken);
                request.SetRequestHeader("X-CSRFToken", LoginSys.crsfToken);
                //SAI.subsystems.logger.LogToFile($"Agregando Header csrfToken : {SAI.subsystems.api.csrfToken}", "Log.txt");
            }


             yield return request.SendWebRequest();          

            if (request.result != UnityWebRequest.Result.Success)
            {
                string JsonError = request.downloadHandler.text;
                ErrorClass err = JsonUtility.FromJson<ErrorClass>(JsonError);
                    
                EditorUtility.DisplayDialog("Error:", err.error, "Ok");
                //string responseText = request.downloadHandler.text;     
                Debug.LogError("Error HTTP: " + request.responseCode + " " + JsonError); // Codigo de respuesta HTTP
                callback(""); ;
            }
            else
            {
                string responseText = request.downloadHandler.text;  
                ErrorClass err = JsonUtility.FromJson<ErrorClass>(responseText);
                EditorUtility.DisplayDialog("Error:", err.message, "Ok");              
                Debug.Log("Request Success, sending to callback" + err.message);
                callback(responseText);
            }
        }
        public static IEnumerator GetRequest(string url, Action<string> callback)
        {
            Debug.Log($"Get Request to {url}");

            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                // Agrega la clave como un encabezado personalizado
                www.SetRequestHeader("Content-Type", "application/json");
                if (LoginSys.SessionKey != string.Empty) www.SetRequestHeader("spaceaisess", LoginSys.SessionKey);
                string security = "{}";

                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(security);
                www.uploadHandler = new UploadHandlerRaw(bodyRaw);
                www.downloadHandler = new DownloadHandlerBuffer();

                string jsonRequestBody = JsonConvert.SerializeObject(security);

                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                {
                    string result = www.downloadHandler.text;
                    Debug.LogError("Response Error: " + www.error + result);

                    callback("");
                }
                else
                {
                    string jsonResponse = www.downloadHandler.text;
                    Debug.Log("API.CS Response: " + jsonResponse);
                    callback(jsonResponse);
                }
            }
        }

        public static IEnumerator PutRequest(string url, string jsonData, Action<string> callback)
        {
            Debug.Log($"Generic Put Request : URL={url} JSONDATA={jsonData}");

            //SAI.subsystems.logger.LogToFile($"Corutina iniciada", "Log.txt");
            UnityWebRequest request = UnityWebRequest.Put(url, jsonData);

            byte[] array = null;
            array = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(array);
            request.uploadHandler.contentType = "application/json";
            if (LoginSys.SessionKey != String.Empty)
            {

                request.SetRequestHeader("spaceaisess", LoginSys.SessionKey);
            }


            if (LoginSys.crsfToken != String.Empty)
            {
                request.SetRequestHeader("csrfToken", LoginSys.SessionKey);
                request.SetRequestHeader("X-CSRFToken", LoginSys.SessionKey);
            }


            yield return request.SendWebRequest();


            if (request.result != UnityWebRequest.Result.Success)
            {

                string responseText = request.downloadHandler.text;


                Debug.LogError("Error HTTP: " + request.responseCode + " " + responseText); // Codigo de respuesta HTTP


                if (!string.IsNullOrEmpty(request.error))
                {
                    Debug.LogError("Mensaje de error: " + request.error); // Mensaje de error proporcionado por Unity
                }

                if (!string.IsNullOrEmpty(responseText))
                {
                    Debug.LogError("Contenido del error: " + responseText); // Mensaje de error proporcionado por la API
                }

                Debug.Log("Request Failed, sending to callback" + responseText);
                callback("");
            }
            else
            {
                string responseText = request.downloadHandler.text;
                Debug.Log("Request Success, sending to callback" + responseText);
                callback(responseText);
            }
        }


    }
    public static class ApiRoutes
    {
        // LOGIN
        public static string Login = "https://apiv2.sos.space/api/login/";
        public static string Register = "https://apiv2.sos.space/api/signup/";
        public static string Logout = "https://apiv2.sos.space/api/logout/";
        public static string PasswordReset = "https://apiv2.sos.space/api/passwordreset";

        // CLUSTERS
        public static string GetClusterList = "https://apiv2.sos.space/api/getuserclusters/";
        public static string GetClusterTransactionList = "https://apiv2.sos.space/api/getclustertransactionslist/";
        public static string PostAddCluster = "https://apiv2.sos.space/api/cluster/";

        // NODES
        public static string GetNodeListEndpoint = "https://apiv2.sos.space/api/getnodelist/";
        public static string GetNodeListClusterEndpoint = "https://apiv2.sos.space/api/getnodelistcluster";
        public static string PostAddNode = "https://apiv2.sos.space/api/node";
        public static string NodeType = "https://apiv2.sos.space/api/nodetype";
        public static string PostBookNode = "https://apiv2.sos.space/api/booknode";
        public static string GetNodeInfo = "https://apiv2.sos.space/api/getnodeinfo/";
        public static string GetValidateNodeExists = "https://apiv2.sos.space/api/node/validateNodeExists";
        public static string PostAddSensor = "https://apiv2.sos.space/api/addNodeSensor";
        public static string GetPostPutNodeSensor = "https://apiv2.sos.space/api/nodesensor";

    }
 [System.Serializable]
    public class ErrorClass
    {
        public string error;
        public string detail;
        public string message;
    }
}

