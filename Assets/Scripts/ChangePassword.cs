using System.Security.Cryptography;
using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using SAISDK;

public class ChangePassword : MonoBehaviour
{
    public TMP_InputField inputEmail;

    public void Register() {
        SceneManager.LoadScene(6);
    }

    public void LogIn() {
        SceneManager.LoadScene(0);
    }

    public void ChangePass(){
        //SceneManager.LoadScene(7);
        string email = inputEmail.text;
        StartCoroutine(LoginSys.ResetPassword(email, ChangePassCallback));
    }
    private void ChangePassCallback(bool response)
    {
        print("Registration Callback response : " + response);

        if (response)
        {
            print("response =" +response);
        }
        else
        {
            print("Changed Bad");
        }
    }

}