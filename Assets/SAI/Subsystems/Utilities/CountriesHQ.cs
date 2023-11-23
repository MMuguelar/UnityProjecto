
using System.Collections.Generic;
using UnityEngine;

using TMPro;


public class CountriesHQ : MonoBehaviour
{
    public TMP_Dropdown countryDropdown;
    public TMP_Text selectedCountryText;
    public TMP_Text countryPhoneCodeText;

  

    private Dictionary<char, List<int>> initialToIndices = new Dictionary<char, List<int>>();
    private char currentInitial;
    private int currentIndex = -1;


    public List<string> countries = new List<string>
    {
        "Afganist�n", "Albania", "Alemania", "Andorra", "Angola", "Antigua y Barbuda", "Arabia Saudita", "Argelia",
        "Argentina", "Armenia", "Australia", "Austria", "Azerbaiy�n", "Bahamas", "Bahr�in", "Banglad�s", "Barbados",
        "Belice", "Ben�n", "Bielorrusia", "Birmania", "Bolivia", "Bosnia y Herzegovina", "Botsuana", "Brasil",
        "Brun�i", "Bulgaria", "Burkina Faso", "Burundi", "But�n", "Cabo Verde", "Camboya", "Camer�n", "Canad�",
        "Catar", "Chad", "Chile", "China", "Chipre", "Colombia", "Comoras", "Congo", "Corea del Norte", "Corea del Sur",
        "Costa de Marfil", "Costa Rica", "Croacia", "Cuba", "Dinamarca", "Dominica", "Ecuador", "Egipto", "El Salvador",
        "Emiratos �rabes Unidos", "Eritrea", "Eslovaquia", "Eslovenia", "Espa�a", "Estados Unidos", "Estonia", "Etiop�a",
        "Fiji", "Filipinas", "Finlandia", "Francia", "Gab�n", "Gambia", "Georgia", "Ghana", "Granada", "Grecia",
        "Guatemala", "Guinea", "Guinea-Bis�u", "Guinea Ecuatorial", "Guyana", "Hait�", "Honduras", "Hungr�a", "India",
        "Indonesia", "Irak", "Ir�n", "Irlanda", "Islandia", "Islas Marshall", "Islas Salom�n", "Israel", "Italia",
        "Jamaica", "Jap�n", "Jordania", "Kazajist�n", "Kenia", "Kirguist�n", "Kiribati", "Kuwait", "Laos", "Lesoto",
        "Letonia", "L�bano", "Liberia", "Libia", "Liechtenstein", "Lituania", "Luxemburgo", "Macedonia del Norte",
        "Madagascar", "Malasia", "Malaui", "Maldivas", "Mal�", "Malta", "Marruecos", "Mauricio", "Mauritania", "M�xico",
        "Micronesia", "Moldavia", "M�naco", "Mongolia", "Montenegro", "Mozambique", "Namibia", "Nauru", "Nepal",
        "Nicaragua", "N�ger", "Nigeria", "Noruega", "Nueva Zelanda", "Om�n", "Pa�ses Bajos", "Pakist�n", "Palaos",
        "Panam�", "Pap�a Nueva Guinea", "Paraguay", "Per�", "Polonia", "Portugal", "Reino Unido", "Rep�blica Centroafricana",
        "Rep�blica Checa", "Rep�blica del Congo", "Rep�blica Democr�tica del Congo", "Rep�blica Dominicana", "Ruanda",
        "Rumania", "Rusia", "Samoa", "San Crist�bal y Nieves", "San Marino", "San Vicente y las Granadinas", "Santa Luc�a",
        "Santo Tom� y Pr�ncipe", "Senegal", "Serbia", "Seychelles", "Sierra Leona", "Singapur", "Siria", "Somalia",
        "Sri Lanka", "Suazilandia", "Sud�frica", "Sud�n", "Sud�n del Sur", "Suecia", "Suiza", "Surinam", "Tailandia",
        "Tanzania", "Tayikist�n", "Timor Oriental", "Togo", "Tonga", "Trinidad y Tobago", "T�nez", "Turkmenist�n",
        "Turqu�a", "Tuvalu", "Ucrania", "Uganda", "Uruguay", "Uzbekist�n", "Vanuatu", "Vaticano", "Venezuela", "Vietnam",
        "Yemen", "Yibuti", "Zambia", "Zimbabue"
    };

    public Dictionary<string, string> phoneCodes = new Dictionary<string, string>
    {
        { "Afganist�n", "+93" }, { "Albania", "+355" }, { "Alemania", "+49" }, { "Andorra", "+376" },
        { "Angola", "+244" }, { "Antigua y Barbuda", "+1-268" }, { "Arabia Saudita", "+966" }, { "Argelia", "+213" },
        { "Argentina", "+54" }, { "Armenia", "+374" }, { "Australia", "+61" }, { "Austria", "+43" },
        { "Azerbaiy�n", "+994" }, { "Bahamas", "+1-242" }, { "Bahr�in", "+973" }, { "Banglad�s", "+880" },
        { "Barbados", "+1-246" }, { "Belice", "+501" }, { "Ben�n", "+229" }, { "Bielorrusia", "+375" },
        { "Birmania", "+95" }, { "Bolivia", "+591" }, { "Bosnia y Herzegovina", "+387" }, { "Botsuana", "+267" },
        { "Brasil", "+55" }, { "Brun�i", "+673" }, { "Bulgaria", "+359" }, { "Burkina Faso", "+226" },
        { "Burundi", "+257" }, { "But�n", "+975" }, { "Cabo Verde", "+238" }, { "Camboya", "+855" },
        { "Camer�n", "+237" }, { "Canad�", "+1" }, { "Catar", "+974" }, { "Chad", "+235" }, { "Chile", "+56" },
        { "China", "+86" }, { "Chipre", "+357" }, { "Colombia", "+57" }, { "Comoras", "+269" }, { "Congo", "+242" }, { "Corea del Norte", "+850" }, { "Corea del Sur", "+82" }, { "Costa de Marfil", "+225" },
        { "Costa Rica", "+506" }, { "Croacia", "+385" }, { "Cuba", "+53" }, { "Dinamarca", "+45" },
        { "Dominica", "+1-767" }, { "Ecuador", "+593" }, { "Egipto", "+20" }, { "El Salvador", "+503" },
        { "Emiratos �rabes Unidos", "+971" }, { "Eritrea", "+291" }, { "Eslovaquia", "+421" },
        { "Eslovenia", "+386" }, { "Espa�a", "+34" }, { "Estados Unidos", "+1" }, { "Estonia", "+372" },
        { "Etiop�a", "+251" }, { "Fiji", "+679" }, { "Filipinas", "+63" }, { "Finlandia", "+358" },
        { "Francia", "+33" }, { "Gab�n", "+241" }, { "Gambia", "+220" }, { "Georgia", "+995" },
        { "Ghana", "+233" }, { "Granada", "+1-473" }, { "Grecia", "+30" }, { "Guatemala", "+502" },
        { "Guinea", "+224" }, { "Guinea-Bis�u", "+245" }, { "Guinea Ecuatorial", "+240" }, { "Guyana", "+592" },
        { "Hait�", "+509" }, { "Honduras", "+504" }, { "Hungr�a", "+36" }, { "India", "+91" },
        { "Indonesia", "+62" }, { "Irak", "+964" }, { "Ir�n", "+98" }, { "Irlanda", "+353" },
        { "Islandia", "+354" }, { "Islas Marshall", "+692" }, { "Islas Salom�n", "+677" }, { "Israel", "+972" },
        { "Italia", "+39" }, { "Jamaica", "+1-876" }, { "Jap�n", "+81" }, { "Jordania", "+962" },
        { "Kazajist�n", "+7" }, { "Kenia", "+254" }, { "Kirguist�n", "+996" }, { "Kiribati", "+686" },
        { "Kuwait", "+965" }, { "Laos", "+856" }, { "Lesoto", "+266" }, { "Letonia", "+371" },
        { "L�bano", "+961" }, { "Liberia", "+231" }, { "Libia", "+218" }, { "Liechtenstein", "+423" },
        { "Lituania", "+370" }, { "Luxemburgo", "+352" }, { "Macedonia del Norte", "+389" },
        { "Madagascar", "+261" }, { "Malasia", "+60" }, { "Malaui", "+265" }, { "Maldivas", "+960" },
        { "Mal�", "+223" }, { "Malta", "+356" }, { "Marruecos", "+212" }, { "Mauricio", "+230" },
        { "Mauritania", "+222" }, { "M�xico", "+52" }, { "Micronesia", "+691" }, { "Moldavia", "+373" },
        { "M�naco", "+377" }, { "Mongolia", "+976" }, { "Montenegro", "+382" }, { "Mozambique", "+258" },
        { "Namibia", "+264" }, { "Nauru", "+674" }, { "Nepal", "+977" }, { "Nicaragua", "+505" },
        { "N�ger", "+227" }, { "Nigeria", "+234" }, { "Noruega", "+47" }, { "Nueva Zelanda", "+64" },
        { "Om�n", "+968" }, { "Pa�ses Bajos", "+31" }, { "Pakist�n", "+92" }, { "Palaos", "+680" },
        { "Panam�", "+507" }, { "Pap�a Nueva Guinea", "+675" }, { "Paraguay", "+595" }, { "Per�", "+51" },
        { "Polonia", "+48" }, { "Portugal", "+351" }, { "Reino Unido", "+44" }, { "Rep�blica Centroafricana", "+236" },
        { "Rep�blica Checa", "+420" }, { "Rep�blica del Congo", "+242" }, { "Rep�blica Democr�tica del Congo", "+243" },
        { "Rep�blica Dominicana", "+1-809" }, { "Ruanda", "+250" }, { "Rumania", "+40" }, { "Rusia", "+7" },
        { "Samoa", "+685" }, { "San Crist�bal y Nieves", "+1-869" }, { "San Marino", "+378" },
        { "San Vicente y las Granadinas", "+1-784" }, { "Santa Luc�a", "+1-758" }, { "Santo Tom� y Pr�ncipe", "+239" },
        { "Senegal", "+221" }, { "Serbia", "+381" }, { "Seychelles", "+248" }, { "Sierra Leona", "+232" },
        { "Singapur", "+65" }, { "Siria", "+963" }, { "Somalia", "+252" }, { "Sri Lanka", "+94" },
        { "Suazilandia", "+268" }, { "Sud�frica", "+27" }, { "Sud�n", "+249" }, { "Sud�n del Sur", "+211" },
        { "Suecia", "+46" }, { "Suiza", "+41" }, { "Surinam", "+597" }, { "Tailandia", "+66" },
        { "Tanzania", "+255" }, { "Tayikist�n", "+992" }, { "Timor Oriental", "+670" }, { "Togo", "+228" },
        { "Tonga", "+676" }, { "Trinidad y Tobago", "+1-868" }, { "T�nez", "+216" }, { "Turkmenist�n", "+993" },
        { "Turqu�a", "+90" }, { "Tuvalu", "+688" }, { "Ucrania", "+380" }, { "Uganda", "+256" },
        { "Uruguay", "+598" }, { "Uzbekist�n", "+998" }, { "Vanuatu", "+678" }, { "Vaticano", "+379" },
        { "Venezuela", "+58" }, { "Vietnam", "+84" }, { "Yemen", "+967" }, { "Yibuti", "+253" },
        { "Zambia", "+260" }, { "Zimbabue", "+263" }
    };

    private void Start()
    {
        InitializeDropdown();
        //CapitalLetterFunction();
    }

   /* private void CapitalLetterFunction()
    {
        // Crear un diccionario que mapee iniciales a �ndices de elementos en el Dropdown
        for (int i = 0; i < countryDropdown.options.Count; i++)
        {
            char initial = char.ToLower(countryDropdown.options[i].text[0]);
            if (!initialToIndices.ContainsKey(initial))
            {
                initialToIndices.Add(initial, new List<int>());
            }
            initialToIndices[initial].Add(i);
        }
    }
   */

    private void InitializeDropdown()
    {
        if (countryDropdown == null) return;
        countryDropdown.ClearOptions();
        countryDropdown.AddOptions(countries);
        countryDropdown.onValueChanged.AddListener(OnCountryDropdownValueChanged);

        // Selecciona el primer pa�s por defecto
        OnCountryDropdownValueChanged(0);
    }


    private void Update()
    {
        return;
        // Verificar si se presion� una tecla
        if (Input.anyKeyDown)
        {
            char pressedKey = char.ToLower(Input.inputString[0]);

            // Verificar si la tecla presionada corresponde a una inicial en el Dropdown
            if (initialToIndices.ContainsKey(pressedKey))
            {
                // Si es la misma inicial, avanzar al siguiente elemento
                if (currentInitial == pressedKey)
                {
                    currentIndex = (currentIndex + 1) % initialToIndices[pressedKey].Count;
                }
                else
                {
                    currentInitial = pressedKey;
                    currentIndex = 0;
                }

                int indexToSelect = initialToIndices[pressedKey][currentIndex];
                countryDropdown.value = indexToSelect;
                countryDropdown.onValueChanged.Invoke(indexToSelect); // Invocar el evento de cambio
            }
        }
    }
    private void OnCountryDropdownValueChanged(int index)
    {
        if (selectedCountryText == null) return;
        string selectedCountry = countries[index];
        if(selectedCountry != null) { selectedCountryText.text = "Pa�s seleccionado: " + selectedCountry; }
       
        if(countryPhoneCodeText != null)
        {
            if (phoneCodes.TryGetValue(selectedCountry, out string phoneCode))
            {
                countryPhoneCodeText.text = "C�digo telef�nico: " + phoneCode;
            }
            else
            {
                countryPhoneCodeText.text = "C�digo telef�nico no encontrado";
            }

        }

       
    }
}
