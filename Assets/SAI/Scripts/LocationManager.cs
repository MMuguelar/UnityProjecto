using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.IO;
using System.Collections.Generic;




public class LocationManager : MonoBehaviour
{
    public TMP_Text locationText;


    public string latitude;
    public string longitude;

    public GPSLocation GpsLocation;

    [System.Serializable]
    private class IP
    {
        public string ip;
    }

    // Data structure to represent the JSON response from the IP geolocation service
    [System.Serializable]
    private class IPInfoData
    {
        public string IP;
        public string city;
        public string loc;
    }

    // Data structure to represent the JSON format for IP history
    [System.Serializable]
    private class IPHistoryData
    {
        public List<IPInfoData> IpHistory;
    }

    private class City
    {
        public string city;
    }

    private IPHistoryData ipHistoryData;

    private void Start()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
         // Load the existing IP history data from the file
        LoadIPHistoryData();

        // Fetch the public IP address using ipify API
        FetchPublicIP();
#elif UNITY_ANDROID
        GetLocationAndroid();

#else
        filePath = Path.Combine(Application.dataPath, "JSON", fileName);
#endif



    }


    private void GetLocationAndroid()
    {
        print("TODO : Call GPSLocation");
    }

    private void LoadIPHistoryData()
    {
        // Define the file name and file path
        string fileName = "JSONIPHistory.json";
        string filePath;

#if UNITY_EDITOR || UNITY_STANDALONE
        filePath = Path.Combine(Application.persistentDataPath, fileName);
#elif UNITY_ANDROID
    filePath = Path.Combine(Application.persistentDataPath, fileName);
#elif UNITY_IOS || UNITY_IPHONE || UNITY_WEBGL
    filePath = Path.Combine(Application.persistentDataPath, "JSON", fileName);
#else
    filePath = Path.Combine(Application.dataPath, "JSON", fileName);
#endif

        // Check if JSON file exists
        if (File.Exists(filePath))
        {
            string jsonContent = File.ReadAllText(filePath);

            // Check if the file is empty or contains valid JSON data
            if (!string.IsNullOrEmpty(jsonContent))
            {
                ipHistoryData = JsonUtility.FromJson<IPHistoryData>(jsonContent);
            }
            else
            {
                // The file exists but is empty, initialize a new IP history data object
                ipHistoryData = new IPHistoryData { IpHistory = new List<IPInfoData>() };
            }
        }
        else
        {
            // If the file doesn't exist, initialize a new IP history data object
            ipHistoryData = new IPHistoryData { IpHistory = new List<IPInfoData>() };
        }
    }


    private void FetchPublicIP()
    {
        string ipUrl = "https://api.ipify.org/?format=json";
        UnityWebRequest ipRequest = UnityWebRequest.Get(ipUrl);

        // Start the request
        var asyncOp = ipRequest.SendWebRequest();

        // Set up a callback for when the request is complete
        asyncOp.completed += (asyncOperation) =>
        {
            string jsonIpResponse = ipRequest.downloadHandler.text;
            IP ipInfo = JsonUtility.FromJson<IP>(jsonIpResponse);

            string ip = ipInfo.ip;

           // Debug.Log("JSON IP: " + ipRequest.downloadHandler.text);
           // Debug.Log("FIRST IP: " + ip);

            // Check if the IP already exists in the history
            string cityFromHistory = GetCityFromHistory(ip);

           // Debug.Log("CITY FROM HISTORY: " + cityFromHistory);

            if (cityFromHistory != null)
            {
                // IP exists in history, use the city from history

                //  Debug.Log("USING HISTORY");
                GameObject go = GameObject.FindGameObjectWithTag("CityText");
                if (go != null)
                {
                    locationText = go.GetComponent<TextMeshProUGUI>();
                    if (locationText != null) locationText.text = cityFromHistory.ToUpper();

                }
            }
            else
            {
                // IP doesn't exist in history, fetch the city using the API

             //   Debug.Log("USING API");
                FetchCityFromAPI(ip);
            }
        };
    }

    private string GetCityFromHistory(string ip)
    {
       // Debug.Log("ipHistoryData: " + ipHistoryData);
        IPInfoData entry = ipHistoryData.IpHistory.Find(x => x.IP == ip);
        if (entry != null)
        {
            string location = entry.loc;
            string[] parts = location.Split(',');

            // Convertir las partes en valores numéricos
            latitude = parts[0];
            longitude = parts[1];
            return entry.city;
        }
        return null;
    }

    private void FetchCityFromAPI(string ip)
    {
        string url = "https://ipinfo.io/json";
        UnityWebRequest request = UnityWebRequest.Get(url);

        // Start the request
        var asyncOp = request.SendWebRequest();

        // Set up a callback for when the request is complete
        asyncOp.completed += (asyncOperation) =>
        {
            string jsonResponse = request.downloadHandler.text;
            print("GEOLOCALIZATION DATA : " + jsonResponse);

            IPInfoData response = JsonUtility.FromJson<IPInfoData>(jsonResponse);
            string city = response.city;// ParseCityFromJson(jsonResponse);
            string loc = response.loc;

            // Dividir la cadena basándonos en la coma
            string[] parts = loc.Split(',');

            // Convertir las partes en valores numéricos
            latitude = parts[0];
            longitude = parts[1];


            // Add the new entry to the history and save to file
            IPInfoData newEntry = new IPInfoData { IP = ip, city = city, loc = loc };
            ipHistoryData.IpHistory.Add(newEntry);
            SaveIPHistoryData();
          //  Debug.Log("CITY: " + city);
          //  Debug.Log("IP: " + ip);
            locationText.text = city.ToUpper();
        };
    }

 

    private void SaveIPHistoryData()
    {
        // Define the file name and file path
        string fileName = "JSONIPHistory.json";
        string filePath;

#if UNITY_EDITOR || UNITY_STANDALONE
        filePath = Path.Combine(Application.persistentDataPath, fileName);
#elif UNITY_ANDROID
        filePath = Path.Combine(Application.persistentDataPath, fileName);
#elif UNITY_IOS || UNITY_IPHONE || UNITY_WEBGL
        filePath = Path.Combine(Application.persistentDataPath, "JSON", fileName);
#else
        filePath = Path.Combine(Application.dataPath, "JSON", fileName);
#endif

        string jsonContent = JsonUtility.ToJson(ipHistoryData);
        File.WriteAllText(filePath, jsonContent);
    }

    // ... (rest of your methods)
}
