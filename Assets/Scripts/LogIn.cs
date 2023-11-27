using System.Security.Cryptography;
using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using SAISDK;

public class Login : MonoBehaviour
{
    public TMP_InputField inputUserName;
    public TMP_InputField inputPassword;

    // Start is called before the first frame update
    void Start()
    {
        //SAI.SDK.Login.SignOut();
        inputPassword.contentType = TMP_InputField.ContentType.Password;
        //SAI.SDK.Login.SignUp("campo@s.com","pedro","+541168983444", "cs", "p");
    }
    // Update is called once per frame
    private void Update() {
        print("username: " +inputUserName.text);
        print("Password: " +inputPassword.text);
    }

    public void Register() {
        SceneManager.LoadScene(6);
    }


    public void submit()
    {
        ExampleLogin(inputUserName.text, inputPassword.text);
    }

    private void ExampleLogin(string email,string password)
    {
        StartCoroutine(LoginSys.Login(email, password, LoginCallback));
    }

    private void ExampleLogOut(){
        StartCoroutine(LoginSys.Logout(LogoutCallBack));
    }
    private void LogoutCallBack(bool response){

        print("Logout Callback response : " + response);

        if(response)
        {
            print("Logout OK");
        }
        else
        {
            print("Logout Bad");
        }
    }
    private void LoginCallback(bool response)
    {
        print("Login Callback response : " + response);

        if(response)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            print("Login Bad");
        }
    }

    public void ExampleRegistration(string email,string username,string phone,string city ="",string placeID ="")
    {
        StartCoroutine(LoginSys.Register(email, username, phone, RegistrationCallback,city,placeID));
    }

    private void RegistrationCallback(bool response)
    {
        print("Registration Callback response : " + response);

        if (response)
        {
            print("Registration OK");
        }
        else
        {
            print("Registration Bad");
        }
    }


}