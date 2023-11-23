using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RememberMeScript : MonoBehaviour
{
    public TMP_InputField usernameInputField;
    public TMP_InputField passwordInputField;
    public Toggle rememberMeToggle;

    private void Start()
    {
        print($"START OF: RememberMeScript.cs \n");
        // Cargar datos almacenados si la opción "Recordar" está activada
        if (PlayerPrefs.HasKey("RememberMe") && PlayerPrefs.GetInt("RememberMe") == 1)
        {
            print($"RememberMe Toggle is : ON \n ");
            LoadNewUserData();
            //LoadSavedUserData();
        }
        else
        {
            print($"RememberMe Toggle is : OFF \n ");
        }
    }

    private void LoadNewUserData()
    {
        print("Loading User Session Key");
        if (PlayerPrefs.HasKey("SessionKey"))
        {
            
            SAI.SDK.Login.SessionKey = PlayerPrefs.GetString("SessionKey");
            SAI.SDK.Login.csrfToken = PlayerPrefs.GetString("csrfToken");

            print($"Found Session Key : {SAI.SDK.Login.SessionKey} \n Found csrfToken : {SAI.SDK.Login.csrfToken} \n" +
                $"Checking if Session Key is Valid with Fake Request");

            string url = SAI.SDK.Wallet.GetWalletEndpoint;
            string response = SAI.SDK.API.GenericGetRequest(url); 
            if(response != string.Empty)
            {
                print("Session Key is VALID \n moving to Home. \n SIDENOTE: Should we load data here?");
                

                print($"Setting Up User Data");
                JSONNode responseJson = JSON.Parse(response);
                SAI.SDK.Wallet.userName = responseJson["username"];
                SAI.SDK.Wallet.userCredit = responseJson["total"];

               
                print("Moving to Desktop Scene");
                SceneManager.LoadScene("Desktop Template");
            }
            else
            {
                print("Session Key is NOT valid. \n returning.");
                PlayerPrefs.DeleteKey("SessionKey");
                PlayerPrefs.SetInt("RememberMe", 0);
            }
        }

    }


    public void Login()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;
      
        if (rememberMeToggle.isOn)
        {
            SaveUserData(username, password);
        }
        else
        {
            
            PlayerPrefs.DeleteKey("Username");
            PlayerPrefs.DeleteKey("Password");
            PlayerPrefs.DeleteKey("RememberMe");
        }

    
    }

    private void SaveUserData(string username, string password)
    {
        PlayerPrefs.SetString("Username", username);
        PlayerPrefs.SetString("Password", password);
        PlayerPrefs.SetInt("RememberMe", 1);
    }

    private void LoadSavedUserData()
    {
        if (PlayerPrefs.HasKey("Username"))
        {
            string savedUsername = PlayerPrefs.GetString("Username");
            usernameInputField.text = savedUsername;
        }

        if (PlayerPrefs.HasKey("Password"))
        {
            string savedPassword = PlayerPrefs.GetString("Password");
            passwordInputField.text = savedPassword;
        }

        rememberMeToggle.isOn = true; 
    }
}
