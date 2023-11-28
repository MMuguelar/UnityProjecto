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
    public TMP_InputField inputEmail;
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
        print("username: " +inputEmail.text);
        print("Password: " +inputPassword.text);
    }

    public void Register() {
        SceneManager.LoadScene(6);
    }

    public void submit()
    {
        ExampleLogin(inputEmail.text, inputPassword.text);
    }

    private void ExampleLogin(string email,string password)
    {
        StartCoroutine(LoginSys.Login(email, password, LoginCallback));
    }

    private void LoginCallback(bool response)
    {
        print("Login Callback response : " + response);

        if(response)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            print("Login Bad");
        }
    }

    public void ForgotPassword(){
        SceneManager.LoadScene(7);
    }
}