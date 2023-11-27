using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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

       // if(inputUserName != "" || )
        if (inputPassword.text == inputRepeatPassword.text){
            ExampleRegistration(inputEmail.text, inputUserName.text);
        }
    }

    public void ExampleRegistration(string email,string username,string phone = "",string city ="",string placeID ="")
    {
        StartCoroutine(LoginSys.Register(email, username, phone, RegistrationCallback,city,placeID));
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
            else if (inputField != null && string.IsNullOrWhiteSpace(inputField.text))
            {
                print("complete " + inputField + " field");
                inputsValid = false;
                break; // Exit the loop early if any input is invalid
            }
        }

        submitButton.interactable = inputsValid;
    }

    private bool IsEmailValid(string email)
    {
        // Regex pattern to validate email addresses
        string pattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$";

        Regex regex = new Regex(pattern);
        return regex.IsMatch(email);
    }
}