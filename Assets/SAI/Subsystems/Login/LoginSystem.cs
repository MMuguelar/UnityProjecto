using Newtonsoft.Json;
using SimpleJSON;


using System.Collections;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

/// <summary>
///  ABOUT : This class handles Login/Singup/SingIn/Registration
///  Last Update : 31/8/23
///  
///  USAGE :
///  Call SignIn Function to Login in
///  Call SignUp Function to Register
///  Call LogOut Function to Logout
///  Call ResetPassword Function to Reset Password via eMail
///  
/// </summary>


public class LoginSystem : MonoBehaviour
{
    
    public string SessionKey = "";
    public string csrfToken ="";

    [Header("API Endpoints")]
    
    public string routeSignIn = "/api/login/";
    public string routeSignUp = "/api/singup/";
    public string routeLogout = "/api/logout/";
    public string routeReset = "/api/reset/";


    #region Events
    public delegate void LoginSuccess();
    public static event LoginSuccess LoginSuccessTrigger;
    public delegate void LoginFailure();
    public static event LoginFailure LoginFailureTrigger;

    public delegate void LogoutSuccess();
    public static event LogoutSuccess LogoutSuccessTrigger;

    public delegate void RegistrationSuccess();
    public static event RegistrationSuccess RegistrationSuccessTrigger;
    public delegate void RegistrationFailure();
    public static event RegistrationFailure RegistrationFailureTrigger;

    

    public UnityEvent onLoginSuccess;
    public UnityEvent onLoginFailure;
    public UnityEvent onLogoutSuccess;
    public UnityEvent onLogoutFailure;
    public UnityEvent onRegistrationSuccess;
    public UnityEvent onRegistrationFailure;
    public UnityEvent onPasswordResetSuccess;
    public UnityEvent onPasswordResetFailure;


 

    #endregion

    #region Public Functions


    
    public void SignIn(string email, string username) { StartCoroutine(co_SignIn(email, username)); }
    
    public void SignUp(string email, string username, string phone, string city, string placeId)

    {
        // StartCoroutine(co_SignUp(email, username, phone,city,placeId)); 
        string url = SAI.SDK.API.host + routeSignUp;
        RegistrationData registrationData = new();
        registrationData.email = email;
        registrationData.username = username;
        registrationData.phone = phone;
        registrationData.city = city;
        registrationData.placeid = placeId;

        string json = JsonConvert.SerializeObject(registrationData, Formatting.Indented);
        print($"Este es el json ya serializado : {json}");

        string response  = SAI.SDK.API.GenericPostRequest(url, json);
        if(response != string.Empty)
        {
            print("Registration Success : " + response);
            RegistrationSuccessTrigger();
            onRegistrationSuccess.Invoke();
        }
        else
        {            
            RegistrationFailureTrigger();
            onRegistrationFailure.Invoke();
        }
    }
    
    public void SignOut() { StartCoroutine(co_LogOut()); }
    
    public void ResetPassword(string email) { StartCoroutine(co_ResetPassword(email)); }




    
    #endregion

    #region Private Functions
    IEnumerator co_SignIn(string email, string password)
    {
        string url = SAI.SDK.API.host + SAI.SDK.Login.routeSignIn;
        print("Llamado a la funcion");
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);


        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {

            yield return www.SendWebRequest();


            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Login Failed : " + www.error);
                SAI.SDK.errorHandler.ShowPopup(www.downloadHandler.text,"ERROR");
                LoginFailureTrigger();
                onLoginFailure.Invoke();
            }
            else
            {
                print("Login OK : " + www.downloadHandler.text);

                SAI.SDK.Login.csrfToken = www.GetResponseHeader("X-CSRFToken");
                print("LOGIN : ESTOY GENERANDO TOKEN : " + www.GetResponseHeader("X-CSRFToken"));


                // Parse the JSON response
                JSONNode responseJson = JSON.Parse(www.downloadHandler.text);

                SessionKey = responseJson["session_key"];

                // Extract session_key and csrftoken
                PlayerPrefs.SetString("sessionKey", SessionKey);
                PlayerPrefs.SetString("csrfToken", responseJson["csrftoken"]);

                print("SESSION KEY : " + SessionKey);
                print("csrfTOKEN : " + responseJson["csrftoken"]);

                SAI.SDK.Login.SessionKey = SessionKey;
                SAI.SDK.Login.csrfToken = responseJson["csrftoken"];

                SAI.SDK.debuger.sessionKey = SessionKey;
                SAI.SDK.debuger.csrfToken = responseJson["csrftoken"];


                LoginSuccessTrigger();
                onLoginSuccess.Invoke();
            }
        }
    }   
    IEnumerator co_LogOut()
    {
        string url = SAI.SDK.API.host + SAI.SDK.Login.routeLogout;


        WWWForm form = new WWWForm();

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", "Bearer " + SAI.SDK.Login.csrfToken);
            www.SetRequestHeader("X-CSRFToken", SAI.SDK.Login.csrfToken);
            www.SetRequestHeader("csrftoken", SAI.SDK.Login.csrfToken);
            www.SetRequestHeader("spaceaisess", SAI.SDK.Login.SessionKey);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Logout Failed : " + www.error);
                onLogoutFailure.Invoke();
            }
            else
            {
                print("Logout OK : " + www.downloadHandler.text);
                onLogoutSuccess.Invoke();
            }
        }
    }
    IEnumerator co_ResetPassword(string email)
    {
        string url = SAI.SDK.API.host + SAI.SDK.Login.routeReset;

        WWWForm form = new WWWForm();
        form.AddField("email", email);

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Registration Failed : " + www.error);
                onPasswordResetFailure.Invoke();
            }
            else
            {
                print("Registration OK : " + www.downloadHandler.text);
                onPasswordResetSuccess.Invoke();
            }
        }
    }




    #endregion

    [System.Serializable]
    public class RegistrationData
    {
        public string email;
        public string username;
        public string phone;
        public string city;
        public string placeid;
    }


}
