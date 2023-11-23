using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class AutoCompleteCity : MonoBehaviour
{
    public TMP_InputField city;
    public TMP_InputField countrycode;
    public TMP_InputField phonenum;
    public TMP_Text formatedText;
    public TMP_Dropdown citiesDropdown;
    public List<string> dropdownlist;
    public string placeID;

    void Start()
    {
        //CognitionHQ.subsystems.loginSystem.SignIn("usuario", "clave");
        //print(CognitionHQ.subsystems.utilitiesSystem.FormatPhoneNumber("+543489323274"));
        //CognitionHQ.subsystems.blockchain.GetBalance("0x056412215468");

        phonenum.onValueChanged.AddListener(onPhoneInputChange);
        city.onValueChanged.AddListener(AutoCompletado);
        citiesDropdown.onValueChanged.AddListener(setCountryAreaCode);
        citiesDropdown.ClearOptions();
        

    }

    public void setCityText()
    {
        city.onValueChanged.RemoveAllListeners();
        city.text = citiesDropdown.options[0].text;
        city.onValueChanged.AddListener(AutoCompletado);
        citiesDropdown.ClearOptions();
    }

    public void setCountryAreaCode(int value)
    {

        citiesDropdown.Show();

        if(value < SAI.SDK.Util.gAPI.countryNamesReadOnly.Count)
        {
            try
            {

                //print("Area code : " + SAI.subsystems.utilitiesSystem.GetPhoneByCountry(SAI.subsystems.utilitiesSystem.gAPI.countryNamesReadOnly[value].ToString()));
                countrycode.text = SAI.SDK.Util.GetPhoneByCountry(SAI.SDK.Util.gAPI.countryNamesReadOnly[value].ToString());
                //city.onValueChanged.RemoveAllListeners();
                //city.text = citiesDropdown.options[value].text;
            }
            catch (Exception x)
            {
                print(x);
                throw;
            }
            //print(citiesDropdown.options[value].text);
        }

       
        
        //city.text = citiesDropdown.value.ToString();


    }

    // Utiliza la liberia de CognitionHQ Utilities para autocompletar el texto de la ciudad
    
    public void AutoCompletado(string input)
    {

        if (city.text.Length < 4) return;

        SAI.SDK.Util.AutoCompleteCityAPI(input);
        citiesDropdown.ClearOptions();
        citiesDropdown.AddOptions(SAI.SDK.Util.gAPI.cityDescriptionsReadOnly);
        setCountryAreaCode(0);
        if (SAI.SDK.Util.gAPI.placeIdsReadOnly.Count > 0)
        {
            placeID = SAI.SDK.Util.gAPI.placeIdsReadOnly[0];
        }
        
        PlayerPrefs.SetString("PlaceID",placeID.ToString());
    }

    


    
    public void onPhoneInputChange(string value)
    {

        //print(value);

        string areaCode = countrycode.text;
        string phoneNumber = phonenum.text;
        string fullNumber = areaCode + phoneNumber;
        string phone = RemoveSpacesAndHyphens(fullNumber);
        PlayerPrefs.SetString("phone",phone);

        
        
        string pn = RemoveSubstring(SAI.SDK.Util.FormatPhoneNumber(phone),areaCode);        

        //Todo Validad // IS VALID?
        //if(CognitionHQ.subsystems.utilitiesSystem.ISVALIDNUMBER)

        phonenum.text = pn; 
    }

    string RemoveSpacesAndHyphens(string input)
    {
        // Reemplazar los espacios con una cadena vacía
        string textWithoutSpaces = input.Replace(" ", "");

        // Reemplazar los guiones con una cadena vacía
        string textWithoutHyphens = textWithoutSpaces.Replace("-", "");

        return textWithoutHyphens;
    }
    string RemoveSubstring(string mainString, string substringToRemove)
    {
        int index = mainString.IndexOf(substringToRemove);

        if (index >= 0)
        {
            mainString = mainString.Remove(index, substringToRemove.Length);
        }

        return mainString;




    }


    
    public void setCountryCode(string country)
    {
        countrycode.text = SAI.SDK.Util.GetPhoneByCountry(country);
    }
  

  
}
