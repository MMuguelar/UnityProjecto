using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Events;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using UnityEngine.Networking;

public class RegistrationLogin : MonoBehaviour
{

    public TMP_InputField emailField;
    public TMP_InputField usernameField;
    public TMP_InputField phoneNumber;
    public TMP_InputField cityField;
    public TMP_InputField countryCode;
    public TMP_Dropdown dropdown;

    public UnityEvent LoginSuccessEvent;
    public UnityEvent LoginFailEvent;

    public UnityEvent SignUpSuccessEvent;
    public UnityEvent SignUpFailEvent;

    public Button LogInButton;




    private void Start()
    {
        SAI.SDK.logger.LogToFile("User at Registration Scene", "Log.txt");
    }

    public void Login()
    {
        SAI.SDK.Login.SignIn(emailField.text.ToLower(), "NewPassword2547");

        SAI.SDK.logger.LogToFile("User is trying to Log in", "Log.txt");
    }

    public void Logout()
    {

        SAI.SDK.Login.SignOut();
        /*
        //Deslogeamos

        string url = SAI.subsystems.api.rLogout;
        logoutdata data = new();
        string jsondata = JsonConvert.SerializeObject(data, Formatting.Indented);
        SAI.subsystems.api.SendJsonPostRequest(url,jsondata,logoutCallback);
        */
    }

    public void logoutCallback(UnityWebRequest response)
    {
        if(response.result != UnityWebRequest.Result.Success)
        {
            print("Logout Error");
        }
        else
        {
            print("Logout Success");
        }
    }
    public void ConnectToAPI()
    {

        SAI.SDK.logger.LogToFile("Conecting to API", "Log.txt");

        string phone = countryCode.text + phoneNumber.text;
    

        if (PlayerPrefs.HasKey("phone"))
        {
            phone = PlayerPrefs.GetString("phone");
           
        }

        string city = string.Empty;
        if(dropdown.itemText.text != string.Empty) { city = dropdown.itemText.text; } else { city = cityField.text; };
        



        print($"Fix this : email={emailField.text.ToLower()} username={usernameField.text} phone={phone} city={city} placeid={PlayerPrefs.GetString("PlaceID")} )");

        SAI.SDK.Login.SignUp(emailField.text.ToLower(), usernameField.text, phone, city, PlayerPrefs.GetString("PlaceID"));
    }

    public void goBackToLogin(string loginScene)
    {
        SceneManager.LoadScene(loginScene);
    }

    public void RegistrarNodos()
    {
        SAI.SDK.register_nodes.RegisterNodes();
    }

    public void RegistrarDevices()
    {
        SAI.SDK.register_devices.RegisterDevices();
    }


    private void OnEnable()
    {
        LoginSystem.LoginSuccessTrigger += LoginOK;
        LoginSystem.LoginFailureTrigger += LoginBAD;

        LoginSystem.RegistrationSuccessTrigger += RegOK;
        LoginSystem.RegistrationFailureTrigger += RegFail;
    }

    private void RegFail()
    {
       
        SignUpFailEvent.Invoke();
    }

    private void RegOK()
    {
       
        SignUpSuccessEvent.Invoke();
    }

    private void LoginBAD()
    {
       
        LoginFailEvent.Invoke();
    }

    private void LoginOK()
    {
       
        LoginSuccessEvent.Invoke();

    }

    private void OnDisable()
    {
        LoginSystem.LoginSuccessTrigger -= LoginOK;
        LoginSystem.LoginFailureTrigger -= LoginBAD;

        LoginSystem.RegistrationSuccessTrigger -= RegOK;
        LoginSystem.RegistrationFailureTrigger -= RegFail;
    }


    [System.Serializable]
    public class logoutdata
    {

    }



}
