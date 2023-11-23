
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClusterTransactionPG415 : MonoBehaviour
{
    [Header("Endpoint")]
    public string GetClusterTransactionList = "/api/getclustertransactionslist";

    public Transacciones currentTransactions;       




    public Transacciones GetTransactionList(string cluster_mac)
    {

        string ClusterTransactionEndpoint = SAI.SDK.API.host + GetClusterTransactionList;
        ClusterTransactionEndpoint += "?cluster_mac=" + cluster_mac;
        string response = SAI.SDK.API.GenericGetRequest(ClusterTransactionEndpoint);
        if (response != string.Empty)
        {
            Transacciones trans = new Transacciones();
            trans = JsonUtility.FromJson<Transacciones>(response);
            currentTransactions = trans;
        }
        else
        {
            return null;
        }

        return currentTransactions;
    }



    
    public void BringEndpointInfo(string cluster)
    {
       
    }


}


[System.Serializable]
public class Transacciones
{
    public List<transactions> transactions;

}

[System.Serializable]
public class transactions
{
    public string description;
    public float total;
    public string username;
    public string date;
}


