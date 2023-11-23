using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LogInPage : MonoBehaviour
{

    //[SerializeField] TMP_InputField EmailLoginInput;

    [Header("Home")]
    [SerializeField] GameObject HomeScreen;
    [Header("BookNIO")]
    [SerializeField] GameObject BookNIOScreen;
    [Header("CreateAccount")]
    [SerializeField] GameObject CreatePersonalAccountScreen;
    [Header("UserVerification")]
    [SerializeField] GameObject UserVerificationScreen;
    [Header("NameYourCluster")]
    [SerializeField] GameObject NameYourClusterScreen;

    [Header("Create Personal Account UI")]
    [SerializeField] TMP_InputField EmailInput;
    [SerializeField] TMP_InputField CountryInput;

    [Header("User Verification UI")]
    [SerializeField] TMP_InputField VerificationCodeInput;

    [Header("Name Your Cluster UI")]
    [SerializeField] TMP_InputField ClusterNameInput;
    [SerializeField] TMP_InputField NodeNameInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OpenBookNIOScreen()
    {
        BookNIOScreen.SetActive(true);
        HomeScreen.SetActive(false);
        CreatePersonalAccountScreen.SetActive(false);
        UserVerificationScreen.SetActive(false);
        NameYourClusterScreen.SetActive(false);
    }
    public void OpenHomeScreen()
    {
        BookNIOScreen.SetActive(false);
        HomeScreen.SetActive(true);
        CreatePersonalAccountScreen.SetActive(false);
        UserVerificationScreen.SetActive(false);
        NameYourClusterScreen.SetActive(false);
    }
    public void OpenCreatePersonalAccountScreen()
    {
        BookNIOScreen.SetActive(false);
        HomeScreen.SetActive(false);
        CreatePersonalAccountScreen.SetActive(true);
        UserVerificationScreen.SetActive(false);
        NameYourClusterScreen.SetActive(false);
    }

    public void SavePersonalAccount()
    {
        //Get Email and Country data (DEBUG)

        BookNIOScreen.SetActive(false);
        HomeScreen.SetActive(false);
        CreatePersonalAccountScreen.SetActive(false);
        UserVerificationScreen.SetActive(true);
        NameYourClusterScreen.SetActive(false);

        Debug.Log("Email: " + EmailInput.text + ", Country: " + CountryInput.text);
    }
    public void UserVerification()
    {
        //Get Code (DEBUG)

        BookNIOScreen.SetActive(false);
        HomeScreen.SetActive(false);
        CreatePersonalAccountScreen.SetActive(false);
        UserVerificationScreen.SetActive(false);
        NameYourClusterScreen.SetActive(true);

        Debug.Log("Verification Code: " + VerificationCodeInput.text);
    }

    public void SaveClusterAndNodeName()
    {
        //Get Names (DEBUG)
        Debug.Log("Cluster Name: " + ClusterNameInput.text + ", Node Name: " + NodeNameInput.text);
    }

}
