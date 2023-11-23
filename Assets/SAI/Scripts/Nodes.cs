using Newtonsoft.Json;

using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Nodes : MonoBehaviour
{
    [Header("Endpoints")]
    public string GetNodeListEndpoint = "/api/getnodelist/";
    public string GetNodeListClusterEndpoint = "/api/getnodelistcluster";
    public string PostAddNode = "/api/node";
    public string NodeType = "/api/nodetype";
    public string PostBookNode = "/api/booknode";
    public string GetNodeInfo = "/api/getnodeinfo/";
    public string GetValidateNodeExists = "/api/node/validateNodeExists";
    public string PostAddSensor = "/api/addNodeSensor";
    public string GetPostPutNodeSensor = "/api/nodesensor";


    public List<NodeData> NodeListExample;
    public List<NodeData> NodeFromClusterListExample;
    public List<NodeData> NodeByIDExample;

    public NodeTypes _nodeTypes = NodeTypes.BC;
    public enum NodeTypes
    {
        BC,
        CO,
        SDR,
        FULL
    }

    public void ConvertNodeType(int index)
    {
        // Obtener el valor del enum correspondiente al índice seleccionado
        _nodeTypes = (NodeTypes)index;

    }

    
    private void GetNodeListExample()
    {
        NodeListExample = GetNodeList();
        
    }
    
    private void GetNodeListClusterExample(Cluster cluster)
    {
        NodeFromClusterListExample = GetNodeFromCluster(cluster);
    }
    
    private void GetNodeByIDExample(int ID)
    {
        List<Cluster> myClusters = SAI.SDK.Clusters.GetClusterList();
        if (ID <= myClusters.Count)
            NodeByIDExample = GetNodeByID(ID);
        else print("Invalid ID"); 
        
    }    
   
    public List<NodeData> GetNodeList()
    {
        //string url = "http://127.0.0.1:8000/api/getnodelist/";
        string url = SAI.SDK.API.host + GetNodeListEndpoint;
        string jsonResponse = SAI.SDK.API.GenericGetRequest(url);

        NodesDataList response = JsonUtility.FromJson<NodesDataList>(jsonResponse);

        if (response != null)
        {
            return response.nodes;
        }
        else
        {
            Debug.LogError("Error al analizar los datos JSON.");


            string jsonData = "X";

            SAI.SDK.Util.ErrorHandler.ShowPopupJson(jsonData);




            return new List<NodeData>();
        }
    }
    public List<NodeData> GetNodeFromCluster(Cluster cluster)
    {


        string url = SAI.SDK.API.host+ GetNodeListClusterEndpoint + "?cluster_mac=" + cluster.cluster_mac;
        string jsonResponse = SAI.SDK.API.GenericGetRequest(url);
        NodesDataList response = JsonUtility.FromJson<NodesDataList>(jsonResponse);

        if (response != null)
            return response.nodes;
        else
        {
            Debug.LogError("Error al analizar los datos JSON.");
            return new List<NodeData>();
        }
    }
    public List<NodeData> GetNodeByID(int id)
    {
        

        string url = SAI.SDK.API.host + GetNodeListClusterEndpoint + "?cluster_mac=" + SAI.SDK.Clusters.GetClusterList()[id].cluster_mac;
        string jsonResponse = SAI.SDK.API.GenericGetRequest(url);
        NodesDataList response = JsonUtility.FromJson<NodesDataList>(jsonResponse);

        if (response != null)
            return response.nodes;
        else
        {
            Debug.LogError("Error al analizar los datos JSON.");
            return null;
        }
    }
    public bool AddNode(string name,string mac,string nodetype,string modified,string altitude, string longitude, string latitude,int status,string clustermac)
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

        string url = SAI.SDK.API.host + PostAddNode;
        string json = JsonConvert.SerializeObject(nodeCreation,Formatting.Indented);


        string response = SAI.SDK.API.GenericPostRequest(url, json);
        if (response != string.Empty) return true;
        else return false;
    }
 
    public bool AddSensor(string node_mac,string SensorTypeCode,string AddOkValue, string AddMidValue, string AddErrorValue,string AddFormulaValue)
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

        Debug.Log("SENSOR CONFIG: " + jsonConfig);

        string url = SAI.SDK.API.host+ SAI.SDK.Nodes.PostAddSensor;

        if (SAI.SDK.API.GenericPostRequest(url, jsonConfig) != string.Empty)
        { 
            return true;
        }
        else return false;
    }

    public bool EditSensor(string sensorId, string OkValue,string MidValue,string ErrorValue,string FormulaValue)
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

        Debug.Log("SENSOR CONFIG: " + jsonConfig);

        string url = SAI.SDK.API.host+SAI.SDK.Nodes.GetPostPutNodeSensor;

        if(SAI.SDK.API.GenericPutRequest(url, jsonConfig) != string.Empty)
        {
            return true;
        }return false;

    }

    public bool Add_New_Device(string deviceName, string mac, string cluster_mac, int nodeType, string latitude, string longitude)
    {


        //


        _nodeTypes = (NodeTypes)nodeType;
        string url = SAI.SDK.API.host + SAI.SDK.Nodes.PostAddNode;
        newDevice device = new newDevice();
        device.mac = mac;
        device.name = deviceName;
        device.nodetype = _nodeTypes.ToString();
        device.op_system = SAI.SDK.Devices.GetDeviceSO();
        device.cluster = cluster_mac;
        device.latitude = latitude;
        device.longitude = longitude;
        device.altitude = "";

        string json = JsonConvert.SerializeObject(device, Formatting.Indented);


        string response = SAI.SDK.API.GenericPostRequest(url, json);
        if (response != string.Empty)
        {
            print("Exito al agregar el nodo" + response);
            return true;
        }
        else
        {
            return false;
        }






    }

}

[System.Serializable]
public class SensorConfiguration
{
    public SensorStatus status;
    public string formula;



}

[System.Serializable]
public class NodesResponse
{
    public List<node_single> nodes;
}

[System.Serializable]
public class node_single
{
    public string node_name = "";
    public string node_mac = "";
    public string node_status = "on";
    public float node_altitude = 0f;
    public float node_longitude = 0f;
    public float node_latitude = 0f;
    public string last_modification = "2023-10-05T15:16:40:40.592010Z";
    public string nodetype_name = "";
    public string nodetype_description = "";
    public string permissiontype_description = "";
    public string owner_username = "";

}
[System.Serializable]
public class NodesDataList
{
    public List<NodeData> nodes;
}

[System.Serializable]
public class TempNode
{
    public Node node;
}

[System.Serializable]
public class OnlineNodesListObject
{
    public List<OnlineNodesClass> onlineNodes;
}

[System.Serializable]
public class TelemetryData
{
    public string date;
    public TelemetryDetail detail;
}

[System.Serializable]
public class SensorData
{
    public string date;
    public SensorDetail detail;
}

[System.Serializable]
public class TelemetryDetail
{
    public string name;
    public float grav_x;
    public float grav_y;
    public float grav_z;
    public string userID;
    public float altitude;
    public float latitude;
    public float longitude;
    public float board_temp;
    public float lin_accel_x;
    public float lin_accel_y;
    public float lin_accel_z;
}

[System.Serializable]
public class SensorDetail
{
    public string name;
    public string userID;
    public float boardTemp;
    public string dateStamp;
    public string timeStamp;
    public float voltageVibr;
    public float currentSensor;
    public float voltageSensor3;
    public float voltageSensor100;
}

[System.Serializable]
public class RootObject
{
    public List<TelemetryData> telemetry_data;
    public List<SensorData> sensor_data;
}


[System.Serializable]
public class SensorListObject
{
    public List<Sensor> Sensor;
}

[System.Serializable]
public class SensorStatus
{
    public int ok;
    public int mid;
    public int error;
}

[System.Serializable]
public class SensorDataWrapper
{
    public SensorFromNodeData[] data;
}

[System.Serializable]
public class SensorFromNodeData
{
    public int nodesensor_id;
    public string node_mac;
    public string node_name;
    public string sensortype_code;
    public string sensortype_name;
    public SensorConfiguration nodesensor_config;
}