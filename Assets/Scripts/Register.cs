using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using System.Linq;
using SAISDK;

public class Register : MonoBehaviour
{
    public Button submitButton;
    public TMP_InputField inputEmail;
    public TMP_InputField inputUserName;
    public TMP_InputField inputPassword;
    public TMP_InputField inputRepeatPassword;
    private List<TMP_InputField> inputFields = new List<TMP_InputField>();

    // Start is called before the first frame update
    void Start()
    {
        inputPassword.contentType = TMP_InputField.ContentType.Password;
        inputRepeatPassword.contentType = TMP_InputField.ContentType.Password;

        inputFields.Add(inputEmail);
        inputFields.Add(inputUserName);
        inputFields.Add(inputPassword);
        inputFields.Add(inputRepeatPassword);
        if (submitButton != null)
        {
            submitButton.interactable = false;
        }
    }
    // Update is called once per frame
    private void Update() {

        ValidateInput();  
    }

    public void submit()
    {
        if (inputPassword.text == inputRepeatPassword.text){
            ExampleRegistration(inputEmail.text, inputUserName.text, inputPassword.text);
        }else{
            EditorUtility.DisplayDialog("Password:",
            "The passwords don't match.", "Ok");
        }
    }

    public void ExampleRegistration(string email,string username,string password, string phone = "+541168684882",string city ="",string placeID ="")
    {
        StartCoroutine(LoginSys.Register(email, username, password, phone, RegistrationCallback,city,placeID));
    }

    private void RegistrationCallback(bool response)
    {
        print("Registration Callback response : " + response);

        if (response)
        {
            SceneManager.LoadScene(5);
        }
        else
        {
            print("Registration Bad");
        }
    }

    public void ValidateInput()
    {
        
       bool inputsValid = true;

        foreach (TMP_InputField inputField in inputFields)
        {
            if (inputField != null && string.IsNullOrWhiteSpace(inputField.text))
            {
                    print("complete " + inputField + " field");
                    inputsValid = false;
                    break; // Exit the loop early if any input is invalid
                
                if (inputField == inputEmail)
                {
                    // Validate email using regex pattern
                    if (inputField != null && string.IsNullOrWhiteSpace(inputField.text) || !IsEmailValid(inputField.text))
                    {
                        print("invalid email");
                        inputsValid = false;
                        break; // Exit the loop early if email is invalid
                    }
                }
                if(inputField== inputPassword || inputField == inputRepeatPassword){
                    if (inputField != null && string.IsNullOrWhiteSpace(inputField.text) || !IsPasswordValid(inputField.text))
                    {
                        print("invalid " + inputField + " field");
                        break; // Exit the loop early if email is invalid
                    }
                }
            }
        }

        submitButton.interactable = inputsValid;
    }
    public void LogIn() {
        SceneManager.LoadScene(0);
    }
    private bool IsEmailValid(string email)
    {
        // Regex pattern to validate email addresses
        string pattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$";

        Regex regex = new Regex(pattern);
        return regex.IsMatch(email);
    }

    private bool IsPasswordValid(string password){
        print(password);
        string specialCharacters = "!@#$%^&*(),.?\":{}|<>";
        if ((password.Length < 8)){

            EditorUtility.DisplayDialog("Error:",
            "The password must be a minimum of 8 characters.", "Ok");

            return false;
        }if((!password.Any(char.IsUpper)) || (!password.Any(char.IsLower))){

            EditorUtility.DisplayDialog("Error:",
            "Password must contain both uppercase and lowercase characters.", "Ok");

            return false;
        }if((password.All(char.IsDigit))){

            EditorUtility.DisplayDialog("Error:",
            "Password must contain at least one digit.", "Ok");

            return false;
        }if((!password.Any(c => specialCharacters.Contains(c)))){

            EditorUtility.DisplayDialog("Error:",
            "Password must contain at least one special character (!@#$%^&*(),.?\":{}|<>)", "Ok");

            return false;
        }
        return true;
    }
}