using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Events;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using Newtonsoft.Json;
using SimpleJSON;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LogInLogic : MonoBehaviour
{
    [Header("Required")]
    public TMP_InputField usernameField;
    public TMP_InputField passwordField;
    public TMP_Text errorMessage;

    [Header("OnFinishEvents")]
    public UnityEvent LoginSuccessEvent;
    public UnityEvent LoginFailEvent;

    public GameObject DevicesceneGO;
    public GameObject LoginsceneGO;
    public CanvasGroup DeviceSceneGO;
    public CanvasGroup LoginSceneGO;


    private void Start()
    {
       
    }




    private void OnEnable()
    {
        LoginSystem.LoginSuccessTrigger += MyFunction;
    }

    public void MyFunction()
    {

    }
    public void Login()
    {
        

        print("Login In");
        //SAI.subsystems.logger.LogToFile("User is loggin", "Log.txt");

   
        string url = SAI.SDK.API.host+SAI.SDK.Login.routeSignIn;
        print("LA URL : " + url);


        LoginData loginData = new LoginData();
        loginData.email = usernameField.text.ToLower();
        loginData.password = passwordField.text;

        string jsonData = JsonConvert.SerializeObject(loginData,Formatting.Indented);
        print("LoginLogic.cs : Logeando " + url + "//" + jsonData);
        string response = SAI.SDK.API.GenericPostRequest(url, jsonData);
        print("LoginLogic.cs : Response : " + response);
        if(response != string.Empty)
        {
            JSONNode responseJson = JSON.Parse(response);

            // Extract session_key and csrftoken
            PlayerPrefs.SetString("sessionKey", responseJson["session_key"]);
            PlayerPrefs.SetString("csrfToken", responseJson["csrftoken"]);

            print("SESSION KEY : " + responseJson["session_key"]);
            print("csrfTOKEN : " + responseJson["csrftoken"]);

            SAI.SDK.Login.SessionKey = responseJson["session_key"];
            SAI.SDK.Login.csrfToken = responseJson["csrftoken"];

            SAI.SDK.debuger.sessionKey = responseJson["session_key"];
            SAI.SDK.debuger.csrfToken = responseJson["csrftoken"];






            if (PlayerPrefs.HasKey("RememberMe"))
            {
                if (PlayerPrefs.GetInt("RememberMe") == 1)
                {
                    PlayerPrefs.SetString("SessionKey", SAI.SDK.Login.SessionKey);
                }
                else
                {
                    PlayerPrefs.SetString("SessionKey", "");
                }
            }

            LoginOK();
        }
        else
        {
            print("LoginLogic.cs : Fallo al logear. Despachando evento.");
            LoginBAD();
        }
    }

    public void GoToRegisterScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        LogoutCheck();
    }

    public void LogoutCheck()
    {
        if (SAI.SDK.Login.SessionKey != string.Empty) SAI.SDK.Login.SignOut();
    }


    public void UpdateClusterList()
    {
        //  SAI.subsystems.userInfo.GetClusters();
        print("Update cluster removed");
    }

    




    private void LoginBAD()
    {
        
        LoginFailEvent.Invoke();

    }

    private void LoginOK()
    {
        print("LogInLogin : LoginOk");
        if (SAI.SDK.Devices.IsNewDevice())
        {
            print("LogInLogin : New Device Found!");
            DeviceSceneGO.alpha = 1;
            LoginSceneGO.alpha = 0;
            DevicesceneGO.SetActive(true);
            LoginsceneGO.SetActive(false);
#if PLATFORM_ANDROID
            SAI.SDK.locationManager.GpsLocation.GetGPSLocation();
#endif
        }
        else
        {
            print("LogInLogin : Known Device Found!");
            SceneManager.LoadScene("Desktop Template");
        }
        //LoginSuccessEvent.Invoke();

    }

    /*
    private void CheckDevice()
    {
        print("Checking current Device");
        string url = SAI.subsystems.api.rValidateNodeExists;
        string parameters = ("?node_mac=" + SAI.subsystems.userInfo.devices.currentDeviceMAC);
        SAI.subsystems.api.SendGetRequest(url + parameters, CheckDeviceCallback);
    }

    private void CheckDeviceCallback(UnityWebRequest responseData)
    {
        string response = responseData.downloadHandler.text;
        print("CheckDeviceCallback Response = "+response);

        if(responseData.result != UnityWebRequest.Result.Success)
        {
            print($"**** NEW DEVICE DETECTED");
            DeviceSceneGO.alpha = 1;
            LoginSceneGO.alpha = 0;
            DevicesceneGO.SetActive(true);
            LoginsceneGO.SetActive(false);
        }
        else
        {
            print($"DEVICE RECOGNIZED MOVING NEXT");
            SceneManager.LoadScene("Desktop Template");
        }

    }
    */



    [System.Serializable]
    public class LoginData
    {
        public string email;
        public string password;

    }

}
