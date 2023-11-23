using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.Networking;
using static LoginSystem;
using static System.Net.WebRequestMethods;


namespace SAISDK
{
    public static class LoginSys 
    {
        public static string SessionKey;
        public static IEnumerator Login(string email, string password, Action<bool> callback)
        {
            Debug.Log("Loggin In");
            string url = ApiRoutes.Login;
            WWWForm form = new WWWForm();
            form.AddField("email", email);
            form.AddField("password", password);

            using (UnityWebRequest www = UnityWebRequest.Post(url, form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error: " + www.error);
                    callback(false); // Llama al callback con 'false' para indicar un fallo.
                }
                else
                {                 

                    // Extraemos la SessionKey del Usuario
                    JSONNode responseJson = JSON.Parse(www.downloadHandler.text);
                    SessionKey = responseJson["session_key"];                    
                    PlayerPrefs.SetString("sessionKey", SessionKey);
                    PlayerPrefs.SetString("csrfToken", responseJson["csrftoken"]);

                    // La solicitud fue exitosa
                    callback(true); // Llama al callback con 'true' para indicar éxito.
                }
            }
        }
        public static IEnumerator Register(string email,string username,string phone,Action<bool> callback, string city = "",string placeId = "")
        {
            string url = ApiRoutes.Register;

            WWWForm registrationData = new WWWForm();
            registrationData.AddField("email",email);
            registrationData.AddField("username",username);
            registrationData.AddField("phone",phone);
            registrationData.AddField("city",city);
            registrationData.AddField("placeid",placeId);

            using (UnityWebRequest www = UnityWebRequest.Post(url, registrationData))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Error: " + www.error);
                    callback(false); // Llama al callback con 'false' para indicar un fallo.
                }
                else
                {    
                    // La solicitud fue exitosa
                    callback(true); // Llama al callback con 'true' para indicar éxito.
                }
            }

        }


    }
}

