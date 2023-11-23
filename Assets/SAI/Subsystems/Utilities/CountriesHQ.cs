
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
        "Afganistán", "Albania", "Alemania", "Andorra", "Angola", "Antigua y Barbuda", "Arabia Saudita", "Argelia",
        "Argentina", "Armenia", "Australia", "Austria", "Azerbaiyán", "Bahamas", "Bahréin", "Bangladés", "Barbados",
        "Belice", "Benín", "Bielorrusia", "Birmania", "Bolivia", "Bosnia y Herzegovina", "Botsuana", "Brasil",
        "Brunéi", "Bulgaria", "Burkina Faso", "Burundi", "Bután", "Cabo Verde", "Camboya", "Camerún", "Canadá",
        "Catar", "Chad", "Chile", "China", "Chipre", "Colombia", "Comoras", "Congo", "Corea del Norte", "Corea del Sur",
        "Costa de Marfil", "Costa Rica", "Croacia", "Cuba", "Dinamarca", "Dominica", "Ecuador", "Egipto", "El Salvador",
        "Emiratos Árabes Unidos", "Eritrea", "Eslovaquia", "Eslovenia", "España", "Estados Unidos", "Estonia", "Etiopía",
        "Fiji", "Filipinas", "Finlandia", "Francia", "Gabón", "Gambia", "Georgia", "Ghana", "Granada", "Grecia",
        "Guatemala", "Guinea", "Guinea-Bisáu", "Guinea Ecuatorial", "Guyana", "Haití", "Honduras", "Hungría", "India",
        "Indonesia", "Irak", "Irán", "Irlanda", "Islandia", "Islas Marshall", "Islas Salomón", "Israel", "Italia",
        "Jamaica", "Japón", "Jordania", "Kazajistán", "Kenia", "Kirguistán", "Kiribati", "Kuwait", "Laos", "Lesoto",
        "Letonia", "Líbano", "Liberia", "Libia", "Liechtenstein", "Lituania", "Luxemburgo", "Macedonia del Norte",
        "Madagascar", "Malasia", "Malaui", "Maldivas", "Malí", "Malta", "Marruecos", "Mauricio", "Mauritania", "México",
        "Micronesia", "Moldavia", "Mónaco", "Mongolia", "Montenegro", "Mozambique", "Namibia", "Nauru", "Nepal",
        "Nicaragua", "Níger", "Nigeria", "Noruega", "Nueva Zelanda", "Omán", "Países Bajos", "Pakistán", "Palaos",
        "Panamá", "Papúa Nueva Guinea", "Paraguay", "Perú", "Polonia", "Portugal", "Reino Unido", "República Centroafricana",
        "República Checa", "República del Congo", "República Democrática del Congo", "República Dominicana", "Ruanda",
        "Rumania", "Rusia", "Samoa", "San Cristóbal y Nieves", "San Marino", "San Vicente y las Granadinas", "Santa Lucía",
        "Santo Tomé y Príncipe", "Senegal", "Serbia", "Seychelles", "Sierra Leona", "Singapur", "Siria", "Somalia",
        "Sri Lanka", "Suazilandia", "Sudáfrica", "Sudán", "Sudán del Sur", "Suecia", "Suiza", "Surinam", "Tailandia",
        "Tanzania", "Tayikistán", "Timor Oriental", "Togo", "Tonga", "Trinidad y Tobago", "Túnez", "Turkmenistán",
        "Turquía", "Tuvalu", "Ucrania", "Uganda", "Uruguay", "Uzbekistán", "Vanuatu", "Vaticano", "Venezuela", "Vietnam",
        "Yemen", "Yibuti", "Zambia", "Zimbabue"
    };

    public Dictionary<string, string> phoneCodes = new Dictionary<string, string>
    {
        { "Afganistán", "+93" }, { "Albania", "+355" }, { "Alemania", "+49" }, { "Andorra", "+376" },
        { "Angola", "+244" }, { "Antigua y Barbuda", "+1-268" }, { "Arabia Saudita", "+966" }, { "Argelia", "+213" },
        { "Argentina", "+54" }, { "Armenia", "+374" }, { "Australia", "+61" }, { "Austria", "+43" },
        { "Azerbaiyán", "+994" }, { "Bahamas", "+1-242" }, { "Bahréin", "+973" }, { "Bangladés", "+880" },
        { "Barbados", "+1-246" }, { "Belice", "+501" }, { "Benín", "+229" }, { "Bielorrusia", "+375" },
        { "Birmania", "+95" }, { "Bolivia", "+591" }, { "Bosnia y Herzegovina", "+387" }, { "Botsuana", "+267" },
        { "Brasil", "+55" }, { "Brunéi", "+673" }, { "Bulgaria", "+359" }, { "Burkina Faso", "+226" },
        { "Burundi", "+257" }, { "Bután", "+975" }, { "Cabo Verde", "+238" }, { "Camboya", "+855" },
        { "Camerún", "+237" }, { "Canadá", "+1" }, { "Catar", "+974" }, { "Chad", "+235" }, { "Chile", "+56" },
        { "China", "+86" }, { "Chipre", "+357" }, { "Colombia", "+57" }, { "Comoras", "+269" }, { "Congo", "+242" }, { "Corea del Norte", "+850" }, { "Corea del Sur", "+82" }, { "Costa de Marfil", "+225" },
        { "Costa Rica", "+506" }, { "Croacia", "+385" }, { "Cuba", "+53" }, { "Dinamarca", "+45" },
        { "Dominica", "+1-767" }, { "Ecuador", "+593" }, { "Egipto", "+20" }, { "El Salvador", "+503" },
        { "Emiratos Árabes Unidos", "+971" }, { "Eritrea", "+291" }, { "Eslovaquia", "+421" },
        { "Eslovenia", "+386" }, { "España", "+34" }, { "Estados Unidos", "+1" }, { "Estonia", "+372" },
        { "Etiopía", "+251" }, { "Fiji", "+679" }, { "Filipinas", "+63" }, { "Finlandia", "+358" },
        { "Francia", "+33" }, { "Gabón", "+241" }, { "Gambia", "+220" }, { "Georgia", "+995" },
        { "Ghana", "+233" }, { "Granada", "+1-473" }, { "Grecia", "+30" }, { "Guatemala", "+502" },
        { "Guinea", "+224" }, { "Guinea-Bisáu", "+245" }, { "Guinea Ecuatorial", "+240" }, { "Guyana", "+592" },
        { "Haití", "+509" }, { "Honduras", "+504" }, { "Hungría", "+36" }, { "India", "+91" },
        { "Indonesia", "+62" }, { "Irak", "+964" }, { "Irán", "+98" }, { "Irlanda", "+353" },
        { "Islandia", "+354" }, { "Islas Marshall", "+692" }, { "Islas Salomón", "+677" }, { "Israel", "+972" },
        { "Italia", "+39" }, { "Jamaica", "+1-876" }, { "Japón", "+81" }, { "Jordania", "+962" },
        { "Kazajistán", "+7" }, { "Kenia", "+254" }, { "Kirguistán", "+996" }, { "Kiribati", "+686" },
        { "Kuwait", "+965" }, { "Laos", "+856" }, { "Lesoto", "+266" }, { "Letonia", "+371" },
        { "Líbano", "+961" }, { "Liberia", "+231" }, { "Libia", "+218" }, { "Liechtenstein", "+423" },
        { "Lituania", "+370" }, { "Luxemburgo", "+352" }, { "Macedonia del Norte", "+389" },
        { "Madagascar", "+261" }, { "Malasia", "+60" }, { "Malaui", "+265" }, { "Maldivas", "+960" },
        { "Malí", "+223" }, { "Malta", "+356" }, { "Marruecos", "+212" }, { "Mauricio", "+230" },
        { "Mauritania", "+222" }, { "México", "+52" }, { "Micronesia", "+691" }, { "Moldavia", "+373" },
        { "Mónaco", "+377" }, { "Mongolia", "+976" }, { "Montenegro", "+382" }, { "Mozambique", "+258" },
        { "Namibia", "+264" }, { "Nauru", "+674" }, { "Nepal", "+977" }, { "Nicaragua", "+505" },
        { "Níger", "+227" }, { "Nigeria", "+234" }, { "Noruega", "+47" }, { "Nueva Zelanda", "+64" },
        { "Omán", "+968" }, { "Países Bajos", "+31" }, { "Pakistán", "+92" }, { "Palaos", "+680" },
        { "Panamá", "+507" }, { "Papúa Nueva Guinea", "+675" }, { "Paraguay", "+595" }, { "Perú", "+51" },
        { "Polonia", "+48" }, { "Portugal", "+351" }, { "Reino Unido", "+44" }, { "República Centroafricana", "+236" },
        { "República Checa", "+420" }, { "República del Congo", "+242" }, { "República Democrática del Congo", "+243" },
        { "República Dominicana", "+1-809" }, { "Ruanda", "+250" }, { "Rumania", "+40" }, { "Rusia", "+7" },
        { "Samoa", "+685" }, { "San Cristóbal y Nieves", "+1-869" }, { "San Marino", "+378" },
        { "San Vicente y las Granadinas", "+1-784" }, { "Santa Lucía", "+1-758" }, { "Santo Tomé y Príncipe", "+239" },
        { "Senegal", "+221" }, { "Serbia", "+381" }, { "Seychelles", "+248" }, { "Sierra Leona", "+232" },
        { "Singapur", "+65" }, { "Siria", "+963" }, { "Somalia", "+252" }, { "Sri Lanka", "+94" },
        { "Suazilandia", "+268" }, { "Sudáfrica", "+27" }, { "Sudán", "+249" }, { "Sudán del Sur", "+211" },
        { "Suecia", "+46" }, { "Suiza", "+41" }, { "Surinam", "+597" }, { "Tailandia", "+66" },
        { "Tanzania", "+255" }, { "Tayikistán", "+992" }, { "Timor Oriental", "+670" }, { "Togo", "+228" },
        { "Tonga", "+676" }, { "Trinidad y Tobago", "+1-868" }, { "Túnez", "+216" }, { "Turkmenistán", "+993" },
        { "Turquía", "+90" }, { "Tuvalu", "+688" }, { "Ucrania", "+380" }, { "Uganda", "+256" },
        { "Uruguay", "+598" }, { "Uzbekistán", "+998" }, { "Vanuatu", "+678" }, { "Vaticano", "+379" },
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
        // Crear un diccionario que mapee iniciales a índices de elementos en el Dropdown
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

        // Selecciona el primer país por defecto
        OnCountryDropdownValueChanged(0);
    }


    private void Update()
    {
        return;
        // Verificar si se presionó una tecla
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
        if(selectedCountry != null) { selectedCountryText.text = "País seleccionado: " + selectedCountry; }
       
        if(countryPhoneCodeText != null)
        {
            if (phoneCodes.TryGetValue(selectedCountry, out string phoneCode))
            {
                countryPhoneCodeText.text = "Código telefónico: " + phoneCode;
            }
            else
            {
                countryPhoneCodeText.text = "Código telefónico no encontrado";
            }

        }

       
    }
}
