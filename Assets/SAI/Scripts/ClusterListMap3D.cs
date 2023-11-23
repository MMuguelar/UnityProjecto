using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class ClusterListMap3D : MonoBehaviour
{
    public List<Cluster> clusterList = new();
    public List<NodeData> nodeList = new();
    public TMPro.TMP_Dropdown clusteListDropDown;
    public EarthMap earthMap;
    private void Start()
    {

        earthMap = GameObject.FindObjectOfType<EarthMap>();
        clusteListDropDown.onValueChanged.AddListener(SwitchCluster);
        clusterList = SAI.SDK.Clusters.GetClusterList();
        LoadDropdown();

        SAI.SDK.Login.SignIn("spaceai@spaceai.com", "spaceai");
            
    }



    
    public void LoadDropdown()
    {
        ListOfCLusters();
        nodesOnCluster();
    }


 
    private void ListOfCLusters()
    {
        earthMap.DestroyAllInstantiatedNodes();
        clusterList = SAI.SDK.Clusters.GetClusterList();

        clusteListDropDown.ClearOptions();

        List<string> opciones = new List<string>();

        //opciones.Add("Open Space Network");

        for (int i = 0; i < clusterList.Count; i++)
        {
            string opcion = "Name: " + clusterList[i].cluster_name;
            opciones.Add(opcion);
            
        }
        clusteListDropDown.AddOptions(opciones);
    }

    private void nodesOnCluster()
    {
        earthMap.DestroyAllInstantiatedNodes();

        nodeList = SAI.SDK.Nodes.GetNodeByID(clusteListDropDown.value);



        for (int i = 0; i < nodeList.Count; i++)
        {
            var node = nodeList[i];
            Debug.Log("Name: " + node.node_name);
            Debug.Log("Altitute: " + node.node_altitude);
            Debug.Log("Longitude: " + node.node_longitude);
            Debug.Log("Latitude: " + node.node_latitude);

            Vector3 positionNode = earthMap.LatLongToXYZ(node.node_latitude, node.node_longitude, 0);
            earthMap.InstantiateNodePrefab(positionNode);
        }
    }

    public void SwitchCluster(int value)
    {

        earthMap.DestroyAllInstantiatedNodes();
        nodeList = SAI.SDK.Nodes.GetNodeByID(value);
        if(nodeList.Count > 0) nodesOnCluster();
       
    }
}
