//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;
//using System.Threading;
//using System.Text;
//using UnityEngine.Networking;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.IO;
//using System;
//using UnityEngine.SceneManagement;
//using System.Globalization;
//public class Main : MonoBehaviour
//{
//    public TMP_Text Infodaytimetext;

//    public TMP_Text onlineNodesT;
//    public TMP_Text filesT;
//    public TMP_Text dataOnDemandT;
//    public TMP_Text bandwidthT;
//    public TMP_Text wattsT;
//    public TMP_Text totalT;
//    public long onlineNodesV = 0;
//    public float filesV = 0;
//    public float dataOnDemandV = 0;
//    public float bandwidthV = 0;
//    public float wattsV = 0;
//    public long totalV = 0;

//    public long onlineNodesVN = 0;
//    public float filesVN = 0;
//    public float dataOnDemandVN = 0;
//    public float bandwidthVN = 0;
//    public float wattsVN = 0;
//    public long totalVN = 0;

//    public GameObject LOADER;

//    public GameObject PANELG1;
//    public GameObject PANELG2;
//    public GameObject PANELG3;
//    public GameObject PANELDATOS;
//    //public GameObject NODE1;
//    //public GameObject LINE1;
//    //public GameObject LINE1FX;
//    //public GameObject LINE2;
//    //public GameObject LINE2FX;
//    //public GameObject ARROW;
//    //public GameObject MASMENOS;
//    //public GameObject BUTTONS1;
//    //public GameObject BUTTONS2;
//    //public GameObject BUTTONS3;
//    //public GameObject panelINFO_SUP;
//    //public GameObject panelINFO_SUP_INFO;
//    public GameObject panelINFO_INF;
//    //public GameObject BG;
//    //public GameObject nodeP;

//    public GameObject node_info1;

//    public GameObject node_info3;

//    public DataService DATA;
//    public bool hide_loader = false;
//    public float reload_time = 10;

//    public TMP_Text NodeName;
//    public TMP_Text NodeGiving;
//    public TMP_Text NodeFrom;
//    public TMP_Text NodePosition;
//    public TMP_Text Node_Total_Money;

//    public GameObject nodeCont;
//    public GameObject nodePrefab;

//    public GameObject botonBACK;


//    void Start()
//    {

//        DATA = GameObject.Find("DATA").GetComponent<DataService>();


//        //Button to go to the home page scene (scene not done)
//        botonBACK.SetActive(!DATA.IS_WEB);

//        string N = UnityEngine.Random.Range(10, 100).ToString() + UnityEngine.Random.Range(10, 100).ToString();


//        //DATA.addNode("usuario324");
//        //DATA.confirmUser("usuario324");

//        onlineNodesT.text = "";

//        filesT.text = "";
//        dataOnDemandT.text = "";
//        bandwidthT.text = "";
//        wattsT.text = "";
//        totalT.text = "";

//        animate0();
//        get_online_nodes_firsttime();
//    }

//    //get online nodes for the first time (2 secs delay)
//    public void get_online_nodes_firsttime()
//    {
//        CancelInvoke("getNodesOnline");
//        Invoke("getNodesOnline", 2);
//    }

//    public void getNodesOnline()
//    {
//        DATA.get_online_nodes();
//    }

//    public void returnNodesOnline()
//    {
//        //destroy the nodes in the node container
//        destroy_child_list(nodeCont);

//        int[] narray = DATA.onlineNodes.onlineNodes;
//        int max = narray.Length;


//        for (int i = 0; i < max; i++)
//        {
//            if (narray[i] != 1)
//            {
//                GameObject node = (GameObject)Instantiate(nodePrefab);
//                node.transform.SetParent(nodeCont.transform, false);

//                bool isCurrent = false;

//                if (DATA.current_id == narray[i])
//                    isCurrent = true;

//                node.GetComponent<NodeCode>().set_node(narray[i], "node" + narray[i].ToString(), isCurrent);
//            }
//        }

//        CancelInvoke("getNodesOnline");
//        Invoke("getNodesOnline", 5);
//    }

//    public void destroy_child_list(GameObject parent)
//    {

//        foreach (Transform child in parent.transform)
//        {
//            Destroy(child.gameObject);
//        }
//    }


//    public void update_data_node_byid(int id)
//    {
//        print(id);
//        DATA.getNodeInfo(id.ToString());
//    }

//    public void update_data_node()
//    {

//        NodeName.text = "" + DATA.current_node_info.node.name;
//        NodeGiving.text = DATA.current_node_info.node.giving.S + " S" + '\n' +
//            DATA.current_node_info.node.giving.P + " P" + '\n' +
//            DATA.current_node_info.node.giving.C + " C" + '\n' +
//            DATA.current_node_info.node.giving.W + " W";

//        string city = DATA.CITY;

//        NodeFrom.text = "From " + DATA.current_node_info.node.from.name;
//        NodePosition.text = "Alt: " + DATA.current_node_info.node.position.altitude + "" + '\n' + "Lat: " + DATA.current_node_info.node.position.lat + "" + '\n' + "Long: " + DATA.current_node_info.node.position.longitude + "";

//        float calc = 0;

//        float price = 1;

//        calc = DATA.PRICE_S * float.Parse(DATA.current_node_info.node.giving.S, CultureInfo.InvariantCulture) * DATA.SCALE_S +
//               DATA.PRICE_P * float.Parse(DATA.current_node_info.node.giving.P, CultureInfo.InvariantCulture) * DATA.SCALE_P +
//               DATA.PRICE_C * float.Parse(DATA.current_node_info.node.giving.C, CultureInfo.InvariantCulture) * DATA.SCALE_C +
//               DATA.PRICE_W * float.Parse(DATA.current_node_info.node.giving.W, CultureInfo.InvariantCulture) * DATA.SCALE_W;

//        Node_Total_Money.text = calc.ToString("F1");

//        anim_nodeinfo();
//    }


//    void changeText1(float newValue)
//    {
//        onlineNodesV = Mathf.CeilToInt(newValue);

//        onlineNodesT.text = onlineNodesV.ToString();
//    }

//    void changeText2(float newValue)
//    {
//        totalV = Mathf.CeilToInt(newValue);

//        totalT.text = totalV.ToString() + "b";
//    }

//    //assign values to node info, assign values to node info global resources and animating texts
//    public void animate_texts()
//    {
//        Hashtable ht = iTween.Hash("from", (float)onlineNodesV, "to", (float)onlineNodesVN, "time", 3f, "onupdate", "changeText1", "easetype", iTween.EaseType.easeOutQuad);

//        iTween.ValueTo(gameObject, ht);

//        Hashtable ht2 = iTween.Hash("from", (float)totalV, "to", (float)totalVN, "time", 2f, "onupdate", "changeText2", "easetype", iTween.EaseType.easeOutQuad);

//        iTween.ValueTo(gameObject, ht2);

//        filesV = filesVN;
//        dataOnDemandV = dataOnDemandVN;
//        bandwidthV = bandwidthVN;
//        wattsV = wattsVN;
//        totalV = totalVN;

//        filesT.text = "Storage Capacity: " + filesV.ToString() + " Gb";
//        dataOnDemandT.text = "Computing Power: " + dataOnDemandV.ToString() + " Gfps"; ;
//        bandwidthT.text = "BandWidthc " + bandwidthV.ToString() + " Mbps";
//        wattsT.text = "Energy: " + wattsV.ToString() + " MW";
//    }

//    //Deactivating GameObjects (panels and texts) and animating
//    void animate0()
//    {
//        float time = 0f;
//        float delay = 0;
//        float scale = 0;

//        if (!hide_loader)
//            LOADER.SetActive(true);

//        PANELDATOS.SetActive(false);
//        PANELG1.SetActive(false);
//        PANELG2.SetActive(false);
//        PANELG3.SetActive(false);
//        //NODE1.SetActive(false);
//        //LINE1.SetActive(false);
//        //LINE2.SetActive(false);
//        //LINE1FX.SetActive(false);
//        //LINE2FX.SetActive(false);
//        //ARROW.SetActive(false);
//        //MASMENOS.SetActive(false);
//        //BUTTONS1.SetActive(false);
//        //BUTTONS2.SetActive(false);
//        //BUTTONS3.SetActive(false);
//        //panelINFO_SUP.SetActive(false);
//        //panelINFO_SUP_INFO.SetActive(false);
//        node_info1.SetActive(false);

//        node_info3.SetActive(false);
//        panelINFO_INF.SetActive(false);
//        //nodeP.SetActive(false);

//        CancelInvoke("animate1");
//        if (!hide_loader)
//            Invoke("animate1", 3.3f);
//        else
//            animate1();
//    }

//    public void show_nodeinfo()
//    {
//        update_data_node_byid(1);
//    }

//    public void anim_nodeinfo()
//    {
//        node_info1.GetComponent<anim_image>().Anim_image(0f, 2);

//        node_info3.GetComponent<anim_image>().Anim_image(0.4f, 2);

//    }


//    //enable node info and Activating panels
//    void animate1()
//    {
//        LOADER.SetActive(false);

//        CancelInvoke("enable_nodeP");
//        Invoke("enable_nodeP", 3.0f);
//        CancelInvoke("enable_node_info");
//        Invoke("enable_node_info", 2.5f);

//        PANELDATOS.SetActive(true);

//        float time = 0.5f;
//        float delay = 1;
//        float scale = 0.5f;

//        CancelInvoke("reload_data");
//        Invoke("reload_data", time + delay + 2);

//        panelINFO_INF.SetActive(true);

//        PANELG1.SetActive(true);
//        PANELG2.SetActive(true);
//        PANELG3.SetActive(true);
//    }

//    void enable_nodeP()
//    {
//        //nodeP.SetActive(true);
//    }

//    void enable_node_info()
//    {
//        CancelInvoke("anim_nodeinfo");
//        Invoke("anim_nodeinfo", 0.5f);
//    }

//    void update_daytime()
//    {
//        DateTime theTime = DateTime.Now;
//        string date = theTime.ToString("yyyy-MM-dd\\ HH:mm");
//        Infodaytimetext.text = date;
//    }

//    void reload_data()
//    {
//        onlineNodesT.text = "";
//        filesT.text = "";
//        dataOnDemandT.text = "";
//        bandwidthT.text = "";
//        wattsT.text = "";
//        totalT.text = "";

//        //get global resources
//        DATA.getNetWorkStatus();

//        CancelInvoke("reload_data");
//        Invoke("reload_data", reload_time);
//    }

//    void relad_data_request()
//    {


//    }

//    public void go_home()
//    {

//        CancelInvoke("getNodesOnline");
//        SceneManager.LoadScene("home");
//    }
//}