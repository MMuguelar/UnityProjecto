using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System;

public class NodeHQElement : MonoBehaviour
{
    public int nodeCode = 1;
    public int nodeQuantity = 1;
    public string nodeName = string.Empty;
    public Toggle toggle;

    public TMP_Text inputField;

    private void Update()
    {
        nodeQuantity = int.Parse(inputField.text);
    }

}
