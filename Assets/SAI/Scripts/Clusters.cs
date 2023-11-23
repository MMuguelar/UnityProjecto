using Newtonsoft.Json;

using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Clusters : MonoBehaviour
{
    [Header("Endpoints")]
    public string GetClusterListEndpoint = "/api/getuserclusters/";
    public string GetClusterTransactionListEndpoint = "/api/getclustertransactionslist/";
    public string PostAddCluster = "/api/cluster/";

    [Header("User Clusters")]
    private List<Cluster> clusterList = new List<Cluster>();  

    public List<Cluster> GetClusterList()
    {
       
        string url = SAI.SDK.API.host + GetClusterListEndpoint;
        string jsonResponse = SAI.SDK.API.GenericGetRequest(url);
        ClusterResponse response = JsonUtility.FromJson<ClusterResponse>(jsonResponse);

        if (response != null)
        {
            return response.clusters;
        }
        else
        {
            Debug.LogError("Error al analizar los datos JSON.");
            return new List<Cluster>();
        }
           
       
    }
    

    
    public void GetClusterListExample()
    {
        clusterList = GetClusterList();
    }

    public bool AddCluster(string clusterName,string ClusterDescription)
    {
        string addClusterName = Regex.Replace(clusterName, @"[^\x20-\x7E]", "");
        string addClusterDescription = Regex.Replace(ClusterDescription, @"[^\x20-\x7E]", "");

        var requestData = new
        {
            name = addClusterName,
            description = addClusterDescription
        };

        string jsonConfig = JsonConvert.SerializeObject(requestData);
        string url = SAI.SDK.API.host + PostAddCluster;
        string respose = SAI.SDK.API.GenericPostRequest(url, jsonConfig);
        if (respose != string.Empty)
            return true;
        else return false;   

    }

   
}

[System.Serializable]
public class Cluster
{
    public string cluster_name = "";
    public string nodes_count = "";
    public int nodes_data;
    public string permissiontype_description = "";
    public string owner_username = "";
    public string cluster_mac = "";
    public List<node_single> cluster_nodes;
}

[System.Serializable]
public class ClusterInfoRequest
{
    public string cluster_mac;
}

[System.Serializable]
public class ClustersList
{
    public List<Cluster> clusters;
}

[System.Serializable]
public class UserClusterWithNodes
{
    public Cluster clusterData;
    public List<node_single> clusterNodes;
}

[System.Serializable]
public class ClusterResponse
{
    public List<Cluster> clusters;
}
