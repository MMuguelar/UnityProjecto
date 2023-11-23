using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using System.Globalization;
using System.Net;

public class DHT : MonoBehaviour
{
    private const string server = "https://networkmonitor.sos.space";

    private const string SERVER_GEOCODING = "https://api.geoapify.com/v1/geocode/reverse?";

    private string serverAddNode = server + "/addNode";
    private string serverKeepAlive = server + "/keepAlive";
    private string serverNodeConfirmUser = server + "/confirmUser";

    public Geoloc GEODATA;

    public string LAT = "0";
    public string LONG = "0";
    public string ALT = "0";
    public string CITY = "";

    public User_Profile USER_PROFILE = new User_Profile();

    public int current_id = 0;
    public string current_name = "";

    void Start()
    {
        //GetUserLocation();
        StartCoroutine(addNodeC(""));
    }
    public class BypassCertificateHandler : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            // Ignore certificate validation
            return true;
        }
    }

    IEnumerator addNodeC(string name)
    {
        WWWForm form = new WWWForm();
        name = "newUser";
        string user_id = name;

        string GS = "0";
        string GP = "0";
        string GC = "0";
        string GW = "0";

        USER_PROFILE.fromCity = "a";
        USER_PROFILE.altitude = "0";
        USER_PROFILE.lat = "0";
        USER_PROFILE.longitude = "0";

        string server = serverAddNode + "?name=" + name + "&avail=Instant" + "&userID=" + user_id +
            "&fromName=" + USER_PROFILE.fromCity + "&mag=Unknown" + "&dist=21063" + "&alt=" + USER_PROFILE.altitude + "&lat=" + USER_PROFILE.lat + "&long=" + USER_PROFILE.longitude + "&GS=" + GS + "&GP=" + GP
            + "&GC=" + GC + "&GW=" + GW + "&TS=1250" + "&TP=155" + "&TC=40000" + "&TW=12" + "&velocity=31000";
        Debug.Log(server);

        UnityWebRequest www = new UnityWebRequest();

        //www.certificateHandler= new AcceptAllCertificatesSignedWithASpecificKeyPublicKey();

        using (www = UnityWebRequest.Post(server, form))
        {

            www.certificateHandler = new BypassCertificateHandler();

            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string DATA = www.downloadHandler.text;
                Debug.Log(DATA);


                if (DATA != "CONFIRM_USER")
                {
                    //NodesId nodeid = new NodesId();
                    //nodeid = JsonUtility.FromJson<NodesId>(DATA);
                    //current_id = nodeid.ID;
                    //print("current_id " + current_id);

                    //getNodeInfo(current_id.ToString());

                    //Main MAIN = GameObject.Find("CODE").GetComponent<Main>();
                    //if (MAIN != null)
                    //    MAIN.get_online_nodes_firsttime();

                    //nodoACTIVADO = true;

                    CancelInvoke("keepalive_every");
                    Invoke("keepalive_every", 30);
                }
                else
                {

                    confirmUser(name);
                }
            }
        }
    }

    public void keepAlive()
    {
        StartCoroutine(keepAliveC());
    }

    public void confirmUser(string name)
    {
        StartCoroutine(confirmUserC(name));
    }

    IEnumerator confirmUserC(string name)
    {
        WWWForm form = new WWWForm();

        string user_id = name;
        string user_base64 = "";

        user_base64 = name;
        user_base64 = Base64Encode(user_base64);
        user_base64 = Base64Encode(user_base64);
        user_base64 = Base64Encode(user_base64);
        user_base64 = Base64Encode(user_base64);


        string server = serverNodeConfirmUser + "?name=" + user_base64;

        print("confirmUserC " + name);
        print("confirmUserC " + server);
        Debug.Log("BASE64NAME: " + user_base64);

        using (UnityWebRequest www = UnityWebRequest.Post(server, form))
        {
            www.certificateHandler = new BypassCertificateHandler();

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string DATA = www.downloadHandler.text;
                print("confirmUserC " + DATA);
                current_name = name;


                CancelInvoke("addNodeDElayed");
                Invoke("addNodeDElayed", 2);

            }
        }
    }
    void addNodeDElayed()
    {
        Debug.Log("Entered addNodeDElayed");
        addNode(current_name);
    }

    public void addNode(string name)
    {


        StartCoroutine(addNodeC(name));


    }

    IEnumerator keepAliveC()
    {
        WWWForm form = new WWWForm();

        string server = serverKeepAlive + "?id=" + current_id.ToString();

        print("keepAliveC " + server);

        using (UnityWebRequest www = UnityWebRequest.Post(server, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string DATA = www.downloadHandler.text;
                print("keepAliveC " + DATA);
            }
        }
    }

    public void keepalive_every()
    {
        keepAlive();
    }

    public static string Base64Encode(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }
    public void GetUserLocation()
    {
        if (!Input.location.isEnabledByUser) //FIRST IM CHACKING FOR PERMISSION IF "true" IT MEANS USER GAVED PERMISSION FOR USING LOCATION INFORMATION
        {

            if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.CoarseLocation))
            {
                UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.CoarseLocation);
            }

            if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.FineLocation))
            {
                UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.FineLocation);
            }

        }
        else
        {
            StartCoroutine(GetLatLonUsingGPS());
        }
    }

    IEnumerator GetLatLonUsingGPS()
    {
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("Location service not enabled");
            yield break;
        }

        // Check if app has location permission
        if (UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.FineLocation))
        {
            // Start service before querying location
            Input.location.Start();

            // Wait until service initializes
            int maxWait = 20;
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                yield return new WaitForSeconds(1);
                maxWait--;
            }

            // Service didn't initialize in 20 seconds
            if (maxWait < 1)
            {
                Debug.Log("Timed out");
                yield break;
            }

            // Connection has failed
            if (Input.location.status == LocationServiceStatus.Failed)
            {
                Debug.Log("Unable to determine device location");
                yield break;
            }
            else
            {
                // Access granted and location value could be retrieved
                Debug.Log("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
                LAT = Input.location.lastData.latitude.ToString();
                LONG = Input.location.lastData.longitude.ToString();
            }

            // Stop service if there is no need to query location updates continuously
            Input.location.Stop();
        }
        else
        {
            // Request location permission from user
            UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.FineLocation);
        }
    }

    public void getGeoCodingReverse()
    {

        StartCoroutine(getGeoCodingReverseC());
    }

    IEnumerator getGeoCodingReverseC()
    {
        string tallong = "-34.5812188,-58.4316171,16.75";

        string apikey = "520159ddd17a41ddb1e7a2aa5f4aa85d";

        //string latS = "-34.5812188";
        //string longS = "-58.4316171";

        string latS = LAT;
        string longS = LONG;

        string SERVERG = SERVER_GEOCODING + "lat=" + latS + "&lon=" + longS + "&apiKey=" + apikey;

        using (UnityWebRequest webRequest = UnityWebRequest.Get(SERVERG))
        {
            print("getGeoCodingReverseC: " + SERVERG);
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log("ERROR");

            }
            else
            {

                string DATA = webRequest.downloadHandler.text;
                print("DATA " + DATA);


                GEODATA = new Geoloc();
                GEODATA = JsonUtility.FromJson<Geoloc>(DATA);

                CITY = GEODATA.features[0].properties.city;
                USER_PROFILE.fromCity = CITY;

            }
        }
    }
}

public class User_Profile
{
    public string name;
    public bool configurado;
    public float energy;
    public float storage;
    public float processing;
    public float comm;
    public string metadata;

    public string altitude;
    public string lat;
    public string longitude;
    public string fromCity;
    public string pk;
    public string[] hashed_files;

}

[Serializable]
public class Geoloc
{
    public Features[] features;
}

[Serializable]
public class Features
{
    public Properties properties;
}

[Serializable]
public class Properties
{
    public string country_code;
    public string housenumber;
    public string street;
    public string country;
    public string state;
    public string city;
    public string district;
    public string suburb;
    public string result_type;
    public string postcode;
    public string formatted;
}