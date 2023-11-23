
using SimpleJSON;


using System.Collections.Generic;
using System.Text;
using UnityEngine;

using UnityEngine.UI;

using UnityEngine.Events;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine.Networking;
using System.Linq;
using Newtonsoft.Json;

public class DevicesHQ : MonoBehaviour
{

    public List<DeviceHQElement> Devices;

  

    #region PostWihoutToken

    
    public  void RegisterDevices()
    {
        RegisterDevicesUWR(Devices);
    }
   


    public void RegisterDevicesUWR(List<DeviceHQElement> devices)
    {
        string url = SAI.SDK.API.host + SAI.SDK.Devices.routeDevice;
        Debug.Log("URL DEVICES: " + url);

        List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();

        if(devices.Count == 0)
        {
           devices = FindObjectsOfType<DeviceHQElement>().ToList();
        }

        foreach (DeviceHQElement device in devices)
        {
            if (device.toggle.isOn)
            {
                Dictionary<string, object> requestData = new Dictionary<string, object>
                {
                    {"devicetype", device.deviceCode},
                    {"quantity", device.deviceQuantity}
                };

                dataList.Add(requestData);
            }
        }

        Debug.Log(dataList.Count + " DEVICES ADDED");

        if (dataList.Count == 0)
        {
            SuccessFunc();
            return;
        }

        //string jsonData = JsonUtility.ToJson(dataList,Formatting.Indented);
        //string temporal = "[{ \"device\":1,\"quantity\":1}]";
        string jsonData = JsonConvert.SerializeObject(dataList, Formatting.Indented);


        Debug.Log("JSON: " + jsonData);
        if (SAI.SDK.API.GenericPostRequest(url, jsonData) != string.Empty)
            SuccessFunc();
        else
            FailureFunc();
      

       // StartCoroutine(SendPostRequest(url, jsonData));
    }



    // Coroutine to send the POST request
    private IEnumerator SendPostRequest(string url, string jsonData)
    {



        byte[] array = Encoding.UTF8.GetBytes(jsonData);
        print("3 - Configurando Headers");
        UnityWebRequest request = UnityWebRequest.PostWwwForm(url, jsonData);
        request.uploadHandler = new UploadHandlerRaw(array);
        request.uploadHandler.contentType = "application/json";
       

        //request.SetRequestHeader("Content-Type", "application/json");

        request.SetRequestHeader("spaceaisess", SAI.SDK.Login.SessionKey);
        request.SetRequestHeader("csrfToken", SAI.SDK.Login.csrfToken);
        request.SetRequestHeader("X-CSRFToken", SAI.SDK.Login.csrfToken);

                 

        //print($"SE VA A ENVIAR LA REQUEST {request.uploadHandler.data} a {url}");

        yield return request.SendWebRequest();
        print("5 - SE ENVIO EL REQUEST, ESPERANDO RESPUESTA DEL SERVIDOR...");

        if (request.result == UnityWebRequest.Result.Success)
        {
            print("505 - RESPUESTA POSITIVA");
            //Debug.Log("Devices OK: " + request.downloadHandler.text);      
            SuccessFunc();
        }
        else
        {
            string handler = request.downloadHandler.text;
            print("6 - RESPUESTA NEGATIVA");
            Debug.Log("Devices Error: " + request.error);
            Debug.Log(handler);

            SAI.SDK.errorHandler.ShowPopup(handler,"ERROR");

            FailureFunc();
        }
        print("FIN DE LA FUNCION");
    }

    public void SuccessFunc()
    {
        print("DEVICES OK");
        
    }

    public void FailureFunc()
    {
        print("DEVICES FAIL");
        
    }



    #endregion

  


}











