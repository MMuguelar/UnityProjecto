using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    public string adress;
    public string name;
    public string availability;
    public from from;
    public position position;
    public giving giving;
    public taking taking;
    public string velocity;
    public string services;
    public string lastHash;
    public string connection;
    public string registration;
    public bool awaitingResponse;
    public bool status;
}

[System.Serializable]
public class from
{
    public string name;
    public string mag;
    public string dist;
}

[System.Serializable]
public class position
{
    public string altitude;
    public string lat;
    public string @long;
}

[System.Serializable]
public class giving
{
    public string S;
    public string P;
    public string C;
    public string W;
}

[System.Serializable]
public class taking
{
    public string S;
    public string P;
    public string C;
    public string W;
}

[System.Serializable]
public class voltage
{
    public string SEN;
    public string CS;
    public string VS100;
    public string VS3;
    public string VV;
}

[System.Serializable]
public class NodeResponse
{
    public Node node;
    public string mqtt;
    public bool awaitingResponse;
    public bool status;
}