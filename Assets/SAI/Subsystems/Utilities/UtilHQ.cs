using PhoneNumbers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class UtilHQ : MonoBehaviour
{

    public ErrorHandler ErrorHandler;
    public UDP UDP;



    public AutoCompleteGAPI gAPI;
    public PhoneNumberFormatterHQ phoneUtil;
    public CountriesHQ countryUtil;




    public void DestroyChilders(GameObject parent)
    {
        // Itera a través de todos los hijos del objeto padre
        for (int i = parent.transform.childCount - 1; i >= 0; i--)
        {
            // Obtén el hijo en la posición 'i'
            GameObject hijo = parent.transform.GetChild(i).gameObject;

            // Destruye el objeto hijo
            Destroy(hijo);
        }
    }
    public void OpenURL(string url)
    {
        if (!string.IsNullOrEmpty(url))
            Application.OpenURL(url);
    }
    public void AutoCompleteCityAPI(string text)
    {
        gAPI.StartAutocompleteRequest(text);
    }
    public string FormatPhoneNumber(string phoneNumber)
    {
        return phoneUtil.InternationalFormatter(phoneNumber);
    }
    public string GetPhoneByCountry(string country)
    {

        if (countryUtil.phoneCodes.ContainsKey(country))
        {
            return countryUtil.phoneCodes[country];
        }
        else return null;

    }
    public string GetCountryByPhone(string phoneNumber)
    {

        if (countryUtil.phoneCodes.ContainsValue(phoneNumber))
        {
            return countryUtil.phoneCodes[phoneNumber];
        }
        else return null;
    }
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

   

}




