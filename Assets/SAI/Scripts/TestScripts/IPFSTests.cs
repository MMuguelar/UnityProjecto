//using System;
//using System.Text;
//using System.Threading.Tasks;
//using Ipfs.Http.Client;
//using Newtonsoft.Json;
//using UnityEngine;

//public class IPFSTests : MonoBehaviour
//{
//    private async void Start()
//    {
//        // JSON data to be uploaded to IPFS
//        var data = new { key = "value" };

//        // Convert the data to a JSON string
//        string jsonString = JsonConvert.SerializeObject(data);

//        // Create a new instance of the IPFS client
//        IpfsClient client = new IpfsClient();

//        // Add the JSON data to IPFS
//        var result = await client.FileSystem.AddTextAsync(jsonString, "json", Encoding.UTF8);

//        // Print the resulting IPFS hash
//        Debug.Log(result.Id);
//    }
//}