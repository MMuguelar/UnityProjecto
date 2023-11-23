using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SAISDK;
using System;

public class Examples : MonoBehaviour
{

    private void Start()
    {      
        //ExampleLogin("spaceai@spaceai.com", "spaceai");
        
        //ExampleRegistration("TestingSDK@spaceai.com", "TestingSDK", "3489323274");
    }

    public void ExampleLogin(string email,string password)
    {
        StartCoroutine(LoginSys.Login(email, password, LoginCallback));
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

    private void LoginCallback(bool response)
    {
        print("Login Callback response : " + response);

        if(response)
        {
            print("Login OK");
        }
        else
        {
            print("Login Bad");
        }
    }

   
}
