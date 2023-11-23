using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OnlineNodesClass
{
    public string adress;
    public string name;

    public OnlineNodesClass(string _adress, string _name)
    {
        adress = _adress;
        name = _name;
    }
}