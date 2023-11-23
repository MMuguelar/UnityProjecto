using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMPro;

public class JSONAndroidTest : MonoBehaviour
{
    private string jsonFilePath;
    public TMP_Text text;

    private void Awake()
    {
        jsonFilePath = Application.persistentDataPath + "/data.json";

        if (!File.Exists(jsonFilePath))
        {
            CreateJSONFile();

        }
        else
        {
            text.text = "JSON EXISTS!!!!!";
            AddToJSON();
        }
    }

    private void CreateJSONFile()
    {
        Debug.Log("Creating JSON file...");
        text.text += "Creating JSON file...";
        File.WriteAllText(jsonFilePath, "{}");
        AddToJSON();
    }

    private void AddToJSON()
    {
        Debug.Log("Adding to JSON...");
        string jsonContent = File.ReadAllText(jsonFilePath);
        JObject json = JsonConvert.DeserializeObject<JObject>(jsonContent);
        json.Add("written", true);
        File.WriteAllText(jsonFilePath, JsonConvert.SerializeObject(json));
        text.text += "JSON content after: " + JsonConvert.SerializeObject(json);
    }

}
