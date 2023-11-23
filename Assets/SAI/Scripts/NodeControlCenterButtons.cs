using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class NodeControlCenterButtons : MonoBehaviour
{

    [Header("User Panel")]
    [SerializeField] public GameObject UserPanel;
    [Header("Cluster Panel")]
    [SerializeField] public GameObject ClusterPanel;
    [Header("Nodes Panel")]
    [SerializeField] public GameObject NodesPanel;
    [Header("Sensor Panel")]
    [SerializeField] public GameObject SensorPanel;
    [Header("Menu Panel")]
    [SerializeField] public GameObject MenuPanel;
    [Header("History Data")]
    [SerializeField] public GameObject HistoryData;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenClusterPanel()
    {

        ClusterPanel.SetActive(true);
        UserPanel.SetActive(true);
        SensorPanel.SetActive(false);
        NodesPanel.SetActive(false);
        HistoryData.SetActive(false);
    }

    //OPENNODEPANEL
    public void OpenNodeSensorsPanel(string adress)
    {
        ClusterPanel.SetActive(false);
        UserPanel.SetActive(false);
        SensorPanel.SetActive(false);
        NodesPanel.SetActive(true);
        HistoryData.SetActive(true);

    }
    public void OpenSensorPanel()
    {
        ClusterPanel.SetActive(false);
        UserPanel.SetActive(false);
        SensorPanel.SetActive(true);
        NodesPanel.SetActive(false);
        HistoryData.SetActive(false);
    }
}