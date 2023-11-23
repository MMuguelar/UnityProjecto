using Newtonsoft.Json;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class AddNewDevice : MonoBehaviour
{

    public TMP_Dropdown dropdownCluster;
    public TMP_Dropdown dropdownDeviceType;
    public TMP_Text devicename;

    private List<Cluster> clusterList;

 
    public void AddNewDeviceButton()
    {
        string deviceName = devicename.text;
        string deviceMac = SAI.SDK.Devices.currentDeviceMAC;
        clusterList = SAI.SDK.Clusters.GetClusterList();
        string latitude = SAI.SDK.locationManager.latitude;
        string longitude = SAI.SDK.locationManager.longitude;
        string so_system = SAI.SDK.Devices.GetDeviceSO();



        if (SAI.SDK.Nodes.Add_New_Device(deviceName, deviceMac, clusterList[dropdownCluster.value].cluster_mac, dropdownDeviceType.value, latitude, longitude))
        {
            SceneManager.LoadScene("Desktop Template");
        }
        else
        {
            Debug.LogError("Error Adding New Device");
            SceneManager.LoadScene("Desktop Template");
        }
    }

  

   

  
}


