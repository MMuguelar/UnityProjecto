
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Devices : MonoBehaviour
{
    [Header("API Endpoints")]
    public string routeDevice = "/api/device/";
    public string getValidateNodeExists = "/api/node/validateNodeExists";

    [Header("Current Device Properties")]

    public string currentDeviceName;
    public string currentDeviceModel;
    public string currentDeviceType;
    public string currentDeviceSO;
    public string currentDeviceMAC;
    public string currentDevinceID;


    private void Start()
    {
        GetPCData();
        
    }

    private void GetMobileData()
    {
        currentDeviceName = GetDeviceName();
        currentDeviceModel = GetDeviceModel();
        currentDeviceType = GetDeviceType();
        currentDeviceSO = GetDeviceSO();
        currentDeviceMAC = GetIMAIData();
        currentDevinceID = GetDeviceID();
        SAI.SDK.errorHandler.ShowPopup(currentDevinceID, "ID");
    }

    private void GetPCData()
    {
        currentDeviceName = GetDeviceName();
        currentDeviceModel = GetDeviceModel();
        currentDeviceType = GetDeviceType();
        currentDeviceSO = GetDeviceSO();
        currentDeviceMAC = GetDeviceMac();
        currentDevinceID = GetDeviceID();
    }
    public string GetIMAIData()
    {
        print("IMEI DATA NOT DONE YET");
        return null;
    }

    public string GetDeviceID()
    {
        return SystemInfo.deviceUniqueIdentifier;
    }
    public string GetDeviceMac()
    {
        string macAddress = "";
        NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

        foreach (NetworkInterface networkInterface in networkInterfaces)
        {
            if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                networkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
            {
                macAddress = networkInterface.GetPhysicalAddress().ToString();
                break;
            }
        }

        return macAddress;
    }
    public string GetDeviceSO()
    {
        return SystemInfo.operatingSystem;
    }

    public string GetDeviceName()
    {
        return SystemInfo.deviceName;
    }

    public string GetDeviceModel()
    {
        return SystemInfo.deviceModel;
    }

    public string GetDeviceType()
    {
        return SystemInfo.deviceType.ToString();
    }

    
    public bool IsNewDevice()
    {
        print("Checking current Device *NEW SYSTEM*");

        print("HOST : " + SAI.SDK.API.host + "RUTA : " + getValidateNodeExists);
        string url = SAI.SDK.API.host + getValidateNodeExists;
#if PLATFORM_ANDROID
        string parameters = ("?node_mac=" + GetDeviceID());
#else
 string parameters = ("?node_mac=" + currentDeviceMAC);  
#endif


        string response = SAI.SDK.API.GenericGetRequest(url + parameters);

        if(response != "")
        {
            print("False _ Is new Device porque la response fue : " + response);
            return false;
        }
        else
        {
            print("true _ Is Old Device por que la response fue : " + response);
            return true;
        }
           

    }



   
}

[System.Serializable]
public class newDevice
{
    public string mac;
    public string name;
    public string op_system;
    public string nodetype;
    public string cluster;
    public string altitude;
    public string longitude;
    public string latitude;

}