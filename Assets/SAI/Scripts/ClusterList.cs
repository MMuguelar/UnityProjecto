using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ClusterList : MonoBehaviour
{
    public NodeControl nodeControl;
    public TMPro.TMP_Dropdown clusteList;
    // Start is called before the first frame update
    void Start()
    {
        List();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void List()
    {
        List<NodeData> nodeList = SAI.SDK.Nodes.GetNodeList();
        int N = nodeList.Count;

        // Borra las opciones actuales en el Dropdown
        clusteList.ClearOptions();

        // Crea una lista de opciones para el Dropdown
        List<string> opciones = new List<string>();

        for (int i = 0; i < N; i++)
        {
            string opcion = "Name: " + nodeList[i].node_name;
            opciones.Add(opcion);
        }

        // Asigna las opciones al Dropdown
        clusteList.AddOptions(opciones);
    }

}
