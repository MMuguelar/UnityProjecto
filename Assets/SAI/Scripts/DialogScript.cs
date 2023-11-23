using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine.Networking;
using System.IO.Compression;

public class DialogScript : MonoBehaviour
{
    private Dictionary<string, string> fileMap = new Dictionary<string, string>();

    private Dictionary<string, string> fileTypeMap = new Dictionary<string, string>();

    private string filePath;

    private string URL = "http://dfs.sos.space:8085/ipfs/";

    public string fileListEndpoint = "/api/listuserfiles";

    [SerializeField]
    public GameObject dialog;

    public GameObject dialogTextGameObject;

    public GameObject tableContent;

    public GameObject loadingGameObject;

    public bool isLoading = false;

    public Image spiningWheel;

    public Button downloadButton;

    public TMP_Text debugText;

    //private static bool startExecuted = false;

    public void Start()
    {
        dialog.SetActive(false);
        loadingGameObject.SetActive(false);
    }
    public void OpenDialog(string childName)
    {
        dialog.SetActive(true);
        LoadDataIntoDialog(childName);
    }

    public void CloseDialog()
    {
        dialog.SetActive(false);
    }

    public void LoadDataIntoDialog(string childName)
    {
        string url = SAI.SDK.API.host + fileListEndpoint + "?owner_file=True&public_file=True";

        string response = SAI.SDK.API.GenericGetRequest(url);

        Data data = JsonConvert.DeserializeObject<Data>(response);

        foreach (FileInfo file in data.Files)
        {
            fileMap[file.name] = file.hash;
            fileTypeMap[file.name] = file.fileType;
            Debug.Log("file hash" + file.hash);
        }
        Debug.Log("TRANSFROM: " + transform);
        string hash = fileMap[childName];
        string currentFileExtension = fileTypeMap[childName];

        Debug.Log(string.Format("Hash for " + childName + hash));

        dialog.SetActive(true);

        LoadFile(URL + hash, currentFileExtension);


    }

    public void LoadFile(string url, string currentFileExtension)
    {


        switch (currentFileExtension.ToLower())
        {
            case ".txt":
                StartCoroutine(LoadTextCoroutine(url));
                break;

            case ".jpg":
            case ".jpeg":
            case ".png":
            case ".gif":
                //CHECK ABOUT THE .JSON FILE TYPE
            case ".json":
                StartCoroutine(LoadImageCoroutine(url));
                break;

            case ".mp4":
            case ".mov":
            case ".avi":
            case ".mkv":
                StartCoroutine(LoadVideoCoroutine(url));
                break;

            default:
                Debug.Log("Unsupported file type: " + currentFileExtension);
                break;
        }
    }

    //HACER LA FUNCION EN CASO DE QUE SERA DE TIPO VIDEO, TAMBIEN AÑADIR UNA PANTALLA DE CARGA Y ¡LO MAS IMPORTANTE! ===> HACER QUE EN LA DE LA IMAGEN SI LA RESPUESTA ES MALA POR PARTE DEL SERVER, VUELVA A INTENTAR

    private IEnumerator LoadTextCoroutine(string url)
    {
        dialogTextGameObject = dialog.transform.GetChild(1).GetChild(0).GetChild(1).gameObject;
        dialogTextGameObject.GetComponent<TMP_Text>().text = "";

        Image image = dialog.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>();
        image.sprite = null;

        // Load the text file from the URL
        UnityWebRequest textRequest = UnityWebRequest.Get(url);
        yield return textRequest.SendWebRequest();

        // Check if there were any errors
        if (textRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Failed to load text file: " + textRequest.error);
            yield break;
        }

        // Get the file contents from the response
        string fileContents = textRequest.downloadHandler.text;

        dialogTextGameObject.GetComponent<TMP_Text>().text = fileContents;

        downloadButton.onClick.AddListener(() => DownloadTextButtonClicked(url));
    }

    private IEnumerator LoadVideoCoroutine(string url)
    {
        dialogTextGameObject = dialog.transform.GetChild(1).GetChild(0).GetChild(1).gameObject;
        dialogTextGameObject.GetComponent<TMP_Text>().text = "";

        downloadButton.onClick.AddListener(() => DownloadVideoButtonClicked(url));

        yield break;
    }

    private IEnumerator LoadImageCoroutine(string url)
    {
        isLoading = true;
        loadingGameObject.SetActive(true);

        spiningWheel = loadingGameObject.GetComponent<Image>();

        StartCoroutine(SpinWheelRoutine());

        dialogTextGameObject = dialog.transform.GetChild(1).GetChild(0).GetChild(1).gameObject;
        dialogTextGameObject.GetComponent<TMP_Text>().text = "";

        Image image = dialog.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>();
        image.sprite = null;


        // Load the texture from the URL
        Debug.Log("IMAGE COROUTINE");

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        // Check if there were any errors
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Failed to load image: " + www.error);
            yield break;
        }

        // Get the texture from the response and assign it to an image component
        Texture2D texture = DownloadHandlerTexture.GetContent(www);
        image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        loadingGameObject.SetActive(false);
        isLoading = false;

        downloadButton.onClick.AddListener(() => DownloadImageButtonClicked(url));
    }

    private void DownloadVideoButtonClicked(string url)
    {
        StartCoroutine(DownloadVideo(url));
    }

    private void DownloadImageButtonClicked(string url)
    {
        StartCoroutine(DownloadImage(url));
    }
    private void DownloadTextButtonClicked(string url)
    {
        StartCoroutine(DownloadTextFile(url));
    }

    private IEnumerator DownloadVideo(string url)
    {
        isLoading = true;
        loadingGameObject.SetActive(true);

        spiningWheel = loadingGameObject.GetComponent<Image>();

        StartCoroutine(SpinWheelRoutine());

        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Failed to download video: " + www.error);
            yield break;
        }

        // Get the video data from the response
        byte[] compressedData = www.downloadHandler.data;

        // Decompress the video data
        byte[] videoData;
        using (MemoryStream compressedStream = new MemoryStream(compressedData))
        {
            using (MemoryStream decompressedStream = new MemoryStream())
            {
                using (GZipStream gzipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
                {
                    gzipStream.CopyTo(decompressedStream);
                }
                videoData = decompressedStream.ToArray();
            }
        }

        // Get the download folder path based on the platform
        string downloadFolderPath;
#if UNITY_ANDROID && !UNITY_EDITOR
        downloadFolderPath = "/storage/emulated/0/Download";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        downloadFolderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile) + "\\Downloads";
#elif UNITY_IOS || UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        downloadFolderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile) + "/Downloads";
#else
        // For other platforms, save in the persistent data path
        downloadFolderPath = Application.persistentDataPath;
#endif

        // Save the video file to the download folder
        string fileName = "video.mp4";
        string filePath = Path.Combine(downloadFolderPath, fileName);
        File.WriteAllBytes(filePath, videoData);

        loadingGameObject.SetActive(false);
        isLoading = false;
    }

    private IEnumerator DownloadImage(string url)
    {
        isLoading = true;
        loadingGameObject.SetActive(true);

        spiningWheel = loadingGameObject.GetComponent<Image>();

        StartCoroutine(SpinWheelRoutine());

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Failed to download image: " + www.error);
            debugText.text = www.error;
            yield break;
        }

        // Get the downloaded texture
        Texture2D texture = DownloadHandlerTexture.GetContent(www);

        // Convert texture to bytes
        byte[] imageData = texture.EncodeToPNG();

        // Get the download folder path based on the platform
        string downloadFolderPath;
#if UNITY_ANDROID && !UNITY_EDITOR
    downloadFolderPath = "/storage/emulated/0/Download";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        downloadFolderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile) + "\\Downloads";
#elif UNITY_IOS || UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
    downloadFolderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile) + "/Downloads";
#else
    // For other platforms, save in the persistent data path
    downloadFolderPath = Application.persistentDataPath;
#endif

        // Save the image file to the download folder
        string fileName = "image.png";
        string filePath = Path.Combine(downloadFolderPath, fileName);
        File.WriteAllBytes(filePath, imageData);
        debugText.text = filePath;
        loadingGameObject.SetActive(false);
        isLoading = false;
    }

    private IEnumerator DownloadTextFile(string url)
    {
        isLoading = true;
        loadingGameObject.SetActive(true);

        spiningWheel = loadingGameObject.GetComponent<Image>();

        StartCoroutine(SpinWheelRoutine());

        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Failed to download text file: " + www.error);
            yield break;
        }

        // Get the downloaded text
        string textData = www.downloadHandler.text;

        // Get the download folder path based on the platform
        string downloadFolderPath;
#if UNITY_ANDROID && !UNITY_EDITOR
    downloadFolderPath = "/storage/emulated/0/Download";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        downloadFolderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile) + "\\Downloads";
#elif UNITY_IOS || UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
    downloadFolderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile) + "/Downloads";
#else
    // For other platforms, save in the persistent data path
    downloadFolderPath = Application.persistentDataPath;
#endif

        // Save the text file to the download folder
        string fileName = "textfile.txt";
        string filePath = Path.Combine(downloadFolderPath, fileName);
        File.WriteAllText(filePath, textData);

        loadingGameObject.SetActive(false);
        isLoading = false;
    }

    public IEnumerator SpinWheelRoutine()
    {
        while (isLoading)
        {
            spiningWheel.transform.Rotate(0, 0, -1);
            yield return null;
        }
    }

    public class Data
    {
        public FileInfo[] Files { get; set; }
    }

    public class FileInfo
    {
        public string name { get; set; }
        public string hash { get; set; }
        public string fileType { get; set; }
    }
}
