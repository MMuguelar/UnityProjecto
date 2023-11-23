using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json.Linq;

public class FilesTableLogic : MonoBehaviour
{
    private string jsonFilePath;

    public GameObject filesTableContent;


    private void Start()
    {
        SAI.SDK.Storage.CheckJsonFileExists();

        InvokeRepeating("FillFilesTable", 0f, 2f);
    }

    private void FillFilesTable()
    {
        SAI.SDK.Storage.FillFilesTable(filesTableContent);
    }

}
