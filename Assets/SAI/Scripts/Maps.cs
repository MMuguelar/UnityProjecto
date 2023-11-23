using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Globalization;

public class Maps : MonoBehaviour
{
    public GameObject mapGameObject;
    public string apiKey;
    public static double lat = -38.467248;
    public static double lon = -58.308311;
    public int zoom = 14;
    public enum resolution { low = 1, high = 2 };
    public resolution mapResolution = resolution.low;
    public enum type { roadmap, satellite, gybrid, terrain };
    public type mapType = type.roadmap;
    private string url = "";
    public int mapWidth = 640;
    public int mapHeight = 640;
    private Rect rect;

    private string apiKeyLast;
    private double latLast = -34.60376;
    private double lonLast = -58.38162;
    private int zoomLast = 14;
    private resolution mapResolutionLast = resolution.low;
    private type mapTypeLast = type.roadmap;
    private bool updateMap = true;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetGoogleMap());
        rect = gameObject.GetComponent<RawImage>().rectTransform.rect;
        //HACER EL ANCHO Y LARGO CON EL TRANSFORM DEL MAP
        mapWidth = (int)Math.Round(rect.width);
        mapHeight = (int)Math.Round(rect.height);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (updateMap && (apiKeyLast != apiKey || !latLast.Equals(lat) || !lonLast.Equals(lon) || zoomLast != zoom || mapResolutionLast != mapResolution || mapTypeLast != mapType))
        {
            rect = gameObject.GetComponent<RawImage>().rectTransform.rect;
            mapWidth = (int)Math.Round(rect.width);
            mapHeight = (int)Math.Round(rect.height);
            StartCoroutine(GetGoogleMap());
            updateMap = false;
        }
    }


    IEnumerator GetGoogleMap()
    {
        // Convert lat and lon to strings, replace commas with dots, and convert back to double
        string formattedLat = lat.ToString().Replace(",", ".");
        string formattedLon = lon.ToString().Replace(",", ".");
        double parsedLat = double.Parse(formattedLat, CultureInfo.InvariantCulture);
        double parsedLon = double.Parse(formattedLon, CultureInfo.InvariantCulture);

        url = "https://maps.googleapis.com/maps/api/staticmap?center=" + formattedLat + "," + formattedLon + "&zoom=" + zoom + "&size=" + mapWidth + "x" + mapHeight + "&scale=" + mapResolution + "&maptype=" + mapType + "&markers=size:big%7Ccolor:red%7Clabel:N%7C" + formattedLat + "," + formattedLon + "&key=" + apiKey;
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        Debug.Log(url);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("WWW ERROR: " + www.error);
        }
        else
        {
            gameObject.GetComponent<RawImage>().texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

            apiKeyLast = apiKey;
            latLast = parsedLat;
            lonLast = parsedLon;
            zoomLast = zoom;
            mapResolutionLast = mapResolution;
            mapTypeLast = mapType;
            updateMap = true;
        }
    }



}