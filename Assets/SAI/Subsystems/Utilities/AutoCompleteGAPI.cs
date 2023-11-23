using Ipfs;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
///  Google Autocomplete Script
///  Last Update - 25/8/23
///  Autor - Ares CognitionHQ
///  Resume :
///  Isolated Component used but Cognition Utilities to for autocompleting text 
/// </summary>
public class AutoCompleteGAPI : MonoBehaviour
{
    string CountryAPIKey = "AIzaSyBiUYwb6MdTMfHhpt_bjQwm4B96cbveyFk";
    private string inputText;

    public List<string> cityDescriptionsReadOnly = new List<string>();
    public List<string> countryNamesReadOnly = new List<string>();
    public List<string> placeIdsReadOnly = new List<string>();

    public void StartAutocompleteRequest(string input)
    {

        inputText = input;
        string url = $"https://maps.googleapis.com/maps/api/place/autocomplete/json?input={UnityWebRequest.EscapeURL(inputText)}&types=(cities)&language=es&key={CountryAPIKey}";

        //print("Tirandole a la API con el siguiente ENDPOINT : " + url);
        StartCoroutine(SendAutocompleteRequest(url));
    }

    public IEnumerator SendAutocompleteRequest(string url)
    {


        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)  // Verificar si la solicitud tuvo éxito
            {
                //Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                string response = webRequest.downloadHandler.text;
                //Debug.Log("Response: " + response);

                // Aquí puedes procesar la respuesta JSON y extraer las sugerencias de ciudades.
                ProcessResponse(response);
            }
        }




    }


    private void ProcessResponse(string jsonResponse)
    {
        JSONNode jsonNode = JSON.Parse(jsonResponse);
        JSONArray predictions = jsonNode["predictions"].AsArray;

        List<string> cityDescriptions = new List<string>();
        List<string> countryNames = new List<string>();
        List<string> countryID = new List<string>();

        foreach (JSONNode prediction in predictions)
        {
            string description = prediction["description"].Value;

            // Remove any additional information after the comma
            //description = Regex.Split(description, ",")[0].Trim();

            cityDescriptions.Add(description);
            cityDescriptionsReadOnly = cityDescriptions;

            // Extract country name from the third term
            string countryName = prediction["terms"][prediction["terms"].Count-1]["value"].Value;
            countryNames.Add(countryName);
            countryNamesReadOnly = countryNames;

            // Extract and store place_id
            string placeId = prediction["place_id"].Value;
            countryID.Add(placeId);
            placeIdsReadOnly = countryID;
        }

        // Now you can use the cityDescriptions list as needed (e.g., display in UI).
    }

}
