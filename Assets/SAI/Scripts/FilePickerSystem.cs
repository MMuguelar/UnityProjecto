using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Threading.Tasks;
using Ipfs.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMPro;
using SFB;
using System.IO.Compression;


public class FilePickerSystem : MonoBehaviour
{

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

    public void LoadFile()
    {
        print("LOAD FILE 1");
        SAI.SDK.Storage.LoadFile();
    }
}