using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ipfs.Http;
using System.IO;
using System.Threading.Tasks;

public class IPFSFileSelect : MonoBehaviour
{
    // IPFS HTTP API URL
    private const string IPFS_URL = " http://dfs.sos.space:8085/ipfs/";

    // Path to the file you want to upload
    private string filePath;

    //public TextAsset JSONHashes;

    // Use this for initialization
    public async void OnButtonClick()
    {
        await UploadFile();
    }

    private async Task UploadFile()
    {
        filePath = OpenFileBrowser();
        // Load the file into a byte array
        byte[] fileData = File.ReadAllBytes(filePath);

        // Create a new IpfsHttpClient object to interact with the IPFS node
        var ipfs = new IpfsClient(IPFS_URL);

        try
        {
            // Upload the file to IPFS
            var file = await ipfs.FileSystem.AddAsync(new MemoryStream(fileData));

            // Print the IPFS URL for the uploaded file
            Debug.Log("IPFS URL: " + file.Id);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error uploading file: " + e.Message);
        }
    }

    public string OpenFileBrowser()
    {
        string filePath = UnityEditor.EditorUtility.OpenFilePanel("Select a file", "", "");

        if (filePath.Length != 0)
        {
            // Replace the following line with your existing code that receives the file.
            Debug.Log("Selected file: " + filePath);
        }

        return filePath;
    }
}