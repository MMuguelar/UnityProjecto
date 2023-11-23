using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using System.Globalization;
using Unity.VisualScripting;
using Bitsplash.DatePicker;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Text.RegularExpressions;


/// <summary>
/// 
/// </summary>

public static class SharedData
{
    public static string Mac { get; set; }
    public static string dateFrom { get; set; }
    public static string dateTo { get; set; }
    public static int offset { get; set; }
}
public static class ButtonExtension
{
    public static void AddEventListener<T, F>(this Button button, T param1, F param2, Action<T, F> OnClick)
    {
        button.onClick.AddListener(delegate ()
        {
            OnClick(param1, param2);
        });
    }
    public static void AddEventListener<T>(this Button button, T param1, Action<T> OnClick)
    {
        button.onClick.AddListener(delegate ()
        {
            OnClick(param1);
        });
    }
}


//public class SensorData
//{
//    public string sensorName;
//    public int JSONField;

//    public SensorData(string _sensorName, int _JSONField)
//    {
//        sensorName = _sensorName;
//        JSONField = _JSONField;
//    }
//}

public static class Truncate
{
    public static float TruncateNum(this float value, int digits)
    {
        double mult = Math.Pow(10.0, digits);
        double result = Math.Truncate(mult * value) / mult;
        return (float)result;
    }
    public static double TruncateNum(this double value, int digits)
    {
        double mult = Math.Pow(10.0, digits);
        double result = Math.Truncate(mult * value) / mult;
        return (double)result;
    }
    public static string TruncateNum(this string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value.Substring(0, maxLength);
    }
}

//////////////////////////////////////////////////////////////////




//////////////////////////////////////////////////////////////////

public class NodesTableLogic : MonoBehaviour
{

    public GameObject clusterTableContent;
    public GameObject textButtonTemplate;
    public GameObject nodeRowTemplate;
    public GameObject nodesHolder;
    public GameObject sensorContent;
    public GameObject telemetryContent;
    public GameObject sensorRow;
    public GameObject telemetryRow;
    public GameObject refreshButton;
    public GameObject errorPanel;
    public TMP_Text nodeName;
    public NodeControlCenterButtons controlButtons;
    public NodeControl nodeControl;
    public indexHist resetIndex;
    public TMP_Text updatedTime1;
    public TMP_Text updatedTime2;
    public GameObject clusterCol;
    public GameObject error;
    public GameObject fromDate;
    public GameObject toDate;
    public Button backButton;
    public Button nextHistory;
    public Button prevHistory;
    public TMP_Text pages;
    public GameObject clusterBtnPrefab;
    public TMP_Text clusterTitle;
    public Button allClustersButton;
    public GameObject addCluster_editSensorButton;
    public GameObject sensorPanelTableContent;
    public GameObject sensorEditRowPrefab;
    public GameObject addSensorPopup;
    public GameObject editSensorPopup;
    public GameObject addClusterPopup;
    public Button acceptSensorConfigButton;
    public Button acceptEditSensorConfigButton;
    public Button acceptClusterConfigButton;
    public Button addSensorButton;

    public TMP_Text textToShow;

    public TMP_Text okInputField;
    public TMP_Text midInputField;
    public TMP_Text errorInputField;
    public TMP_Text formulaInputField;

    public TMP_Text addInputSensorTypeCode;
    public TMP_Text addOkInputField;
    public TMP_Text addMidInputField;
    public TMP_Text addErrorInputField;
    public TMP_Text addFormulaInputField;

    public TMP_Text clusterNameInputField;
    public TMP_Text clusterDescriptionInputField;

    private string isoFrom;
    private string isoTo;
    private int offset = 0;

    public List<Cluster> clusterList = new();
    public List<NodeData> nodeList = new();


    //VARIABLES TEMPORALES
    public Sensor lastSensor = null;
    //public TMP_Text counterText;
    public int counter;
    public bool isSensorTable = false;
    public bool deleteHistory = false;
    public bool stopUpdateTime = false;
    public bool requestHistory = false;

    public string currentNodeMac;

    void Start()
    {
        InvokeRepeating("FillTable", 0f, 10f);
        SensorLogic.InitializeSensorValues();
        backButton.onClick.AddListener(OnClick);
        FillClusterColumn();
    }

  //Llena la tabla de Nodos con los Nodos del Cluster y la informacion de los Nodos
    void FillTable()
    {

        addCluster_editSensorButton.transform.GetComponent<Button>().onClick.RemoveAllListeners();

        addCluster_editSensorButton.transform.GetComponent<Button>().onClick.AddListener(AddCluster);

        allClustersButton.onClick.AddListener(FillTable);

        clusterTitle.text = "All Nodes";

        //"Last Update" time is updated
        updatedTime1.text = "UPDATED " + DateTime.Now.ToLongTimeString();
        updatedTime2.text = "UPDATED " + DateTime.Now.ToLongTimeString();

        List<NodeData> nodeList = SAI.SDK.Nodes.GetNodeList();
        int N = nodeList.Count;

        //GameObject rowTemplate = transform.GetChild(0).gameObject;
        GameObject g;

        foreach (Transform child in clusterTableContent.transform)
        {
            print("Im destroying a child");
            Destroy(child.gameObject);
        }


       


        //Set data in row
        for (int i = 0; i < N; i++)
        {

            //Debug.Log("Nodo:" + nodeList[i].name + " " + nodeList[i].adress + " " + nodeList[i].status);
            g = Instantiate(nodeRowTemplate, clusterTableContent.transform);
            g.transform.GetChild(1).GetComponent<TMP_Text>().text = nodeList[i].node_name;

            g.transform.GetChild(2).GetComponent<TMP_Text>().text = nodeList[i].last_modification;// DateTime.ParseExact(nodeList[i].last_modification, "yyyy-MM-ddTHH:mm:ss.ffffffZ", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");


            if (nodeList[i].last_modification != "")
            {
                g.transform.GetChild(2).GetComponent<TMP_Text>().text = DateTime.ParseExact(nodeList[i].last_modification, "yyyy-MM-ddTHH:mm:ss.ffffffZ", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                g.transform.GetChild(2).GetComponent<TMP_Text>().text = "Never";
            }

            Debug.Log("Status: " + nodeList[i].node_status);



            if (nodeList[i].node_status == "on")
            {
                g.transform.GetChild(3).GetComponent<TMP_Text>().color = Color.green;
            }
            else
            {
                g.transform.GetChild(3).GetComponent<TMP_Text>().color = Color.red;
            }
            g.transform.GetChild(3).GetComponent<TMP_Text>().text = nodeList[i].node_status;
            g.transform.GetChild(4).GetComponent<TMP_Text>().text = "Lat: " + nodeList[i].node_latitude + "\nLon: " + nodeList[i].node_longitude;
            g.transform.GetChild(5).GetComponent<TMP_Text>().text = nodeList[i].node_mac;
            g.transform.GetChild(7).GetComponent<Button>().AddEventListener(nodeList[i].node_mac, TestHistory);
            g.transform.GetChild(6).GetComponent<Button>().AddEventListener(nodeList[i].node_latitude.ToString(), nodeList[i].node_longitude.ToString(), NodeClickedViewMap);

            //Debug.Log("LAT: " + nodeList[i].position.lat);
            //Debug.Log("LON: " + nodeList[i].position.@long);
        }
        ////TEXTO DEL CONTADOR
        ////counterText.text = "Counter: " + counter.ToString();
    }
    void FillSensorTable(string node_mac)
    {
        currentNodeMac = node_mac;

        Debug.Log("FillSensorTable");

        string url = SAI.SDK.API.host + SAI.SDK.Nodes.GetPostPutNodeSensor + "?node_mac=" + node_mac;

        string jsonData = JsonConvert.SerializeObject(node_mac, Formatting.Indented);

        string response = SAI.SDK.API.GenericGetRequest(url);

        if (response != string.Empty)
        {
          
            SAI.SDK.Util.DestroyChilders(sensorPanelTableContent);
            
                
                print("SENSOR RESPONSE: " + response);
                List<SensorFromNodeData> sensorDataList = JsonConvert.DeserializeObject<List<SensorFromNodeData>>(response);

                addSensorButton.onClick.RemoveAllListeners();
                addSensorButton.AddEventListener(currentNodeMac, AddSensor);

                GameObject row;

                foreach (SensorFromNodeData sensorData in sensorDataList)
                {
                    row = Instantiate(sensorEditRowPrefab, sensorPanelTableContent.transform);
                    row.transform.GetChild(0).GetComponent<TMP_Text>().text = sensorData.nodesensor_id.ToString();
                    row.transform.GetChild(1).GetComponent<TMP_Text>().text = sensorData.sensortype_name;
                    row.transform.GetChild(2).GetComponent<TMP_Text>().text = sensorData.node_mac;
                    row.transform.GetChild(3).GetComponent<TMP_Text>().text = sensorData.sensortype_code;
                    row.transform.GetChild(4).GetComponent<Button>().AddEventListener(sensorData.nodesensor_id.ToString(), OpenEditPopUp);
                }
            
        }

        controlButtons.OpenSensorPanel();
    }  

    private void FillSensorTableCallback(UnityWebRequest response)
    {
        foreach (Transform child in sensorPanelTableContent.transform)
        {
            Destroy(child.gameObject);
        }

        if (response.result == UnityWebRequest.Result.Success)
        {
            string json = response.downloadHandler.text;
            print("SENSOR RESPONSE: " + json);
            List<SensorFromNodeData> sensorDataList = JsonConvert.DeserializeObject<List<SensorFromNodeData>>(json);

            addSensorButton.onClick.RemoveAllListeners();
            addSensorButton.AddEventListener(currentNodeMac, AddSensor);

            GameObject row;

            foreach (SensorFromNodeData sensorData in sensorDataList)
            {
                row = Instantiate(sensorEditRowPrefab, sensorPanelTableContent.transform);
                row.transform.GetChild(0).GetComponent<TMP_Text>().text = sensorData.nodesensor_id.ToString();
                row.transform.GetChild(1).GetComponent<TMP_Text>().text = sensorData.sensortype_name;
                row.transform.GetChild(2).GetComponent<TMP_Text>().text = sensorData.node_mac;
                row.transform.GetChild(3).GetComponent<TMP_Text>().text = sensorData.sensortype_code;
                row.transform.GetChild(4).GetComponent<Button>().AddEventListener(sensorData.nodesensor_id.ToString(), OpenEditPopUp);
            }
        }
    }

    private void AddSensor(string node_mac)
    {
        print("ADD SENSOR TO: " + node_mac);


        OpenAddSensorPopUp(node_mac);
    }

    private void AddCluster()
    {
        print("ADD CLUSTER");

        OpenAddClusterPopUp();
    }

    public void UpdateSensorConfiguration(string sensorId)
    {

        if(SAI.SDK.Nodes.EditSensor(sensorId, okInputField.text, midInputField.text, errorInputField.text, formulaInputField.text))
        {
            CloseEditPopUp();

            FillSensorTable(currentNodeMac);
        }

     


    }
    public void AddSensorReq(string node_mac)
    {

       if(SAI.SDK.Nodes.AddSensor(node_mac, addInputSensorTypeCode.text, addOkInputField.text, addMidInputField.text, addErrorInputField.text, addFormulaInputField.text))
        {
            CloseAddSensorPopUp();
            FillSensorTable(currentNodeMac);
        }
        else
        {
            print("Fail to add sensor");
        }

        
    }

    public void AddClusterReq()
    {

        if (SAI.SDK.Clusters.AddCluster(clusterNameInputField.text, clusterDescriptionInputField.text))
        {
            CloseAddClusterPopUp();
            FillClusterColumn();
        }
            
          
       
    }

 

    private IEnumerator HideImageAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);

        textToShow.enabled = false;
    }
    void OpenEditPopUp(string sensorId)
    {
        editSensorPopup.transform.GetChild(8).GetComponent<Button>().onClick.RemoveAllListeners();
        editSensorPopup.transform.GetChild(8).GetComponent<Button>().onClick.AddListener(CloseEditPopUp);

        print("OpenEditPopUp with id: " + sensorId);

        editSensorPopup.SetActive(true);

        acceptEditSensorConfigButton.onClick.RemoveAllListeners();
        acceptEditSensorConfigButton.AddEventListener(sensorId, UpdateSensorConfiguration); 
    }
    void OpenAddSensorPopUp(string node_mac)
    {
        addSensorPopup.transform.GetChild(9).GetComponent<Button>().onClick.RemoveAllListeners();
        addSensorPopup.transform.GetChild(9).GetComponent<Button>().onClick.AddListener(CloseAddSensorPopUp);

        print("OpenEditPopUp with node mac: " + node_mac);

        addSensorPopup.SetActive(true);

        acceptSensorConfigButton.onClick.RemoveAllListeners();
        acceptSensorConfigButton.AddEventListener(node_mac, AddSensorReq);
    }

    void OpenAddClusterPopUp()
    {
        addClusterPopup.transform.GetChild(6).GetComponent<Button>().onClick.AddListener(CloseAddClusterPopUp);

        addClusterPopup.SetActive(true);

        acceptClusterConfigButton.onClick.AddListener(AddClusterReq);
    }

    void CloseEditPopUp()
    {
        editSensorPopup.SetActive(false);
    }
    void CloseAddSensorPopUp()
    {
        addSensorPopup.SetActive(false);
    }

    void CloseAddClusterPopUp()
    {
        addClusterPopup.SetActive(false);
    }
    //Clicked in a node of the table
    void TestHistory(string node_mac)
    {
        //HERE I HAVE TO CHANGE THE BUTTON UN TOP TO BE CONFIG SENSOR AND GIVE IT THE OnClick(). THE ONCLICK WILL BE ANOTHER FUNCTION WICH ACTIVATES THE POPUP SENDING THE NODE_MAC

        addCluster_editSensorButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "Config Sensors";
        addCluster_editSensorButton.transform.GetComponent<Button>().onClick.RemoveAllListeners();
        addCluster_editSensorButton.transform.GetComponent<Button>().AddEventListener(node_mac, FillSensorTable);

        Destroy(updatedTime1);
        Destroy(updatedTime2);

        isSensorTable = true;

        nodeName.text = node_mac;

        SharedData.Mac = node_mac;

        controlButtons.OpenNodeSensorsPanel("");

        firstRequestHistory();
    }
    public void firstRequestHistory()
    {
        GameObject t;
        GameObject t1;

        RootObject data = new RootObject();
        nodeControl.test(ref data);

        Debug.Log("Sensor List:");

        int numberOfSensorRows = 0;
        foreach (SensorData sensor in data.sensor_data)
        {
            t = Instantiate(sensorRow, sensorContent.transform);
            numberOfSensorRows++;
            t.transform.GetChild(0).GetComponent<TMP_Text>().text = sensor.date;
            t.transform.GetChild(1).GetComponent<TMP_Text>().text = sensor.detail.boardTemp.ToString();
            t.transform.GetChild(2).GetComponent<TMP_Text>().text = sensor.detail.voltageVibr.ToString();
            t.transform.GetChild(3).GetComponent<TMP_Text>().text = sensor.detail.currentSensor.ToString();
            t.transform.GetChild(4).GetComponent<TMP_Text>().text = sensor.detail.voltageSensor3.ToString();
            t.transform.GetChild(5).GetComponent<TMP_Text>().text = sensor.detail.voltageSensor100.ToString();
        }

        Debug.Log("Elementos instanciados" + numberOfSensorRows);

        //Debug the sensor telemetry
        Debug.Log("Telemetry List:");

        int numberOfTelemetryRows = 0;
        foreach (TelemetryData telemetry in data.telemetry_data)
        {
            t1 = Instantiate(telemetryRow, telemetryContent.transform);
            numberOfTelemetryRows++;
            t1.transform.GetChild(0).GetComponent<TMP_Text>().text = telemetry.date;
            t1.transform.GetChild(1).GetComponent<TMP_Text>().text = telemetry.detail.altitude.ToString();
            t1.transform.GetChild(2).GetComponent<TMP_Text>().text = telemetry.detail.latitude.ToString();
            t1.transform.GetChild(3).GetComponent<TMP_Text>().text = telemetry.detail.longitude.ToString();
            t1.transform.GetChild(4).GetComponent<TMP_Text>().text = telemetry.detail.board_temp.ToString();
            t1.transform.GetChild(5).GetComponent<TMP_Text>().text = telemetry.detail.lin_accel_x.ToString();
            t1.transform.GetChild(6).GetComponent<TMP_Text>().text = telemetry.detail.lin_accel_y.ToString();
            t1.transform.GetChild(7).GetComponent<TMP_Text>().text = telemetry.detail.lin_accel_z.ToString();
            t1.transform.GetChild(8).GetComponent<TMP_Text>().text = telemetry.detail.grav_x.ToString();
            t1.transform.GetChild(9).GetComponent<TMP_Text>().text = telemetry.detail.grav_y.ToString();
            t1.transform.GetChild(10).GetComponent<TMP_Text>().text = telemetry.detail.grav_z.ToString();
        }

        Debug.Log("Elementos instanciados" + numberOfTelemetryRows);

        if (numberOfSensorRows == 0 && numberOfTelemetryRows == 0)
        {
            SAI.SDK.Util.ErrorHandler.ShowPopup("No history data to show", "SpaceAI");
            /*
            error.SetActive(true);
            error.transform.GetChild(0).GetComponent<TMP_Text>().text = "No history data to show";
            Invoke("disableError", 2f);
            */
            nextHistory.interactable = false;
        }
        else if (numberOfSensorRows != 5)
        {
            nextHistory.interactable = false;
        }
        else
        {
            nextHistory.interactable = true;
        }
    }
    public void validateDate()
    {
        resetIndex.ResetPagesIndex();

        requestHistory = true;
        offset = 0;
        
        string fromDate = NewSharedData2.fromDropDown;
        string toDate = NewSharedData2.toDropDown;
        
        Debug.Log("FromDate: " + fromDate + "ToDate: " + toDate);

        DateTime fromDateValue = DateTime.Parse(fromDate);
        DateTime toDateValue = DateTime.Parse(toDate);

        string formattedDateFrom = fromDateValue.ToString("yyyy-MM-ddT00:00:01Z");
        string formattedDateTo = toDateValue.ToString("yyyy-MM-ddT23:59:59Z");

        Debug.Log("FromIso: " + formattedDateFrom + " ToIso: " + formattedDateTo);

        if (toDateValue < fromDateValue)
        {
            Debug.LogError("To date can't be lower From date");
            error.SetActive(true);
            error.transform.GetChild(0).GetComponent<TMP_Text>().text = "To date can't be lower From date";
            Invoke("disableError", 2f);
        }
        else
        {
            Debug.Log("ValidDate");
            SharedData.offset = offset;
            SharedData.dateFrom = formattedDateFrom;
            SharedData.dateTo = formattedDateTo;
            Debug.Log("Offset: " + offset + " " + "dateFrom: " + formattedDateFrom + " " + "toDate: " + formattedDateTo);
            newSearch();
        }
    }
    public void FillClusterColumn()
    {
        clusterList = SAI.SDK.Clusters.GetClusterList();

        Debug.Log("fillClusterColumn");

      

      
        foreach (Transform child in clusterCol.transform)
        {
            if (child != clusterCol.transform.GetChild(0))
            {
                Destroy(child.gameObject);
            }
        }
      

        Debug.Log("CLUSTER COUNT: " + clusterList.Count);

        for (int i = 0; i < clusterList.Count; i++)
        {
            int index = i;

            // Instantiate a button for the cluster
            Debug.Log("CLUSTER NAME: " + clusterList[i].cluster_name);
            GameObject clusterButton = Instantiate(clusterBtnPrefab, clusterCol.transform);

            clusterButton.transform.GetChild(0).GetComponent<TMP_Text>().text = clusterList[i].cluster_name;
            clusterButton.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => OnClusterButtonClick(clusterList[index]));
        }
    }    
    
    private void FillNodesFromCluster(Cluster cluster)
    {
        nodeList = SAI.SDK.Nodes.GetNodeFromCluster(cluster);

        int N = nodeList.Count;

        GameObject g = new GameObject();

        foreach (Transform child in clusterTableContent.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < N; i++)
        {

            //Debug.Log("Nodo:" + nodeList[i].name + " " + nodeList[i].adress + " " + nodeList[i].status);
            g = Instantiate(nodeRowTemplate, clusterTableContent.transform);
            g.transform.GetChild(1).GetComponent<TMP_Text>().text = nodeList[i].node_name;
            g.transform.GetChild(2).GetComponent<TMP_Text>().text = nodeList[i].last_modification; //DateTime.ParseExact(nodeList[i].last_modification, "yyyy-MM-ddTHH:mm:ss.ffffffZ", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
            Debug.Log("Status: " + nodeList[i].node_status);



            if (nodeList[i].node_status == "on")
            {
                g.transform.GetChild(3).GetComponent<TMP_Text>().color = Color.green;
            }
            else
            {
                g.transform.GetChild(3).GetComponent<TMP_Text>().color = Color.red;
            }
            g.transform.GetChild(3).GetComponent<TMP_Text>().text = nodeList[i].node_status;
            g.transform.GetChild(4).GetComponent<TMP_Text>().text = "Lat: " + nodeList[i].node_longitude + "\nLon: " + nodeList[i].node_longitude;
            g.transform.GetChild(5).GetComponent<TMP_Text>().text = nodeList[i].node_mac;
            g.transform.GetChild(7).GetComponent<Button>().AddEventListener(nodeList[i].node_mac, TestHistory);
            g.transform.GetChild(6).GetComponent<Button>().AddEventListener(nodeList[i].node_latitude.ToString(), nodeList[i].node_longitude.ToString(), NodeClickedViewMap);

        }
    }

    private void OnClusterButtonClick(Cluster cluster)
    {

        addCluster_editSensorButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "ADD CLUSTER";
        addCluster_editSensorButton.transform.GetComponent<Button>().onClick.RemoveAllListeners();
        addCluster_editSensorButton.transform.GetComponent<Button>().onClick.AddListener(AddCluster);

        cleanFilter();
        eraseDate();
        CancelInvoke("FillTable");

        

        clusterTitle.text = cluster.cluster_name;
        //nodeList = SAI.SDK.Clusters.nodes.GetNodeFromCluster(cluster);
        FillNodesFromCluster(cluster);
        controlButtons.OpenClusterPanel();
    }

    ////private void HandleClusterListReceived(NodeControl.ClustersList clusterList)
    ////{
    ////    Debug.Log("Received ClusterList with " + clusterList.clusters.Count + " clusters.");

    ////    foreach (Transform child in clusterCol.transform)
    ////    {
    ////        Destroy(child.gameObject);
    ////    }

    ////    foreach (var clusterData in clusterList.clusters)
    ////    {
    ////        // Instantiate a button for the cluster
    ////        GameObject clusterButton = Instantiate(clusterBtnPrefab, clusterCol.transform);

    ////        clusterButton.transform.GetChild(0).GetComponent<TMP_Text>().text = clusterData.cluster_name;
    ////        clusterButton.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => OnClusterButtonClick(clusterData));
    ////    }
    ////}

    // Example event handler for button click
    ////private void OnClusterButtonClick(NodeControl.Cluster clusterData)
    ////{
    ////    CancelInvoke("FillTable");
    ////    // Handle the button click event for the specific clusterData
    ////    Debug.Log("Button clicked for cluster: " + clusterData.cluster_name);
    ////    //List<NodeData> nodeList = nodeControl.GetNodeListCluster(clusterData.cluster_mac);
    ////    int N = nodeList.Count;

    ////    GameObject g;

    ////    foreach (Transform child in transform)
    ////    {
    ////        Destroy(child.gameObject);
    ////    }

    ////    for (int i = 0; i < N; i++)
    ////    {

    ////        //Debug.Log("Nodo:" + nodeList[i].name + " " + nodeList[i].adress + " " + nodeList[i].status);
    ////        g = Instantiate(nodeRowTemplate, transform);
    ////        g.transform.GetChild(1).GetComponent<TMP_Text>().text = nodeList[i].node_name;
    ////        g.transform.GetChild(2).GetComponent<TMP_Text>().text = DateTime.ParseExact(nodeList[i].last_modification, "yyyy-MM-ddTHH:mm:ss.ffffffZ", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
    ////        Debug.Log("Status: " + nodeList[i].node_status);



    ////        if (nodeList[i].node_status == "on")
    ////        {
    ////            g.transform.GetChild(3).GetComponent<TMP_Text>().color = Color.green;
    ////        }
    ////        else
    ////        {
    ////            g.transform.GetChild(3).GetComponent<TMP_Text>().color = Color.red;
    ////        }
    ////        g.transform.GetChild(3).GetComponent<TMP_Text>().text = nodeList[i].node_status;
    ////        g.transform.GetChild(4).GetComponent<TMP_Text>().text = "Lat: " + nodeList[i].node_latitude + "\nLon: " + nodeList[i].node_longitude;
    ////        g.transform.GetChild(5).GetComponent<TMP_Text>().text = nodeList[i].node_mac;
    ////        g.transform.GetChild(7).GetComponent<Button>().AddEventListener(nodeList[i].node_mac, TestHistory);
    ////        g.transform.GetChild(6).GetComponent<Button>().AddEventListener(nodeList[i].node_latitude.ToString(), nodeList[i].node_longitude.ToString(), NodeClickedViewMap);

    ////    }
    ////}

    ////private void HandleClusterListReceived(NodeControl.ClustersList clusterList)
    ////{
    ////    Debug.Log("Received ClusterList with " + clusterList.clusters.Count + " clusters.");

    ////    foreach (Transform child in clusterCol.transform)
    ////    {
    ////        Destroy(child.gameObject);
    ////    }

    ////    foreach (var clusterData in clusterList.clusters)
    ////    {
    ////        // Instantiate a button for the cluster
    ////        GameObject clusterButton = Instantiate(clusterBtnPrefab, clusterCol.transform);

    ////        clusterButton.transform.GetChild(0).GetComponent<TMP_Text>().text = clusterData.cluster_name;
    ////        clusterButton.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => OnClusterButtonClick(clusterData));
    ////    }
    ////}

    // Example event handler for button click
    ////private void OnClusterButtonClick(NodeControl.Cluster clusterData)
    ////{
    ////    CancelInvoke("FillTable");
    ////    // Handle the button click event for the specific clusterData
    ////    Debug.Log("Button clicked for cluster: " + clusterData.cluster_name);
    ////    //List<NodeData> nodeList = nodeControl.GetNodeListCluster(clusterData.cluster_mac);
    ////    int N = nodeList.Count;

    ////    GameObject g;

    ////    foreach (Transform child in transform)
    ////    {
    ////        Destroy(child.gameObject);
    ////    }

    ////    for (int i = 0; i < N; i++)
    ////    {

    ////        //Debug.Log("Nodo:" + nodeList[i].name + " " + nodeList[i].adress + " " + nodeList[i].status);
    ////        g = Instantiate(nodeRowTemplate, transform);
    ////        g.transform.GetChild(1).GetComponent<TMP_Text>().text = nodeList[i].node_name;
    ////        g.transform.GetChild(2).GetComponent<TMP_Text>().text = DateTime.ParseExact(nodeList[i].last_modification, "yyyy-MM-ddTHH:mm:ss.ffffffZ", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
    ////        Debug.Log("Status: " + nodeList[i].node_status);



    ////        if (nodeList[i].node_status == "on")
    ////        {
    ////            g.transform.GetChild(3).GetComponent<TMP_Text>().color = Color.green;
    ////        }
    ////        else
    ////        {
    ////            g.transform.GetChild(3).GetComponent<TMP_Text>().color = Color.red;
    ////        }
    ////        g.transform.GetChild(3).GetComponent<TMP_Text>().text = nodeList[i].node_status;
    ////        g.transform.GetChild(4).GetComponent<TMP_Text>().text = "Lat: " + nodeList[i].node_latitude + "\nLon: " + nodeList[i].node_longitude;
    ////        g.transform.GetChild(5).GetComponent<TMP_Text>().text = nodeList[i].node_mac;
    ////        g.transform.GetChild(7).GetComponent<Button>().AddEventListener(nodeList[i].node_mac, TestHistory);
    ////        g.transform.GetChild(6).GetComponent<Button>().AddEventListener(nodeList[i].node_latitude.ToString(), nodeList[i].node_longitude.ToString(), NodeClickedViewMap);

    ////    }
    ////}
    private void newSearch()
    {
        eraseDate();
        FillHistory();
    }
    public void nextButton()
    {

        if (requestHistory == false)
        {
            offset += 5;
            SharedData.offset = offset;
            eraseDate();
            firstRequestHistory();

            if (offset != 0)
            {
                prevHistory.interactable = true;
            }
         
        }
        else
        {
            offset += 5;
            SharedData.offset = offset;
            eraseDate();
            FillHistory();

            if (offset != 0)
            {
                prevHistory.interactable = true;
            }
        }

    }
    public void prevButton()
    {

        if (requestHistory == false)
        {
            offset -= 5;
            SharedData.offset = offset;
            eraseDate();
            firstRequestHistory();

            if (offset == 0)
            {
                prevHistory.interactable = false;
            }
        }
        else
        {
            offset -= 5;
            SharedData.offset = offset;
            eraseDate();
            FillHistory();

            if (offset == 0)
            {
                prevHistory.interactable = false;
            }
        }
    }
    void cleanFilter()
    {
        fromDate.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Date From";
        toDate.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Date To";
    }
    void FillHistory()
    {
        GameObject t;
        GameObject t1;

        RootObject data = new RootObject();
        nodeControl.dataSearch(ref data);

        int numberOfSensorRows = 0;

        foreach (SensorData sensor in data.sensor_data)
        {
            t = Instantiate(sensorRow, sensorContent.transform);
            numberOfSensorRows++;
            t.transform.GetChild(0).GetComponent<TMP_Text>().text = sensor.date;
            t.transform.GetChild(1).GetComponent<TMP_Text>().text = sensor.detail.boardTemp.ToString();
            t.transform.GetChild(2).GetComponent<TMP_Text>().text = sensor.detail.voltageVibr.ToString();
            t.transform.GetChild(3).GetComponent<TMP_Text>().text = sensor.detail.currentSensor.ToString();
            t.transform.GetChild(4).GetComponent<TMP_Text>().text = sensor.detail.voltageSensor3.ToString();
            t.transform.GetChild(5).GetComponent<TMP_Text>().text = sensor.detail.voltageSensor100.ToString();
        }

        Debug.Log("Valores de los sensores: " + numberOfSensorRows);
        
        if (numberOfSensorRows == 0)
        {
            error.SetActive(true);
            error.transform.GetChild(0).GetComponent<TMP_Text>().text = "No history data to show";
            Invoke("disableError", 2f);
            nextHistory.interactable = false;
        }
        else if (numberOfSensorRows != 5)
        {
            nextHistory.interactable = false;
        }
        else
        {
            nextHistory.interactable = true;
        }

        foreach (TelemetryData telemetry in data.telemetry_data)
        {
            t1 = Instantiate(telemetryRow, telemetryContent.transform);
            t1.transform.GetChild(0).GetComponent<TMP_Text>().text = telemetry.date;
            t1.transform.GetChild(1).GetComponent<TMP_Text>().text = telemetry.detail.altitude.ToString();
            t1.transform.GetChild(2).GetComponent<TMP_Text>().text = telemetry.detail.latitude.ToString();
            t1.transform.GetChild(3).GetComponent<TMP_Text>().text = telemetry.detail.longitude.ToString();
            t1.transform.GetChild(4).GetComponent<TMP_Text>().text = telemetry.detail.board_temp.ToString();
            t1.transform.GetChild(5).GetComponent<TMP_Text>().text = telemetry.detail.lin_accel_x.ToString();
            t1.transform.GetChild(6).GetComponent<TMP_Text>().text = telemetry.detail.lin_accel_y.ToString();
            t1.transform.GetChild(7).GetComponent<TMP_Text>().text = telemetry.detail.lin_accel_z.ToString();
            t1.transform.GetChild(8).GetComponent<TMP_Text>().text = telemetry.detail.grav_x.ToString();
            t1.transform.GetChild(9).GetComponent<TMP_Text>().text = telemetry.detail.grav_y.ToString();
            t1.transform.GetChild(10).GetComponent<TMP_Text>().text = telemetry.detail.grav_z.ToString();
        }
    }
    public void disableError()
    {
        error.SetActive(false);
    }
    void eraseDate()
    {

        foreach (Transform child in telemetryContent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in sensorContent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    //void NodeClicked(string adress)
    //{
    //    FillSensors2(adress);
    //}

    //void FillSensors2(string _address)
    //{
    //    isSensorTable = true;

    //    //"Last Update" time is updated
    //    updatedTime1.text = "UPDATED " + DateTime.Now.ToLongTimeString();
    //    updatedTime2.text = "UPDATED " + DateTime.Now.ToLongTimeString();

    //    // Retrieves info from the node given an address
    //    Node thisnode = nodeControl.getNodeInfo(_address, "");

    //    Debug.Log("THIS NODE: " + thisnode);

    //    // Reads ipfs file and fill the information into a Sensor Class
    //    Sensor sensor = nodeControl.GetSensorList(thisnode.lastHash);

    //    Debug.Log("SENSOR Ducto: " + sensor.voltageSensor3);
    //    Debug.Log("SENSOR: " + sensor.latitud);
    //    Debug.Log("SENSOR: " + sensor.longitud);
    //    Debug.Log("SENSOR Rectificador: " + sensor.voltageSensor100);
    //    Debug.Log("SENSOR Bateria: " + sensor.voltageVibr);
    //    Debug.Log("SENSOR Shunt: " + sensor.currentSensor);
    //    Debug.Log("SENSOR: " + sensor.date);

    //    //initializa by sensor list
    //    List<SensorData> sensorDataList = new List<SensorData>();

    //    int count = 2;
    //    string sensortitle;
    //    string unitofmeasurement;
    //    double resultDouble;

    //    GameObject x;
    //    GameObject r;
    //    string sensorName;

    //    controlButtons.OpenNodeSensorsPanel(_address);

    //    //destroy game objects
    //    foreach (Transform child in nodesHolder.transform)
    //    {
    //        Destroy(child.gameObject);
    //    }

    //    // For each property in the Sensor Class check if it is in the dictionary
    //    foreach (var propertyInfo in typeof(Sensor).GetProperties())
    //    {
    //        // is the property name in the dictionary  i.e it will not enter here for dateStamp but will enter for voltageSensor3
    //        if (SensorLogic._sensorValues.ContainsKey(propertyInfo.Name))
    //        {
    //            count++;

    //            SensorLogic.TryGetSensorNameAndUnitOfMeasurement(propertyInfo.Name, out sensortitle, out unitofmeasurement);

    //            Debug.Log("FINAL NAME: " + sensortitle);

    //            sensorDataList.Add(new SensorData(sensortitle, count));
    //            var sensorValue = SensorLogic._sensorValues[propertyInfo.Name];

    //            x = Instantiate(sensorPrefab, nodesHolder.transform);
    //            Transform sensorRow = x.transform.GetChild(1).GetChild(1).GetChild(0).GetChild(0);
    //            x.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = sensortitle;

    //            r = Instantiate(sensorRow.gameObject, x.transform.GetChild(1).GetChild(1).GetChild(0));

    //            var propertyValue = propertyInfo.GetValue(sensor);

    //            Debug.Log("PV " + sensortitle + ": " + propertyValue);
    //            Debug.Log("PV TO STRING: " + propertyValue.ToString());

    //            if (SensorLogic.TryCalculateSensorValue(propertyInfo.Name, Convert.ToDouble(propertyValue), out resultDouble) && Convert.ToDouble(propertyValue) != 0)
    //            {
    //                Debug.Log("NAME PROPERTY INFO" + propertyInfo.Name);
    //                Debug.Log(sensortitle + "TEST : " + resultDouble);
    //                r.transform.GetChild(0).GetComponent<TMP_Text>().text = sensor.dateStamp + " " + sensor.timeStamp + ":  " + resultDouble.TruncateNum(6).ToString().Replace(',', '.') + unitofmeasurement;
    //            }
    //            else
    //            {
    //                Debug.Log("resultDoubleElse: " + resultDouble);
    //                r.transform.GetChild(0).GetComponent<TMP_Text>().text = "Fuera de Rango";
    //            }

    //            if (Convert.ToDouble(propertyValue) == 0)
    //            {
    //                x.SetActive(false);
    //            }



    //            Destroy(sensorRow.gameObject);

    //        }
    //    }
    //}
    private void OnClick()
    {
       string desktopEscene = "Desktop Template";
       string reloadEscene = "Node Control Center Scene 1";

        nodeName.text = "node control";
        if (!isSensorTable)
        {
            SceneManager.LoadScene(desktopEscene);
        }
        else
        {
            controlButtons.OpenClusterPanel();
            isSensorTable = false;
            SceneManager.LoadScene(reloadEscene);
        }
    }
    void TurnErrorOFF()
    {
        errorPanel.SetActive(false);
    }
    //Refresh Map with Lat and Lon from clicked node
    void NodeClickedViewMap(string lat, string lon)
    {
        Maps.lat = double.Parse(lat, CultureInfo.InvariantCulture);
        Maps.lon = double.Parse(lon, CultureInfo.InvariantCulture);

        // Convert the raw values to strings using the custom NumberFormatInfo for logging
        string formattedLat = Maps.lat.ToString(CultureInfo.InvariantCulture);
        string formattedLon = Maps.lon.ToString(CultureInfo.InvariantCulture);

        Debug.Log("COORDINATES LAT (raw): " + formattedLat);
        Debug.Log("COORDINATES LON (raw): " + formattedLon);
    }
}