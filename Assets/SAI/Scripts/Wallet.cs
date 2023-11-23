
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    [Header("Wallet Endpoints")]
    public string _GetWalletEndpoint = "/api/getwallet/";
    public string GetWalletEndpoint { get { return SAI.SDK.API.host + _GetWalletEndpoint; } }


    [Header("Wallet Data")]
    public string userName;
    public string userCredit;

    public ClusterTransactionPG415 transactions;

    public Transacciones transactionList = new Transacciones();

    
    public void RefreshWallet()
    {
       string response = SAI.SDK.API.GenericGetRequest(GetWalletEndpoint);
       ParseGetWalletDetail(response);

    }

    private void ParseGetWalletDetail(string response)
    {
        if (response == string.Empty) return;
        ApiResponse_GetWalletDetail wallet = JsonUtility.FromJson<ApiResponse_GetWalletDetail>(response);
        this.userName = wallet.username;
        this.userCredit = wallet.total.ToString();        
    }

    public class ApiResponse_GetWalletDetail
    {
        public string username;
        public float total;
        public List<Device> devices;
        public List<HistoryData> history;
    }

    
}
