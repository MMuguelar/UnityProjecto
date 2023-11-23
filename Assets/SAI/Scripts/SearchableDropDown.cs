using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
//using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class SearchableDropDown : MonoBehaviour
{

    [SerializeField] private Button blockerButton;
    [SerializeField] private GameObject buttonsPrefab = null;
    [SerializeField] private int maxScrollRectSize = 180;

    private List<string> avlOptions = new List<string>();

    private Button ddButton = null;
    private TMP_InputField inputField = null;
    private ScrollRect scrollRect = null;
    private Transform content = null;
    private RectTransform scrollRectTrans;
    private bool isContentHidden = true;
    private List<Button> initializedButtons = new List<Button>();

    public delegate void OnValueChangedDel(string val);
    public OnValueChangedDel OnValueChangedEvt;
    public Dictionary<int, string> Countries = new Dictionary<int, string>();

    public string[] countryNames = { "�land Islands", "Zimbabwe", "Zambia", "Yemen", "Western Sahara", "Wallis and Futuna", "Virgin Islands (U.S.)", "Virgin Islands (British)", "Viet Nam", "Venezuela", "Vanuatu", "Uzbekistan", "Uruguay", "United States of America", "United States Minor Outlying Islands", "United Kingdom", "United Arab Emirates", "Ukraine", "Uganda", "Tuvalu", "Turks and Caicos Islands", "Turkmenistan", "Turkey", "Tunisia", "Trinidad and Tobago", "Tonga", "Tokelau", "Togo", "Timor-Leste", "Thailand", "Tanzania, United Republic of", "Tajikistan", "Taiwan", "Syrian Arab Republic", "Switzerland", "Sweden", "Svalbard and Jan Mayen", "Suriname", "Sudan", "Sri Lanka", "Spain", "South Sudan", "South Georgia and the South Sandwich Islands", "South Africa", "Somalia", "Solomon Islands", "Slovenia", "Slovakia", "Sint Maarten (Dutch part)", "Singapore", "Sierra Leone", "Seychelles", "Serbia", "Senegal", "Saudi Arabia", "Sao Tome and Principe", "San Marino", "Samoa", "Saint Vincent and the Grenadines", "Saint Pierre and Miquelon", "Saint Martin (French part)", "Saint Lucia", "Saint Kitts and Nevis", "Saint Helena, Ascension and Tristan da Cunha", "Saint Barth�lemy", "R�union", "Rwanda", "Russian Federation", "Romania", "Republic of North Macedonia", "Qatar", "Puerto Rico", "Portugal", "Poland", "Pitcairn", "Philippines", "Peru", "Paraguay", "Papua New Guinea", "Panama", "Palestine, State of", "Palau", "Pakistan", "Oman", "Norway", "Northern Mariana Islands", "Norfolk Island", "Niue", "Nigeria", "Niger", "Nicaragua", "New Zealand", "New Caledonia", "Netherlands", "Nepal", "Nauru", "Namibia", "Myanmar", "Mozambique", "Morocco", "Montserrat", "Montenegro", "Mongolia", "Monaco", "Moldova (the Republic of)", "Micronesia (Federated States of)", "Mexico", "Mayotte", "Mauritius", "Mauritania", "Martinique", "Marshall Islands", "Malta", "Mali", "Maldives", "Malaysia", "Malawi", "Madagascar", "Macao", "Luxembourg", "Lithuania", "Liechtenstein", "Libya", "Liberia", "Lesotho", "Lebanon", "Latvia", "Lao People's Democratic Republic", "Kyrgyzstan", "Kuwait", "Korea (the Republic of)", "Korea (the Democratic People's Republic of)", "Kiribati", "Kenya", "Kazakhstan", "Jordan", "Jersey", "Japan", "Jamaica", "Italy", "Israel", "Isle of Man", "Ireland", "Iraq", "Iran (Islamic Republic of)", "Indonesia", "India", "Iceland", "Hungary", "Hong Kong", "Honduras", "Holy See", "Heard Island and McDonald Islands", "Haiti", "Guyana", "Guinea-Bissau", "Guinea", "Guernsey", "Guatemala", "Guam", "Guadeloupe", "Grenada", "Greenland", "Greece", "Gibraltar", "Ghana", "Germany", "Georgia", "Gambia", "Gabon", "French Southern Territories", "French Polynesia", "French Guiana", "France", "Finland", "Fiji", "Faroe Islands", "Falkland Islands [Malvinas]", "Ethiopia", "Eswatini", "Estonia", "Eritrea", "Equatorial Guinea", "El Salvador", "Egypt", "Ecuador", "Dominican Republic", "Dominica", "Djibouti", "Denmark", "C�te d'Ivoire", "Czechia", "Cyprus", "Cura�ao", "Cuba", "Croatia", "Costa Rica", "Cook Islands", "Congo", "Congo (the Democratic Republic of the)", "Comoros", "Colombia", "Cocos (Keeling) Islands", "Christmas Island", "China", "Chile", "Chad", "Central African Republic", "Cayman Islands", "Canada", "Cameroon", "Cambodia", "Cabo Verde", "Burundi", "Burkina Faso", "Bulgaria", "Brunei Darussalam", "British Indian Ocean Territory", "Brazil", "Bouvet Island", "Botswana", "Bosnia and Herzegovina", "Bonaire, Sint Eustatius and Saba", "Bolivia (Plurinational State of)", "Bhutan", "Bermuda", "Benin", "Belize", "Belgium", "Belarus", "Barbados", "Bangladesh", "Bahrain", "Bahamas", "Azerbaijan", "Austria", "Australia", "Aruba", "Armenia", "Argentina", "Antigua and Barbuda", "Antarctica", "Anguilla", "Angola", "Andorra", "American Samoa", "Algeria", "Albania", "Afghanistan" };
    public int[] countryValues = { 248, 716, 894, 887, 732, 876, 850, 92, 704, 862, 548, 860, 858, 840, 581, 826, 784, 804, 800, 798, 796, 795, 792, 788, 780, 776, 772, 768, 626, 764, 834, 762, 158, 760, 756, 752, 744, 740, 729, 144, 724, 728, 239, 710, 706, 90, 705, 703, 534, 702, 694, 690, 688, 686, 682, 678, 674, 882, 670, 666, 663, 662, 659, 654, 652, 638, 646, 643, 642, 807, 634, 630, 620, 616, 612, 608, 604, 600, 598, 591, 275, 585, 586, 512, 578, 580, 574, 570, 566, 562, 558, 554, 540, 528, 524, 520, 516, 104, 508, 504, 500, 499, 496, 492, 498, 583, 484, 175, 480, 478, 474, 584, 470, 466, 462, 458, 454, 450, 446, 442, 440, 438, 434, 430, 426, 422, 428, 418, 417, 414, 410, 408, 296, 404, 398, 400, 832, 392, 388, 380, 376, 833, 372, 368, 364, 360, 356, 352, 348, 344, 340, 336, 334, 332, 328, 624, 324, 831, 320, 316, 312, 308, 304, 300, 292, 288, 276, 268, 270, 266, 260, 258, 254, 250, 246, 242, 234, 238, 231, 748, 233, 232, 226, 222, 818, 218, 214, 212, 262, 208, 384, 203, 196, 531, 192, 191, 188, 184, 178, 180, 174, 170, 166, 162, 156, 152, 148, 140, 136, 124, 120, 116, 132, 108, 854, 100, 96, 86, 76, 74, 72, 70, 535, 68, 64, 60, 204, 84, 56, 112, 52, 50, 48, 44, 31, 40, 36, 533, 51, 32, 28, 10, 660, 24, 20, 16, 12, 8, 4 };


    void Start()
    {
        Init();
    }

    private void Init()
    {
        FillDictionaryAndOptions(avlOptions);
        ddButton = this.GetComponentInChildren<Button>();
        scrollRect = this.GetComponentInChildren<ScrollRect>();
        inputField = this.GetComponentInChildren<TMP_InputField>();
        scrollRectTrans = scrollRect.GetComponent<RectTransform>();
        content = scrollRect.content;

        blockerButton.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        blockerButton.gameObject.SetActive(false);
        blockerButton.transform.SetParent(this.GetComponentInParent<Canvas>().transform);

        blockerButton.onClick.AddListener(OnBlockerButtClick);
        ddButton.onClick.AddListener(OnDDButtonClick);
        scrollRect.onValueChanged.AddListener(OnScrollRectvalueChange);
        inputField.onValueChanged.AddListener(OnInputvalueChange);
        inputField.onEndEdit.AddListener(OnEndEditing);

        AddItemToScrollRect(avlOptions);

    }
    public void FillDictionaryAndOptions(List<String> options)
    {
        for (int i = 0; i < 249; i++)
        {
            Countries.Add(countryValues[i], countryNames[i]);
            options.Add(countryNames[i]);
        }

    }

    public string GetValue()
    {
        return inputField.text;
    }

    public void ResetDropDown()
    {
        inputField.text = string.Empty; 
    }


    public void AddItemToScrollRect(List<string> options)
    {
        foreach (var option in options)
        {
            var buttObj = Instantiate(buttonsPrefab, content);
            buttObj.GetComponentInChildren<TMP_Text>().text = option;
            buttObj.name = option;
            buttObj.SetActive(true);
            var butt = buttObj.GetComponent<Button>();
            butt.onClick.AddListener(delegate { OnItemSelected(buttObj); });
            initializedButtons.Add(butt);
        }
        ResizeScrollRect();
        scrollRect.gameObject.SetActive(false);
    }


    private void OnEndEditing(string arg)
    {
        if (string.IsNullOrEmpty(arg))
        {
            Debug.Log("no value entered ");
            return;
        }
        StartCoroutine(CheckIfValidInput(arg));
    }


    IEnumerator CheckIfValidInput(string arg)
    {
        yield return new WaitForSeconds(1);
        if (!avlOptions.Contains(arg))
        {
            inputField.text = String.Empty;
        }
        OnValueChangedEvt?.Invoke(inputField.text);
    }


    private void ResizeScrollRect()
    {

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)content.transform);
        var length = content.GetComponent<RectTransform>().sizeDelta.y;

        scrollRectTrans.sizeDelta = length > maxScrollRectSize ? new Vector2(scrollRectTrans.sizeDelta.x,
            maxScrollRectSize) : new Vector2(scrollRectTrans.sizeDelta.x, length + 5);
    }


    private void OnInputvalueChange(string arg0)
    {
        if (!avlOptions.Contains(arg0))
        {
            FilterDropdown(arg0);
        }
    }


    public void FilterDropdown(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            foreach (var button in initializedButtons)
                button.gameObject.SetActive(true);
            ResizeScrollRect();
            scrollRect.gameObject.SetActive(false);
            return;
        }

        var count = 0;
        foreach (var button in initializedButtons)
        {
            if (!button.name.ToLower().Contains(input.ToLower()))
            {
                button.gameObject.SetActive(false);
            }
            else
            {
                button.gameObject.SetActive(true);
                count++;
            }
        }

        SetScrollActive(count > 0);
        ResizeScrollRect();
    }


    private void OnScrollRectvalueChange(Vector2 arg0)
    {
        //Debug.Log("scroll ");
    }


    private void OnItemSelected(GameObject obj)
    {
        inputField.text = obj.name;
        foreach (var button in initializedButtons)
            button.gameObject.SetActive(true);
        isContentHidden = false;
        OnDDButtonClick();
        StopAllCoroutines();
        StartCoroutine(CheckIfValidInput(obj.name));
    }


    private void OnDDButtonClick()
    {
        if(GetActiveButtons()<=0)
            return;
        ResizeScrollRect();
        SetScrollActive(isContentHidden);
    }
    private void OnBlockerButtClick()
    {
        SetScrollActive(false);
    }


    private void SetScrollActive(bool status)
    {
        scrollRect.gameObject.SetActive(status);
        blockerButton.gameObject.SetActive(status);
        isContentHidden = !status;
        ddButton.transform.localScale = status ? new Vector3(1, -1, 1) : new Vector3(1, 1, 1);
    }


    private float GetActiveButtons()
    {
        var count = content.transform.Cast<Transform>().Count(child => child.gameObject.activeSelf);
        var length = buttonsPrefab.GetComponent<RectTransform>().sizeDelta.y * count;
        return length;
    }

}
