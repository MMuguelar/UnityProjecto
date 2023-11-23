using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;

public static class SharedDateIaProfile
{
    public static int profileID { get; set; }
    public static string profileName { get; set; }
    public static int checkPostPut { get; set; }
}
public class IaPrefabs : MonoBehaviour
{
    [Header("ButtonsMemory")]
    public GameObject faqs;
    public GameObject memoryLink;
    public GameObject uploadFile;
    public TMP_Text faqsButton;
    public TMP_Text memoryLinkButton;
    public TMP_Text uploadFileButton;
    public multipleChoiceDrop scriptMultiple;

    [Header("Others")]

    public GameObject iaPrefab;
    public GameObject addIA;
    public GameObject iaContent;
    public GameObject panelIAConfiguration;
    public GameObject panelMemory;
    public GameObject panelChat;
    public GameObject dropdownContentIA;
    public GameObject dropdownContentMemory;
    public GameObject deletePanel;
    public GameObject waitingPanel;

    [Header("Profile IA")]

    public TMP_Dropdown privacyDrop;
    public TMP_Dropdown baseModelDrop;
    public TMP_Dropdown memorytype;
    public TMP_Text memoryDrop;
    public TMP_InputField newNameIA;
    public TMP_InputField promptInput;
    public TMP_InputField aiDescription;
    public TMP_InputField introMessage;

    private bool isExpandedIA;
    private bool isExpandedMemory;
    private bool newDropDown;
    private bool panelIA;
    private bool panelMemoryP;
    private bool panelChatP;
    private IAProfileList profileList;


    public void GetListIAProfile(string jsonResponse)
    {
        profileList = JsonConvert.DeserializeObject<IAProfileList>(jsonResponse);
    }

    public void IAConfigure()
    {
        DeleteContent();

        GameObject t;

        SAI.SDK.iarequest.GetIAProfileList();

        List<IAProfile> profiles = profileList.profiles;

        //// BOX IA PREFABS ////
        foreach (var profile in profiles)
        {
            t = Instantiate(iaPrefab, iaContent.transform);

            int profileId = profile.aiprofile_id;
            string nameIA = profile.name;
            string baseModel = profile.basemodel;
            string privacy = profile.privacy;
            string memory = string.Join(",", profile.memory);
            string prompt = profile.prompt;
            string iaDescription = profile.ai_description;
            string intro_Message = profile.intro_message;

            t.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => DeleteMessage(profileId));
            t.transform.GetChild(2).GetComponent<TMP_Text>().text = profile.name;
            t.transform.GetChild(3).GetComponent<TMP_Text>().text = ConvertTime(profile.training_time);
            t.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() => ToggleDropdownIA(profileId, nameIA , privacy, memory, baseModel, prompt, iaDescription, intro_Message));
            t.transform.GetChild(5).GetComponent<Button>().onClick.AddListener(() => ToggleDropdownMemory(profileId));
            
        }
    }

    private string ConvertTime(long minutes)
    {
        long days = minutes / 1440; // 1 day = 24 hours * 60 minutes
        long hours = (minutes % 1440) / 60;
        long minutesTraining = minutes % 60;
        return string.Format("{0} days {1} hours {2} minutes ago",days,hours,minutesTraining);
    }

    public void NewIAConfiguration()
    {
        GameObject t;
        t = Instantiate(addIA, iaContent.transform);
        t.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => NewToggleDropdownIA());
    }

    public void DeleteMessage(int profileId)
    {
        int sharedProfileID = profileId;
        deletePanel.SetActive(true);
        deletePanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => DeleteProfile(sharedProfileID));
    }

    public void PanelDelete()
    {
        deletePanel.SetActive(false);
    }

    public void DeleteProfile(int sharedProfileID)
    {
        SharedDateIaProfile.profileID = sharedProfileID;

        SAI.SDK.iarequest.DeleteIAProfile();

        deletePanel.SetActive(false);

        WaitingPanel();

        ResetList();
    }

    public void ToggleDropdownIA(int profileId, string nameIA, string privacy, string memory, string baseModel, string prompt, string iaDescription, string intro_Message)
    {
        int checkPut = 1;

        SharedDateIaProfile.profileID = profileId;
        SharedDateIaProfile.profileName = nameIA;
        SharedDateIaProfile.checkPostPut = checkPut;

        Debug.Log("Memory: " + memory);

        //Call to populate the multipleChoice list with API values//

        List<string> options = scriptMultiple.selectedOptions;
        options.Clear();
        string[] memoryValues = memory.Split(',');
        options.AddRange(memoryValues);
        StartCoroutine(UpdateDropdown(options));

        ///////////////////////////////////////////////////////

        //Calls to retrieve the values that profiles already have and this 

        int privacylIndex = privacyDrop.options.FindIndex(option => option.text == privacy);
        privacyDrop.value = privacylIndex;
        privacyDrop.RefreshShownValue();

        int baseModelIndex = baseModelDrop.options.FindIndex(option => option.text.StartsWith(baseModel));
        baseModelDrop.value = baseModelIndex;
        baseModelDrop.RefreshShownValue();

        newNameIA.text = nameIA;
        promptInput.text = prompt;
        aiDescription.text = iaDescription;
        introMessage.text = intro_Message;

        //////////////////////////////////////////////////////////////

        dropdownContentIA.SetActive(true);
        panelIAConfiguration.SetActive(false);
    }

    public void CloseConfigureIA()
    {
        dropdownContentIA.SetActive(false);
        panelIAConfiguration.SetActive(true);
    }

    IEnumerator UpdateDropdown(List<string> options)
    {
        yield return null; 

        memoryDrop.text = string.Join(", ", options.ToArray());
    }
    public void ToggleDropdownMemory(int profileId)
    {
        //dropdownContentMemory.SetActive(true);
        isExpandedMemory = !isExpandedMemory;
        dropdownContentMemory.SetActive(isExpandedMemory);
        panelMemoryP = !panelMemoryP;
        panelMemory.SetActive(!panelMemoryP);
        panelChatP = !panelChatP;
        panelChat.SetActive(panelChatP);
    }
    public void NewToggleDropdownIA()
    {
        memorytype.value = 0;
        memorytype.onValueChanged.Invoke(memorytype.value);

        dropdownContentIA.SetActive(true);
        panelIAConfiguration.SetActive(false);

        privacyDrop.value = 0;
        baseModelDrop.value = 0;
        privacyDrop.RefreshShownValue();
        baseModelDrop.RefreshShownValue();

        newNameIA.text = "";
        promptInput.text = "";
        aiDescription.text = "";
        introMessage.text = "";

        //isExpandedIA = !isExpandedIA;
        //dropdownContentIA.SetActive(isExpandedIA);
        //newDropDown = !newDropDown;
        //dropdwonIaConfiguration.SetActive(newDropDown);
        //panelIA = !panelIA;
        //panelIAConfiguration.SetActive(!panelIA);
      
    }

    public void WaitingPanel()
    {
        StartCoroutine("waitingTime");
    }

    IEnumerator waitingTime()
    {
        waitingPanel.SetActive(true);
        yield return new WaitForSeconds(4f);
        waitingPanel.SetActive(false);
    }

    public void ButtonSave()
    {
        WaitingPanel();
        CloseConfigureIA();
        ResetList();
    }
    public void FaqS()
    {
        faqsButton.color = new Color(95f / 255f, 144f / 255f, 222f / 255f, 1f);
        memoryLinkButton.color = Color.black;
        uploadFileButton.color = Color.black;

        if (!faqs.activeInHierarchy)
        {
            faqs.SetActive(true);
            memoryLink.SetActive(false);
            uploadFile.SetActive(false);
        }
    }

    public void MemoryLink()
    {
        faqsButton.color = Color.black;
        memoryLinkButton.color = new Color(95f / 255f, 144f / 255f, 222f / 255f, 1f);
        uploadFileButton.color = Color.black;

        if (!memoryLink.activeInHierarchy)
        {
            faqs.SetActive(false);
            memoryLink.SetActive(true);
            uploadFile.SetActive(false);
        }
    }
    public void UploadFile()
    {
        faqsButton.color = Color.black;
        memoryLinkButton.color = Color.black;
        uploadFileButton.color = new Color(95f / 255f, 144f / 255f, 222f / 255f, 1f);

        if (!uploadFile.activeInHierarchy)
        {
            faqs.SetActive(false);
            memoryLink.SetActive(false);
            uploadFile.SetActive(true);
        }
    }

    public void DeleteContent()
    {
        foreach (Transform child in iaContent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void ResetList()
    {
        Invoke("IAConfigure", 3f);
        Invoke("NewIAConfiguration", 3f);
    }

    /// <summary>
    /// GetProfileList
    /// </summary>
    [System.Serializable]
    public class IAProfile
    {
        public int aiprofile_id;
        public string name;
        public Config config;
        public string prompt;
        public string ai_description;
        public string intro_message;
        public float temperature;
        public long training_time;
        public string owner;
        public string basemodel;
        public string privacy;
        public string[] memory;
    }

    [System.Serializable]
    public class Config
    {
        public string algo;
    }

    [System.Serializable]
    public class IAProfileList
    {
        public List<IAProfile> profiles;
        public int amount;
    }

    /// <summary>
    /// GetProfile
    /// </summary>
   

}
