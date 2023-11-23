
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geolocalization : MonoBehaviour
{
    public string ip;
    public string url;

    
    public void GetInfo()
    {
       
        print(SAI.SDK.API.GenericGetRequest(url));
        

    }
}
