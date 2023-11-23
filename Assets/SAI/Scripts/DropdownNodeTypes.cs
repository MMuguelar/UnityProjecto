using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;


public class DropdownNodeTypes : MonoBehaviour
{

    public TMP_Dropdown dropdw;

    private void Start()
    {
        if(dropdw == null) dropdw = GetComponent<TMP_Dropdown>();

        //callGetNodeTypes(); haarcoded bynow
        
        List<TMP_Dropdown.OptionData> nodes = new List<TMP_Dropdown.OptionData>();
        nodes.Add(new TMP_Dropdown.OptionData("Desktop Computer"));
        nodes.Add(new TMP_Dropdown.OptionData("Android Device"));
        nodes.Add(new TMP_Dropdown.OptionData("Tablet"));  
        nodes.Add(new TMP_Dropdown.OptionData("Apple Device"));
        loadOptionsIntoDD(nodes);

    }

    private void callGetNodeTypes()
    {
        // Hacer llamado a la URL
        string url = SAI.SDK.API.host + SAI.SDK.Nodes.NodeType;//SAI.SDK.API.rNodeType;
        string response = SAI.SDK.API.GenericGetRequest(url);
        if(response != string.Empty)
        {
            print("NodeType : " + response);
        }
    }


    private void loadOptionsIntoDD(List<TMP_Dropdown.OptionData> options)
    {
        dropdw.ClearOptions();
        dropdw.AddOptions(options);
    }

}
