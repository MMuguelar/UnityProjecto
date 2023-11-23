using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Net;
using System.IO;
using System;
using System.Linq;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using Ipfs;
using System.Text;
using static System.Net.WebRequestMethods;


public class NodeControl : MonoBehaviour
{

    //Class needed to get JSON

  


    //[Serializable]
    //public class TelemetryDataList
    //{
    //    public List<TelemetryDetail> telemetry;
    //    public List<SensorDetail> sensorHistory;
    //}

    //[Serializable]
    //public class SensorDataList
    //{
    //    public List<SensorDetail> sensorHistory;
    //}









   

    //public string NodeInfoURL = "https://networkmonitor.sos.space/getNodeInfo?id=";
    //public string NodeInfoURL = SAI.subsystems.api.host + SAI.subsystems.api.routeGetNodeInfo;
    public string NodeInfoURLLocal = "http://127.0.0.1:8000/api/getnodeinfo/";

    //JSON Asset
    public TextAsset textJSONNodes;

    public TextAsset textJSONSensors;

    public TextAsset textJSONOnlineNodes;

    public TextAsset textJSONTest;

    public SensorListObject sensorListObj = new SensorListObject();

    public OnlineNodesListObject onlineNodesListObj = new OnlineNodesListObject();

    public List<Node> nodeList = new List<Node>();

    public List<Sensor> sensorList = new List<Sensor>();

    public List<OnlineNodesClass> onlineNodesList = new List<OnlineNodesClass>();

    private List<TelemetryData> telemetryDataList = new List<TelemetryData>();
    private List<SensorData> sensorDataList = new List<SensorData>();

    public Action<ClustersList> OnClusterListReceived;



    // Returns a list of nodes .
    //TO DO: hardcoded the list of nodes to search for now
    //public List<Node> GetNodeList(string s, string order)
    //{
    //    nodeList.Clear();
    //    //Create an object from its JSON representation.
    //    onlineNodesListObj = JsonUtility.FromJson<onlineNodesListObject>(textJSONOnlineNodes.text);



    //    onlineNodesList = onlineNodesListObj.onlineNodes;

    //    for (int i = 0; i < onlineNodesList.Count; i++)
    //    {
    //        AddNodeInfoToNodeList(onlineNodesList[i].adress, onlineNodesList[i].name);
    //    }
    //    return nodeList;
    //}

   


    List<NodeData> ParseJsonResponse(string jsonResponse)
    {
        NodesDataList response = JsonUtility.FromJson<NodesDataList>(jsonResponse);

        if (response != null)
        {
            return response.nodes;
        }
        else
        {
            Debug.LogError("Error al analizar los datos JSON.");
            return new List<NodeData>(); // Devuelve una lista vacía si falla el análisis
        }
    }


    public void test(ref RootObject data)
    {
        string mac = SharedData.Mac;
        int offset = SharedData.offset;

        string urlWithQueryString = SAI.SDK.API.host + SAI.SDK.Nodes.GetNodeInfo;

        NodeInfoRequest request = new NodeInfoRequest();
        request.mac_node = mac;

        var test = new
        {
            mac_node = mac,
            limit = 5,
            offset = offset,
        };



        string jsonRequestBody = JsonConvert.SerializeObject(test); // debe ir request

        using (UnityWebRequest webRequest = new UnityWebRequest(urlWithQueryString, "GET"))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("spaceaisess", SAI.SDK.Login.SessionKey);
            //SAI.subsystems.api.sessionKey
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonRequestBody);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
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

                data = ParseJSONData(jsonResponse);
            }
        }
    }

    public void dataSearch(ref RootObject data) //
    {
        string mac = SharedData.Mac;
        string isoFrom = SharedData.dateFrom;
        string isoTo = SharedData.dateTo;
        int offset = SharedData.offset;

        string url = SAI.SDK.API.host + SAI.SDK.Nodes.GetNodeInfo;

        var searchData = new
        {
            mac_node = mac,
            from_ = isoFrom,
            to = isoTo,
            limit = 5,
            offset = offset
        };

        // Serializar el objeto JSON
        string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(searchData);

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
                data = ParseJSONData(jsonResponse);
            }
        }
    }


    public RootObject ParseJSONData(string json)
        {
        RootObject data = JsonUtility.FromJson<RootObject>(json);

        // Fill the telemetry list
        foreach (TelemetryData telemetry in data.telemetry_data)
        {
            telemetryDataList.Add(telemetry);
        }

        // Fill the sensor list
        foreach (SensorData sensor in data.sensor_data)
        {
            sensorDataList.Add(sensor);
        }

        //// You can now access the parsed data like this:
        //foreach (TelemetryData telemetry in data.telemetry_data)
        //{
        //    Debug.Log("Telemetry Date: " + telemetry.date);
        //    Debug.Log("Telemetry Name: " + telemetry.detail.name);
        //    // Access other telemetry properties as needed
        //}

        //foreach (SensorData sensor in data.sensor_data)
        //{
        //    Debug.Log("Sensor Date: " + sensor.date);
        //    Debug.Log("Sensor Name: " + sensor.detail.name);
        //    // Access other sensor properties as needed
        //}

        //Debug.Log("Telemetry List:");
        //foreach (TelemetryData telemetry in telemetryDataList)
        //{
        //    Debug.Log("Telemetry Date: " + telemetry.date);
        //    Debug.Log("Telemetry Name: " + telemetry.detail.name);
        //    Debug.Log("Telemetry Grav X: " + telemetry.detail.grav_x);
        //    Debug.Log("Telemetry Grav Y: " + telemetry.detail.grav_y);
        //    Debug.Log("Telemetry Grav Z: " + telemetry.detail.grav_z);
        //    Debug.Log("Telemetry UserID: " + telemetry.detail.userID);
        //    Debug.Log("Telemetry Altitude: " + telemetry.detail.altitude);
        //    Debug.Log("Telemetry Latitude: " + telemetry.detail.latitude);
        //    Debug.Log("Telemetry Longitude: " + telemetry.detail.longitude);
        //    Debug.Log("Telemetry Board Temp: " + telemetry.detail.board_temp);
        //    Debug.Log("Telemetry Lin Accel X: " + telemetry.detail.lin_accel_x);
        //    Debug.Log("Telemetry Lin Accel Y: " + telemetry.detail.lin_accel_y);
        //    Debug.Log("Telemetry Lin Accel Z: " + telemetry.detail.lin_accel_z);
        //}

        //// Debug the sensor list
        //Debug.Log("Sensor List:");
        //foreach (SensorData sensor in sensorDataList)
        //{
        //    Debug.Log("Sensor Date: " + sensor.date);
        //    Debug.Log("Sensor Name: " + sensor.detail.name);
        //    Debug.Log("Sensor UserID: " + sensor.detail.userID);
        //    Debug.Log("Sensor Board Temp: " + sensor.detail.boardTemp);
        //    Debug.Log("Sensor Date Stamp: " + sensor.detail.dateStamp);
        //    Debug.Log("Sensor Time Stamp: " + sensor.detail.timeStamp);
        //    Debug.Log("Sensor Voltage Vibr: " + sensor.detail.voltageVibr);
        //    Debug.Log("Sensor Current Sensor: " + sensor.detail.currentSensor);
        //    Debug.Log("Sensor Voltage Sensor3: " + sensor.detail.voltageSensor3);
        //    Debug.Log("Sensor Voltage Sensor100: " + sensor.detail.voltageSensor100);
        //}

        return data;
    }

    //public List<NodeData> GetNodeInfo()
    //{
    //    string url = "http://localhost:8001/api/getnodeinfo/";

    //    using (UnityWebRequest www = UnityWebRequest.Get(url))
    //    {
    //        www.SendWebRequest();

    //        while (!www.isDone)
    //        {
    //            // Wait for the request to complete
    //        }

    //        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
    //        {
    //            Debug.LogError("Error: " + www.error);
    //            return new List<NodeData>(); // Return an empty list on error
    //        }
    //        else
    //        {
    //            // Request succeeded, parse and return the list of nodes
    //            string jsonResponse = www.downloadHandler.text;
    //            Debug.Log("Response: " + jsonResponse);

    //            return ParseJsonResponse(jsonResponse);
    //        }
    //    }
    //}



    //List<HistoryData> ParseJsonResponseH(string jsonResponse)
    //{
    //    HistoryDataList response = JsonUtility.FromJson<HistoryDataList>(jsonResponse);

    //    if (response != null)
    //    {
    //        return response.telemetryData;
    //    }
    //    else
    //    {
    //        Debug.LogError("Failed to parse JSON data.");
    //        return new List<HistoryData>(); // Return an empty list if parsing fails
    //    }
    //}

    public Sensor GetSensorList(string _ipfstoken)
    {
        Sensor sensor = null;

        //if (_ipfstoken != null)
        //{
        //    string url = "http://dfs.sos.space:8085/ipfs/" + _ipfstoken;
        //    Debug.Log(url);

        //    UnityWebRequest request = UnityWebRequest.Get(url);
        //    request.timeout = 15; // Adjust the timeout value as needed

        //    // Send the request
        //    request.SendWebRequest();

        //    while (!request.isDone) // Wait for the request to finish
        //    {
        //        // You can add additional code here if needed
        //    }

        //    if (request.result == UnityWebRequest.Result.Success)
        //    {
        //        string responseText = request.downloadHandler.text;
        //        sensor = JsonConvert.DeserializeObject<Sensor>(responseText);
        //    }
        //    else
        //    {
        //        Debug.LogError($"Request failed: {request.error}");
        //    }
        //}
        //else
        //{
        //    Debug.LogError("IPFS token is null.");
        //}

        return sensor;
    }


    //Returns info from one node. if not online, returns it with status = "off"
    public Node getNodeInfo(string _adress, string _name)
    {
        Node newNode = new Node();
        string url = NodeInfoURLLocal + _adress;
        //string url = SAI.subsystems.api.host + SAI.subsystems.api.routeGetNodeInfo + _adress;

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        //request.Method = "GET";
        request.Timeout = 5000;
        request.ContentType = "application/json";

        ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };


        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        // Get the response stream
        StreamReader reader = new StreamReader(response.GetResponseStream());

        // Read the response and store it in a string
        string responseText = reader.ReadToEnd();


        if (responseText == "Must match a node!")
        {
            position pos = new position();
            pos.lat = "0";
            pos.@long = "0";
            newNode.name = "Node OFF";
            newNode.status = false;
            newNode.position = pos;
            //newNode.position.lon = "lon";
        }
        else
        {
            // cambiar ahora para que use un objeto y adentro un nodo
            NodeResponse tempNode = JsonUtility.FromJson<NodeResponse>(responseText);

            //this part is commented to only get the JSONOnlineNodes name, and not the IPFS one
            //newNode.name = tempNode.node.name;
            newNode.name = _name;
            newNode.status = tempNode.status;
            newNode.adress = tempNode.node.adress;
            newNode.position = tempNode.node.position;
            newNode.lastHash = tempNode.node.lastHash;
        }
        newNode.adress = _adress;
        reader.Close();
        return newNode;
    }

    //Adds node info to the list of nodes
    public void AddNodeInfoToNodeList(string _adress, string _name)
    {
        //get info for one node

        Debug.Log("ONE NODE ADRESS: " + _adress);
        Node oneNode= getNodeInfo(_adress, _name);
        Debug.Log("ONE NODE STATUS: " + oneNode.status);

        //add node to list
        nodeList.Add(oneNode);
    }

    //public List<Node> GetNodeListLocal(string s, string order)
    //{
    //    nodeList.Clear();

    //    // If textJSONOnlineNodes is not empty, use it to get the online nodes list
    //    if (!string.IsNullOrEmpty(textJSONOnlineNodes.text))
    //    {
    //        JsonData jsonData = JsonConvert.DeserializeObject<JsonData>(textJSONOnlineNodes.text);
    //        List<string> onlineNodeNames = jsonData.onlineNodes;

    //        for (int i = 0; i < onlineNodeNames.Count; i++)
    //        {
    //            if (onlineNodeNames[i] != "sosHost")
    //            {
    //                OnlineNodesClass onlineNode = new OnlineNodesClass(onlineNodeNames[i], "Node " + i);
    //                onlineNodesList.Add(onlineNode);
    //            }
    //        }
    //    }

    //    // If textJSONOnlineNodes is empty, get the online nodes list from the given URL
    //    if (onlineNodesList == null || onlineNodesList.Count == 0)
    //    {
    //        //string url = "http://127.0.0.1:8000/api/getnodelist/";
    //        string url = SAI.subsystems.api.host + SAI.subsystems.api.routeGetNodeList;

    //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
    //        request.Method = "GET";
    //        request.Timeout = 10000;

    //        ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

    //        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
    //        // Get the response stream

    //        StreamReader reader = new StreamReader(response.GetResponseStream());

    //        // Read the response and store it in a string
    //        string responseText = reader.ReadToEnd();

    //        Debug.Log("RESPONSEEEEE: " + responseText);

    //        var jsonResult = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(responseText);

    //        // Extract the "onlineNodes" array
    //        if (jsonResult.TryGetValue("onlineNodes", out List<string> onlineNodeNames))
    //        {
    //            // Do something with the onlineNodes array
    //            for (int i = 0; i < onlineNodeNames.Count; i++)
    //            {
    //                if (onlineNodeNames[i] != "sosHost")
    //                {
    //                    OnlineNodesClass onlineNode = new OnlineNodesClass(onlineNodeNames[i], "Node " + i);
    //                    onlineNodesList.Add(onlineNode);
    //                }
    //            }
    //        }

    //    }

    //    for (int i = 0; i < onlineNodesList.Count; i++)
    //    {
    //        AddNodeInfoToNodeList(onlineNodesList[i].adress, onlineNodesList[i].name);
    //    }
    //    Debug.Log("ONLINE NODE LIST: " + nodeList);
    //    return nodeList;
    //}
}
