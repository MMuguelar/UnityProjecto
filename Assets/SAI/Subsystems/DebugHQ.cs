
using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Debug Class for Internal Usage and Examples of SAI Subsystems
/// </summary>

public class DebugHQ : MonoBehaviour
{

    [Header("Debug")]
    public string debugEmail = "Carlos@gmail.com";
    public string debugPassword = "NewPassword2547";
    public string debugUsername = "Tester";
    public string debugPhone = "+543489545451";
    public string debugCity = "Longchamps";
    public string debugPlaceID = "xxxxxxxxxx";

    public string sessionKey = "";
    public string csrfToken = "";

 

    




    
    public void Login()
    {
        SAI.SDK.Login.SignIn(debugEmail, debugPassword);
    }
    
    public void CreateAccount()
    {
        SAI.SDK.Login.SignUp(debugEmail, debugUsername, debugPhone, debugCity, debugPlaceID);
    }
    
    public void RefreshTokens()
    {
        sessionKey = PlayerPrefs.GetString("sessionKey");
        csrfToken = PlayerPrefs.GetString("csrfToken");

    }

    
    public void Logout()
    {
        SAI.SDK.Login.SignOut();
    }




    
    public void RegisterDevices()
    {
        SAI.SDK.register_devices.RegisterDevices();
    }

    
    public void RegisterNodes()
    {
        SAI.SDK.register_nodes.RegisterNodes();
    }

    
    public void StartUDPServer(int port)
    {
        SAI.SDK.UDP.StartReceiver(port);
    }

    
    public void StopUDPServer()
    {
        SAI.SDK.UDP.StopReceiver();
    }

    
    public void SendUDP(string ip,int port,string msg)
    {
        SAI.SDK.UDP.SendUDPMessage(ip,port,msg);
    }



    // Remove Byte Order Mark (BOM) from JSON data
    string CleanJsonFromBOM(string json)
    {
        if (json.Length > 0 && json[0] == '\ufeff')
        {
            json = json.Remove(0, 1);
        }
        return json;
    }


    
    public void PopUp(string mensaje,string titulo)
    {
        print("A");
        SAI.SDK.errorHandler.ShowPopup(mensaje, titulo);
    }
  



    
}