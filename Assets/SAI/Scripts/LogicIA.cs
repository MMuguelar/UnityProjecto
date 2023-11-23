using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using Newtonsoft.Json;


public class LogicIA : MonoBehaviour
{
    public TMP_Dropdown baseModelDrop;
    public TMP_Dropdown privacyTypeDrop;
    public TMP_Dropdown memoryTypeDrop;
    public TMP_InputField newNameProfileIA;
    public TMP_InputField prompt;
    public TMP_InputField aiDescription;
    public TMP_InputField introMessage;
    public multipleChoiceDrop test;

    private List<Engine> engineList;
    private List<Memory> memoryList;
    private List<Privacy> privacyList;

    private string selectedPrivacyCode;
    private string selectedMemoryCode;
    private string selectedBaseModelCode;

    public void startRequestIA()
    {
        SAI.SDK.iarequest.GetBaseModel();
        SAI.SDK.iarequest.GetPrivacyType();
        SAI.SDK.iarequest.GetMemoryType();

        BaseModelDropDown();
        PrivacyDropDowon();
        MemoryDropDown();
    }

    public void BaseModelResponse(string jsonResponse)
    {
        engineList = JsonConvert.DeserializeObject<List<Engine>>(jsonResponse);
    }

    public void BaseModelDropDown()
    {
        foreach (Engine engine in engineList)
        {
            TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData();

            optionData.text = engine.name + "\n" + engine.description;

            string base64Image = System.Text.RegularExpressions.Regex.Replace(engine.image, "data:image/(png|jpeg);base64,", "");
            byte[] imageBytes = System.Convert.FromBase64String(base64Image);

            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageBytes);

            optionData.image = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

            baseModelDrop.options.Add(optionData);
        }
        baseModelDrop.value = 0;

        selectedBaseModelCode = engineList[0].code;

        baseModelDrop.onValueChanged.AddListener(CodeBaseModel);
    }

    public void CodeBaseModel(int index)
    {
        if (index >= 0 && index < engineList.Count)
        {
            selectedBaseModelCode = engineList[index].code;
        }
    }

    public void PrivacyTypeResponse(string jsonResponse)
    {
        privacyList = JsonConvert.DeserializeObject<List<Privacy>>(jsonResponse);
    }

    public void PrivacyDropDowon()
    {

        foreach (Privacy privacy in privacyList)
        {
            privacyTypeDrop.options.Add(new TMP_Dropdown.OptionData(privacy.name));
        }
        privacyTypeDrop.value = 0;

        selectedPrivacyCode = privacyList[0].code;

        privacyTypeDrop.onValueChanged.AddListener(OnPrivacyDropDownValueChanged);

    }

    public void OnPrivacyDropDownValueChanged(int index)
    {
        Debug.Log("Privacy Index: " + index);

        if (index >= 0 && index < privacyList.Count)
        {
            selectedPrivacyCode = privacyList[index].code;
        }
    }

    public void MemoryTypeResponse(string jsonResponse)
    {
        memoryList = JsonConvert.DeserializeObject<List<Memory>>(jsonResponse);
    }

    public void MemoryDropDown()
    {

        foreach (Memory memory in memoryList)
        {
            memoryTypeDrop.options.Add(new TMP_Dropdown.OptionData(memory.name));
          
        }

        multipleChoiceDrop dropdownMultiSelect = memoryTypeDrop.GetComponent<multipleChoiceDrop>();
        if (dropdownMultiSelect != null)
        {
            dropdownMultiSelect.enabled = true;
        }

        memoryTypeDrop.value = 0;

        selectedMemoryCode = memoryList[0].code;

    }

    public void Rtest()
    {
        Dictionary<string, string> memoryCodeDictionary = new Dictionary<string, string>();

        foreach (Memory memory in memoryList)
        {
            memoryCodeDictionary[memory.name] = memory.code;
        }

        List<string> options = test.selectedOptions;

        List<string> selectedCodes = new List<string>();

        foreach (string option in options)
        {
            if (memoryCodeDictionary.ContainsKey(option))
            {
                string code = memoryCodeDictionary[option];
                selectedCodes.Add(code);
                //selectedCodes.Add('"' + code + '"');
            }
        }

        //string codesString = "[" + string.Join(", ", selectedCodes) + "]";
        string codesString = string.Join(",", selectedCodes);


        selectedMemoryCode = codesString;

        test.selectedOptions.Clear();
    }
    public void SendRequestPutOrPost()
    {
        int check = SharedDateIaProfile.checkPostPut;

        Debug.Log("Que llega aqui: " + check);

        if (check == 1)
        {
            SendPutProfileIA();
        }
        else
        {

            NewProfileIA();
        }
    }
    public void NewProfileIA()
    {
        int temperature = 10;

        //if (string.IsNullOrEmpty(newNameProfileIA.text))
        //{
        //    Debug.LogError("you missed assigning the name of your AI");
        //}
        //else
        //{
        var infoIA = new
        {
            name = newNameProfileIA.text,
            config = new
            {
                algo = "algo2"
            },
            prompt = prompt.text,
            ai_description = aiDescription.text,
            intro_message = introMessage.text,
            temperature = temperature,
            basemodel_code = selectedBaseModelCode,
            memorytype_code = selectedMemoryCode,
            privacy_code = selectedPrivacyCode,

        };

        SAI.SDK.iarequest.AIProfile(infoIA, "POST");

        newNameProfileIA.text = "";

        ClearOptionsIAConfigure();
        //}

    }
    public void SendPutProfileIA()
    {
        int ProfileID = SharedDateIaProfile.profileID;
        string NewNameIA = newNameProfileIA.text;
        int temperature = 10;


        if (string.IsNullOrEmpty(NewNameIA))
        {
            NewNameIA = SharedDateIaProfile.profileName;

        }
        else
        {
            NewNameIA = newNameProfileIA.text;
        }

        var infoIA = new
        {
            name = NewNameIA,
            aiprofile_id = ProfileID,
            config = new
            {
                algo = "algo2"
            },
            prompt = prompt.text,
            ai_description = aiDescription.text,
            intro_message = introMessage.text,
            temperature = temperature,
            basemodel_code = selectedBaseModelCode,
            memorytype_code = selectedMemoryCode,
            privacy_code = selectedPrivacyCode,

        };

        Debug.Log("TestJson: " + infoIA);

        SAI.SDK.iarequest.AIProfile(infoIA, "PUT");

        NewNameIA = "";

        ClearOptionsIAConfigure();
    }

    private void ClearOptionsIAConfigure()
    {
        newNameProfileIA.text = "";
        prompt.text = "";
        aiDescription.text = "";
        introMessage.text = "";
    }

    public void IAProfileResponse(string jsonResponse)
    {
        AIProfile aiProfile = JsonConvert.DeserializeObject<AIProfile>(jsonResponse);

        if (aiProfile != null)
        {

            Debug.Log("aiprofile_id: " + aiProfile.aiprofile_id);
            Debug.Log("aiProfile.name: " + aiProfile.name);


            if (aiProfile.config != null)
            {
                Debug.Log("config.algo: " + aiProfile.config.algo);
            }

            Debug.Log("Prompt: " + aiProfile.prompt);
            Debug.Log("aiDescription: " + aiProfile.ai_description);
            Debug.Log("introMessage: " + aiProfile.intro_message);
            Debug.Log("privacy: " + aiProfile.privacy);
            Debug.Log("temperature: " + aiProfile.temperature);
            Debug.Log("owner: " + aiProfile.owner);
            Debug.Log("basemodel: " + aiProfile.basemodel);

            if (aiProfile.memory != null)
            {
                Debug.Log("memory:");
                foreach (string memoryItem in aiProfile.memory)
                {
                    Debug.Log(memoryItem);
                }
            }
        }
    }

    public void ButtonSave()
    {
        Rtest();
        //NewProfileIA();
        SendRequestPutOrPost();
    }

    [System.Serializable]
    public class EngineList
    {
        public List<Engine> IAmotors;
    }
    [System.Serializable]
    public class Engine
    {
        public string code;
        public string name;
        public string description;
        public string image;
    }

    [System.Serializable]

    public class MemoryType
    {
        public List<Memory> memoryType;
    }

    [System.Serializable]

    public class Memory
    {
        public string code;
        public string name;
        public string description;
    }

    [System.Serializable]

    public class PrivacyType
    {
        public List<Privacy> privacyType;
    }

    [System.Serializable]

    public class Privacy
    {
        public string code;
        public string name;
        public string description;
    }

    [System.Serializable]
    public class AIProfile
    {
        public int aiprofile_id;
        public string name;
        public Config config;
        public string prompt;
        public string ai_description;
        public string intro_message;
        public string privacy;
        public int temperature;
        public int training_time;
        public string owner;
        public string basemodel;
        public List<string> memory;
    }

    [System.Serializable]
    public class Config
    {
        public string algo;
    }


}
