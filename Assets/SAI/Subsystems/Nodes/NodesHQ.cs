using Newtonsoft.Json;
using SimpleJSON;

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class NodesHQ : MonoBehaviour
{
    public List<NodeHQElement> Nodes;

    #region PostWihoutToken

    
    public void RegisterNodes()
    {
        SAI.SDK.logger.LogToFile("Registrando Nodos", "Log.txt");
        RegisterDevicesUWR(Nodes);
    }

    public void RegisterDevicesUWR(List<NodeHQElement> nodes)
    {
        string url = SAI.SDK.API.host + SAI.SDK.Nodes.PostBookNode;
        Debug.Log("URL DEVICES: " + url);

        SAI.SDK.logger.LogToFile($"Registrando Nodos en {url} ", "Log.txt");

        List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();

        if (nodes.Count == 0)
            nodes = FindObjectsOfType<NodeHQElement>().ToList();

        foreach (NodeHQElement node in nodes)
        {
            if (node.toggle.isOn)
            {
                Dictionary<string, object> requestData = new Dictionary<string, object>
                {
                    { "nodetype", node.nodeCode },
                    { "quantity", node.nodeQuantity }
                };

                dataList.Add(requestData);
            }
        }

        Debug.Log(dataList.Count + " NODES ADDED");

        SAI.SDK.logger.LogToFile($"{dataList.Count} Nodos registrados", "Log.txt");

        if (dataList.Count == 0)
        {
            bookSuccess();
            return;
        }

        string jsonData = JsonConvert.SerializeObject(dataList, Formatting.Indented);

        Debug.Log("JSON DATA: " + jsonData);

        SAI.SDK.logger.LogToFile($"Serializando {jsonData}", "Log.txt");

        if (SAI.SDK.API.GenericPostRequest(url, jsonData) != string.Empty)
            bookSuccess();
        else
            bookError();
    }

    public void BookNodeCallback(UnityWebRequest response)
    {
        if (response.result != UnityWebRequest.Result.Success)
            bookError();
        else
            bookSuccess();
    }

    public void bookSuccess()
    {
        print("Nodes OK");
        // Button System
        
    }

    public void bookError()
    {
        print("Nodes Fail");
       
    }

    #endregion
}
