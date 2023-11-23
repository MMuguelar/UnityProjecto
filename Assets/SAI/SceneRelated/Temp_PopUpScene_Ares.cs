using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;



public class Temp_PopUpScene_Ares : MonoBehaviour
{

    [Header("DataHistory")]

    public TMP_Text userName;
    public TMP_Text totalBalance;   

    public GameObject PopUp_device_Panel_Prefab;
    public Transform DevicesContainer;

    public GameObject HistoryData_Prefab;
    public Transform HistoryData_Container;

    public List<HistoryData> historyDatas = new List<HistoryData>();

    public bool isDataLoaded = false;

    [Header("MessageBoxes")]
    public GameObject MessagePanelPrefab;
    public GameObject MessagePanelContainer;
    public TMP_Text msgCount;
    public List<GameObject> messages = new List<GameObject>();

    [Header("NotificationBoxes")]
    public GameObject NotificationPanelContainer;
    public TMP_Text notyCount;
    public List<GameObject> notifications = new List<GameObject>();

    [Header("AddNode")]  
    public TMP_Text macID;
    public TMP_Text nodeName;
    public TMP_Dropdown nodetype;
    public TMP_Dropdown cluster;

    [Header("userDisplayPopup")]
    public GameObject userdisplayPopup;

    [Header("Cluster")]
    public TMP_Dropdown mainClusterDropDown;
    public TMP_Dropdown postClusterDropDown;

    [Header("IA Setup")]
    public TMP_Dropdown yourClusterDropDown;
    public Transform DevicesContainerIA;

    public List<userCluster> userClusterList = new List<userCluster>();

    public List<Cluster> localClusterList = new List<Cluster>();
    public List<NodeData> localNodesList = new List<NodeData>();

    public UserNodeData userNodeData = new();

    public NodeTypes _nodeTypes;

  

    
    public void addNodeNew()// new
    {
        localClusterList = SAI.SDK.Clusters.GetClusterList();

        int index = postClusterDropDown.value;
        string mac = localClusterList[index].cluster_mac;

        string latitude = SAI.SDK.locationManager.latitude;
        string longitude = SAI.SDK.locationManager.longitude;
        

        if (SAI.SDK.Nodes.AddNode(nodeName.text, macID.text,
            valorParaEnviar, DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),"0", longitude, latitude, 1,mac))
        {
            print("NODO AGREGADO");
            SAI.SDK.errorHandler.ShowPopup($"Node {nodeName.text} added.", "CONGRATULATIONS");
            
        }
        else
        {
            print("FALLO AL AGREGAR EL NODO");
        }        

    }

    public void SwitchCluster(int id)
    {
        // esta funcion se llama cada vez que el usuario cambia de cluster en el dropdown postClusterDropdown
        print($"NEW * Switching Cluster to {id}");

        // le setea a clusterTransaction la mac para que el use como endpoint (esto no va)
        //SAI.SDK.clusterTransaction.BringEndpointInfo(localClusterList[id].cluster_mac);       

       
        CleanNodeContainerList(); // LimpiaLaUIDeNodos

        // Actualiza los nodos y espera un callback
        //SAI.subsystems.userInfo.UpdateNodes(localClusterList[id]); // sigue en NodeFinishCallback

        localNodesList = SAI.SDK.Nodes.GetNodeByID(id);
        if(localNodesList.Count != 0)
        {
            for (int i = 0; i < localNodesList.Count; i++)
            {
                // Crea el Prefab 
                GameObject go = Instantiate(PopUp_device_Panel_Prefab, DevicesContainer);
                // Lo coloca en el UI
                go.GetComponent<PopUpScene_Panel_Device>().name.text = localNodesList[i].node_name;
                go.GetComponent<PopUpScene_Panel_Device>().description.text = localNodesList[i].nodetype_description;

            }
        }

        LoadTransactions();




    }


    public string valorParaEnviar = "";
    public void OnDropdownValueChanged(int index)
    {
        // Obtener el valor del enum correspondiente al �ndice seleccionado
        _nodeTypes = (NodeTypes)index;

        // Convertir el valor del enum a cadena y enviarlo a la API
        valorParaEnviar = _nodeTypes.ToString();
        print("EL NODETYPE ES : " + valorParaEnviar);
        print("Dropdown value of Node type to send : " + _nodeTypes.ToString());


    }
    public void LoadClustersIntoDropdown()
    {
        if (localClusterList.Count > 0)
        {
            for (int i = 0; i < localClusterList.Count; i++)
            {
                TMP_Dropdown.OptionData ddata = new();
                ddata.text = localClusterList[i].cluster_name;
                List<TMP_Dropdown.OptionData> ddatalist = new();
                ddatalist.Add(ddata);
                mainClusterDropDown.AddOptions(ddatalist);
                postClusterDropDown.AddOptions(ddatalist);
                yourClusterDropDown.AddOptions(ddatalist);
                print("Cluster agregado a la dropdown");

            }
            SAI.SDK.clusterTransaction.BringEndpointInfo(localClusterList[0].cluster_mac);
            LoadNodesIfAny();
        }
        else
        {
          
            localClusterList = SAI.SDK.Clusters.GetClusterList();

        }

    }
    public void LoadNodesIfAny()
    {
        print("Loading Nodes if Any..");
        CleanNodeContainerList();
        LoadClusterAndNodes();

        localNodesList = SAI.SDK.Nodes.GetNodeList();

        if (localNodesList.Count == 0)
        {
            print("LA NODE LIST . COUNT ES : " + localNodesList.Count);
            // Aca esta el error
            //SAI.subsystems.userInfo.UpdateNodes(localClusterList[0]);
            //Invoke("LoadNodesIfAny", 2f);
            CleanNodeContainerList();
        }
        

        for (int i = 0; i < localNodesList.Count; i++)
        {
            // Crea el Prefab 
            GameObject go = Instantiate(PopUp_device_Panel_Prefab, DevicesContainer);
            GameObject go2 = Instantiate(PopUp_device_Panel_Prefab, DevicesContainerIA);
            // Lo coloca en el UI
            go.GetComponent<PopUpScene_Panel_Device>().name.text = localNodesList[i].node_name;
            go.GetComponent<PopUpScene_Panel_Device>().description.text = localNodesList[i].nodetype_description;

            go2.GetComponent<PopUpScene_Panel_Device>().name.text = localNodesList[i].node_name;
            go2.GetComponent<PopUpScene_Panel_Device>().description.text = localNodesList[i].nodetype_description;

        }
    }
    public enum NodeTypes
    {
        BC,
        CO,
        SDR,
        FULL
    }    
    private void Start()
    {
        CleanNodeContainerList();
        // Agregar un listener al Dropdown para manejar cambios en la selecci�n
        postClusterDropDown.onValueChanged.AddListener(SwitchCluster);

        // Llamar a la funci�n inicialmente para manejar el valor predeterminado
        OnDropdownValueChanged(postClusterDropDown.value);

        if (localClusterList.Count == 0)
        {
            print("TMPAres: Cargando Nodos y Clusters");
            localClusterList = SAI.SDK.Clusters.GetClusterList();
            localNodesList = SAI.SDK.Nodes.GetNodeList();

            if (localClusterList.Count != 0) print("Clusters Cargados OK , TOTAL: " + localClusterList.Count);
            if (localNodesList.Count != 0) print("Nodos Cargados OK, TOTAL: " + localNodesList.Count);
        }


        print("Loading Wallet");
       
            string host = SAI.SDK.Wallet.GetWalletEndpoint;
        string response = SAI.SDK.API.GenericGetRequest(host);
           if (response!= string.Empty)
        {
            // Analizar la cadena JSON
            ApiResponse responseData = JsonUtility.FromJson<ApiResponse>(response);
            // Acceder a los valores
            userName.text = responseData.username;
            totalBalance.text = responseData.total.ToString();
            SAI.SDK.Wallet.userName = responseData.username;
            SAI.SDK.Wallet.userCredit = responseData.total.ToString();

        }
        else
        {
            print("Response Error Hardcoding Data");
            userName.text = "DC ERROR";
            totalBalance.text = "98";
        }

        // Cargando Transacciones
       


        // Agrega los mensajes
        for (int i = 0; i < 8; i++)
        {
            print("Adding hardcoded msg");
            GameObject go = Instantiate(MessagePanelPrefab, MessagePanelContainer.transform);
            go.GetComponent<MessagePanelPopUpInfo>().MostrarMensaje($"Test {i}", "This is an example message.");
            messages.Add(go);

        }

        // Agrega las notificaciones
        for (int i = 0; i < 3; i++)
        {
            print("Adding hardcoded notifications");
            GameObject go = Instantiate(MessagePanelPrefab, NotificationPanelContainer.transform);
            go.GetComponent<MessagePanelPopUpInfo>().MostrarMensaje($"Test {i}", "This is an example notification.");
            notifications.Add(go);

        }

        msgCount.text = messages.Count.ToString();
        notyCount.text = notifications.Count.ToString();
           

            //Load Clusters into Dropdown
            
            
            LoadClustersIntoDropdown();
        //LoadNodesIfAny();

        LoadTransactions();

    }

    private void LoadTransactions()
    {
        print("Actualizando Lista de Transacciones");
        SAI.SDK.Util.DestroyChilders(HistoryData_Container.gameObject);

        print("Error aca");
        // Transacciones trans = SAI.SDK.Wallet.Transactions.GetTransactionList(localClusterList[mainClusterDropDown.value].cluster_mac);

        localClusterList = SAI.SDK.Clusters.GetClusterList();
        print("Solicitando Transaccion : " + localClusterList[mainClusterDropDown.value].cluster_mac);
        Transacciones trans = SAI.SDK.Wallet.transactions.GetTransactionList(localClusterList[mainClusterDropDown.value].cluster_mac);

        if (trans.transactions.Count > 0)
        {
            for (int i = 0; i < trans.transactions.Count; i++)
            {

                GameObject go = Instantiate(HistoryData_Prefab, HistoryData_Container);
                // Lo coloca en el UI
                TransactionsDisplayHQ displayHQ = go.GetComponent<TransactionsDisplayHQ>();
                displayHQ.m_OwnerNameText.text = trans.transactions[i].username;
                displayHQ.m_TittleText.text = trans.transactions[i].description;
                displayHQ.m_DateText.text = trans.transactions[i].date;
                displayHQ.m_AmountText.text = trans.transactions[i].total.ToString();
            }
        }
        else
        {
            print("ERROR");
        }
    }

    private void CleanNodeContainerList()
    {

        SAI.SDK.Util.DestroyChilders(DevicesContainer.gameObject);
        
    }
    private void LoadClusterAndNodes()
    {
        print("TMPAres : Loading Clusters and Nodes");
        localClusterList = SAI.SDK.Clusters.GetClusterList();
        localNodesList = SAI.SDK.Nodes.GetNodeList();
    }   
   
    private void TransactionListUpdate()
    {
        print("Actualizando Lista de Transacciones");
        SAI.SDK.Util.DestroyChilders(HistoryData_Container.gameObject);

        Transacciones trans = SAI.SDK.clusterTransaction.currentTransactions;
        print("Transacciones a a�adir : " + trans.transactions.Count);

        if (trans.transactions.Count > 0)
        {
            for (int i = 0; i < trans.transactions.Count; i++)
            {

                GameObject go = Instantiate(HistoryData_Prefab, HistoryData_Container);
                // Lo coloca en el UI
                TransactionsDisplayHQ displayHQ = go.GetComponent<TransactionsDisplayHQ>();
                displayHQ.m_OwnerNameText.text = trans.transactions[i].username;
                displayHQ.m_TittleText.text = trans.transactions[i].description;
                displayHQ.m_DateText.text = trans.transactions[i].date;
                displayHQ.m_AmountText.text = trans.transactions[i].total.ToString();
            }
        }
        else
        {
            print("ERROR");
        }
        
    }   

 

}



[System.Serializable]
public class UserNodeData
{
    public string clusterMac;
    public List<node_single> clusterNodes;
}


[System.Serializable]
public class getNodeListClusterClass
{
    public string cluster_mac = "";
    public int limit = 10;
    public int offset = 0;
}
[System.Serializable]
public class UserClustersClass
{
    public List<userCluster> clusters = new();
}


[System.Serializable]
public class userCluster
{
    public string cluster_name = "";
    public int nodes_count = 0;
    public string permissiontype_description = "";
    public string owner_username = "";
    public string cluster_mac = "";

}

[System.Serializable]
public class NodeCreation
{
   public string mac;
   public string name;
   public string nodetype;
   public string modified;
   public int status;
   public string cluster;
   public string altitude;
   public string longitude;
   public string latitude;
}

[System.Serializable]
public class ApiResponse
{
    public string username;
    public float total;
    public List<Device> devices;
    public List<HistoryData> history;
}

[System.Serializable]
public class Device
{
    public string name;
    public float total;
    public NIO nio;
}


[System.Serializable]
public class NIO
{

}

[System.Serializable]
public class HistoryData
{
    public string username;
    public string date;
    public string total;
    public string description;
}




