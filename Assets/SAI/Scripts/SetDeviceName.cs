using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class SetDeviceName : MonoBehaviour
{
    public TMP_Text texto;
    // Start is called before the first frame update
    void Start()
    {
        string devicename = SAI.SDK.Devices.GetDeviceName();
        string deviceso = SAI.SDK.Devices.GetDeviceSO();
        texto.text = $"New Device {devicename} detected. \n Running on {deviceso}\n";
    }



    
}
