using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Threading.Tasks;
using Ipfs.Http;
using TMPro;
using SFB;
using System.IO.Compression;
using SimpleJSON;
using Newtonsoft.Json.Linq;
using System;

[System.Serializable]
public class FileEntry
{
    public string name;
    public string hash;
    public string fileType;
    public string weight;
    public string date;
    public string privacy_code;
}

[System.Serializable]
public class FilesData
{
    public List<FileEntry> Files = new List<FileEntry>();
}

public class Storage : MonoBehaviour
{
    [Header("Endpoints")]
    public string IPFS_URL = " http://dfs.sos.space:8085/ipfs/";
    public string fileEndpoint = "/api/file";
    public string fileListEndpoint = "/api/listuserfiles";

    [SerializeField]
    private GameObject filePrefab;

    private string jsonFilePath;

    public void LoadFile()
    {
        print("LOAD FILE");
        // Define the file name and file path
        string fileName = "JSONHashes.json";
        string filePath = "";

#if UNITY_EDITOR || UNITY_STANDALONE
        filePath = Path.Combine(Application.persistentDataPath, fileName);
        var paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false);

        Debug.Log(paths.Length);
        if (paths.Length == 1)
        {
            Debug.Log(paths[0]);
            UploadFile(paths[0], filePath);
        }
#elif UNITY_ANDROID
        filePath = Path.Combine(Application.persistentDataPath, fileName);

        // Check if JSON file exists
        if (!File.Exists(filePath))
        {
            Debug.Log("THE JSON FILE DOES NOT EXIST!");
        }

        string[] mimeTypes = { "image/*", "audio/*", "video/*", "text/*", "application/*" };
        NativeFilePicker.Permission permission = NativeFilePicker.PickFile((path) =>
        {
            if (path == null){
                Debug.Log("Operation Cancelled");
            }
            else
            {
                finalPath = path;
                Debug.Log("Picked File: " + finalPath);
                UploadFile(finalPath, filePath);
            }
        }, mimeTypes);
#elif UNITY_IOS || UNITY_IPHONE || UNITY_WEBGL
        filePath = Path.Combine(Application.persistentDataPath, "JSON", fileName);
#else
        filePath = Path.Combine(Application.dataPath, "JSON", fileName);
#endif


    }

    private async Task UploadFile(string path, string jsonFilePath)
    {
        // Check if the file is a video
        string extension = Path.GetExtension(path);
        Debug.Log("EXTENSION: " + extension);
        bool isVideo = IsVideoFile(extension);

        if (isVideo)
        {
            await UploadCompressedVideo(path, jsonFilePath, extension);
        }
        else
        {
            await UploadRegularFile(path, jsonFilePath, extension);
        }
    }
    private bool IsVideoFile(string extension)
    {
        // Convert the extension to lowercase for case-insensitive comparison
        extension = extension.ToLower();

        // Check if the extension corresponds to a video file
        // Modify this logic based on your specific requirements
        if (extension == ".mp4" || extension == ".avi" || extension == ".mov" || extension == ".mkv" || extension == ".wmv" || extension == ".wav")
        {
            return true;
        }

        return false;
    }


    private async Task UploadRegularFile(string path, string jsonFilePath, string extension)
    {
        // Load the file into a byte array
        byte[] fileData = File.ReadAllBytes(path);

        // Create a new IpfsHttpClient object to interact with the IPFS node
        var ipfs = new IpfsClient(IPFS_URL);

        try
        {
            // Upload the file to IPFS
            var file = await ipfs.FileSystem.AddAsync(new MemoryStream(fileData));

            // Create a new FileEntry object
            FileEntry fileEntry = new FileEntry
            {
                name = Path.GetFileName(path),
                hash = file.Id.Hash.ToString(),
                fileType = extension,
                weight = fileData.Length.ToString(),
                date = DateTime.Now.ToString("yyyy-MM-dd"),
                privacy_code = "P3"
            };

            // Convert the FileEntry object to JSON
            string fileEntryJson = JsonUtility.ToJson(fileEntry);

            addFile(fileEntryJson);

            Debug.Log("IPFS URL: " + file.Id);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error uploading file: " + e.Message);
        }
    }

    // Deserialize JSON data from the file
    private FilesData ReadFilesData(string filePath)
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            if (!string.IsNullOrEmpty(json))
            {
                return JsonUtility.FromJson<FilesData>(json);
            }
        }

        return new FilesData();
    }

    // Serialize and write JSON data to the file
    private void WriteFilesData(string filePath, FilesData filesData)
    {
        string json = JsonUtility.ToJson(filesData);
        File.WriteAllText(filePath, json);
    }

    private async Task UploadCompressedVideo(string path, string jsonFilePath, string extension)
    {
        byte[] fileData = File.ReadAllBytes(path);

        // Create a new IPFS client
        var ipfs = new IpfsClient(IPFS_URL);

        try
        {
            // Read the video file into a byte array
            byte[] videoData = File.ReadAllBytes(path);

            // Compress the video data using GZip compression
            byte[] compressedData;
            using (var outputStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(outputStream, CompressionMode.Compress))
                {
                    gzipStream.Write(videoData, 0, videoData.Length);
                }
                compressedData = outputStream.ToArray();
            }

            // Upload the compressed video to IPFS
            var file = await ipfs.FileSystem.AddAsync(new MemoryStream(compressedData));

            // Create a new FileEntry object
            FileEntry fileEntry = new FileEntry
            {
                name = Path.GetFileName(path),
                hash = file.Id.Hash.ToString(),
                fileType = extension,
                weight = fileData.Length.ToString(),
                date = DateTime.Now.ToString("yyyy-MM-dd"),
                privacy_code = "P3"
            };

            // Convert the FileEntry object to JSON
            string fileEntryJson = JsonUtility.ToJson(fileEntry);

            addFile(fileEntryJson);

            Debug.Log("IPFS URL: " + file.Id);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error uploading file: " + e.Message);
        }
    }



    IEnumerator LoadStreamingAsset(string path, System.Action<string> callback)
    {
        string loadedText;
        string fullPath = Path.Combine(Application.streamingAssetsPath, path);

        if (Application.platform == RuntimePlatform.Android)
        {
            // Android requires the use of the WWW class to load files from StreamingAssets
            using (WWW www = new WWW(fullPath))
            {
                yield return www;
                loadedText = www.text;
            }
        }
        else
        {
            loadedText = File.ReadAllText(fullPath);
        }
        callback?.Invoke(loadedText);
    }

    public void CheckJsonFileExists()
    {
        string fileName = "JSONHashes.json";

#if UNITY_EDITOR || UNITY_STANDALONE
        jsonFilePath = Path.Combine(Application.persistentDataPath, fileName);
#elif UNITY_ANDROID
        jsonFilePath = Path.Combine(Application.persistentDataPath, fileName);
#elif UNITY_IOS || UNITY_IPHONE || UNITY_WEBGL
        jsonFilePath = Path.Combine(Application.persistentDataPath, "JSON", fileName);
#else
        jsonFilePath = Path.Combine(Application.dataPath, "JSON", fileName);
#endif

        if (!File.Exists(jsonFilePath))
        {
            Debug.Log("JSON file does not exist. Creating a new one...");
            CreateNewJsonFile();
        }
        else
        {
            Debug.Log("JSON EXISTS!!!!!");
        }
    }

    public void CreateNewJsonFile()
    {
        File.WriteAllText(jsonFilePath, "{}");
        Debug.Log("Created JSON at: " + jsonFilePath);
    }

    public string LoadStreamingAsset(string path)
    {
        string loadedText = "";

        string fileName = "JSONHashes.json";

#if UNITY_EDITOR || UNITY_STANDALONE
        jsonFilePath = Path.Combine(Application.persistentDataPath, fileName);
#elif UNITY_ANDROID
        jsonFilePath = Path.Combine(Application.persistentDataPath, fileName);
#elif UNITY_IOS || UNITY_IPHONE || UNITY_WEBGL
        jsonFilePath = Path.Combine(Application.persistentDataPath, "JSON", fileName);
#else
        jsonFilePath = Path.Combine(Application.dataPath, "JSON", fileName);
#endif

        if (File.Exists(jsonFilePath))
        {
            loadedText = File.ReadAllText(jsonFilePath);
        }
        else
        {
            Debug.LogError("Could not find JSON file: " + jsonFilePath);
        }

        return loadedText;
    }

    public void FillFilesTable(GameObject filesTableContent)
    {
        //THIS LINE WILL BE CHANGED WITH FILTERS
        string url = SAI.SDK.API.host + fileListEndpoint + "?owner_file=True&public_file=True";

        string response = SAI.SDK.API.GenericGetRequest(url);

        print("FillFiles RESPONSE: " + response);

        if (string.IsNullOrEmpty(response)) return;

        foreach (Transform child in filesTableContent.transform)
        {
            Destroy(child.gameObject);
        }

        // Parse the JSON file into a JObject
        JObject jsonObject = JObject.Parse(response);

        print("FillFiles JSON: " + response);

        // Get the "Files" property as a JArray
        JArray filesArray = (JArray)jsonObject["files"];

        // Get the number of files in the array
        int numFiles = filesArray.Count;

        //Debug.Log("Number of files: " + numFiles);

        // Loop through the files in the array
        foreach (JObject fileObject in filesArray)
        {
            // Get the name and hash properties
            string fileName = (string)fileObject["name"];
            string fileHash = (string)fileObject["hash"];

            print("File Name: " + fileName + " File Hash: " + fileHash);

            GameObject g = Instantiate(filePrefab, filesTableContent.transform);
            g.transform.GetChild(0).GetChild(2).name = fileName;

            //Button button = g.transform.GetChild(0).GetComponent<Button>();

            //Button.ButtonClickedEvent newOnClickEvent = new Button.ButtonClickedEvent();
            //button.onClick = newOnClickEvent;

            //newOnClickEvent.AddListener(ListenerFunction(childName));

            if (fileName.Length > 15)
            {
                string newName = fileName.Substring(0, 9) + ".." + fileName.Substring(fileName.LastIndexOf('.'));
                fileName = newName;
            }

            g.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = fileName;

            //Debug.Log("Name: " + fileName + ", Hash: " + fileHash);
        }
    }

    void addFile(string fileData)
    {
        string url = SAI.SDK.API.host + fileEndpoint;

        string response = SAI.SDK.API.GenericPostRequest(url, fileData);

        print(response);
    }
}
