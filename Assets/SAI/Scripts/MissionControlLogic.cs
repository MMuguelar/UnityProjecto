using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;

public class MissionControlLogic : MonoBehaviour
{
    public TMP_Text txtdeviceID;
    public TMP_Text txtMSGCount;
    public TMP_Text txtboardTemp;
    public TMP_Text txtBatteryAndTemp;
    public TMP_Text txtGrav_xyz;
    public TMP_Text txtAccel_xyz;
    public TMP_Text txtLatLon;
    public TMP_Text txtAltitude;
    public TMP_Text txtMsn1;
    public TMP_Text txtMsn2;
    public TMP_Text txtMsn3;
    public TMP_Text txtMsn4;
    public TMP_Text txtMsn5;
    public TMP_Text txtMsn6;

    public float reload_time = 3;

    public string deviceID;
    public string MSGCount;
    public string boardTemp;
    public string Battery;
    public string Grav_x;
    public string Grav_y;
    public string Grav_z;
    public string Accel_x;
    public string Accel_y;
    public string Accel_z;
    public string Lat;
    public string Lon;
    public string Altitude;
    public string msn1;
    public string msn2;
    public string msn3;
    public string msn4;
    public string msn5;
    public string msn6;

    public GameObject ROCKET;
    public GameObject PUNTO;
    public GameObject toggleActive;
    public GameObject toggleSleep;
    public Vector3[] coordenadas;

    public string server = "http://192.168.4.1/main/actualizar_data";
    

    void Start()
    {
        reload_data();
    }

    void Update()
    {

    }

    public string chipID = "";
    public string firmwareESP = "";
    public int txStart;
    public int txTime;
    public int datalogTime;
    public string status = "";
    public string remoteId = "";

    private void parse_config(string[] linesInFile)
    {
        Debug.Log(linesInFile);
        deviceID = linesInFile[0].Split('|')[1];
        chipID = linesInFile[1].Split('|')[1];
        firmwareESP = linesInFile[2].Split('|')[1];

        txStart = int.Parse(linesInFile[3].Split('|')[1]);
        txTime = int.Parse(linesInFile[4].Split('|')[1]);
        datalogTime = int.Parse(linesInFile[5].Split('|')[1]);
        status = linesInFile[6].Split('|')[1];
        remoteId = linesInFile[7].Split('|')[1];
        MSGCount = linesInFile[8].Split('|')[1];

        boardTemp = linesInFile[9].Split('|')[1];
        Accel_x = (linesInFile[10].Split('|')[1].Replace('.', ','));
        Accel_y = (linesInFile[11].Split('|')[1].Replace('.', ','));
        Accel_z = (linesInFile[12].Split('|')[1].Replace('.', ','));

        Grav_x = (linesInFile[13].Split('|')[1].Replace('.', ','));
        Grav_y = (linesInFile[14].Split('|')[1].Replace('.', ','));
        Grav_z = (linesInFile[15].Split('|')[1].Replace('.', ','));

        Lat = linesInFile[16].Split('|')[1].Replace('.', ',');
        Lon = linesInFile[17].Split('|')[1].Replace('.', ',');
        Altitude = linesInFile[18].Split('|')[1].Replace('.', ',');
        Battery = linesInFile[19].Split('|')[1].Replace('.', ',');

        msn1 = linesInFile[20].Split('|')[1];
        msn2 = linesInFile[21].Split('|')[1];
        msn3 = linesInFile[22].Split('|')[1];
        msn4 = linesInFile[23].Split('|')[1];
        msn5 = linesInFile[24].Split('|')[1];
        msn6 = linesInFile[25].Split('|')[1];

        update_values();
    }

    //private void parse_config(string[] linesInFile)
    //{
    //    string exmpl = linesInFile[0];
    //    linesInFile = exmpl.Split('	');

    //    deviceID = linesInFile[0];
    //    MSGCount = (linesInFile[1]);

    //    boardTemp = (linesInFile[2]);
    //    Accel_x = (linesInFile[3]);
    //    Accel_y = (linesInFile[4]);
    //    Accel_z = (linesInFile[5]);

    //    Grav_x = (linesInFile[6]);
    //    Grav_y = (linesInFile[7]);
    //    Grav_z = (linesInFile[8]);

    //    Lat = (linesInFile[9]);
    //    Lon = (linesInFile[10]);
    //    Altitude = (linesInFile[11]);

    //    update_values();
    //}


    void update_values()
    {
        txtdeviceID.text = deviceID;
        txtMSGCount.text = MSGCount;
        txtboardTemp.text = boardTemp;
        txtBatteryAndTemp.text = Battery + "% / " + boardTemp;
        txtGrav_xyz.text = "X: " + Grav_x.TruncateNum(7) + "\nY: " + Grav_y.TruncateNum(7) + "\nZ: " + Grav_z.TruncateNum(7);
        txtAccel_xyz.text = "X: " + Accel_x.TruncateNum(7) + "\nY: " + Accel_y.TruncateNum(7) + "\nZ: " + Accel_z.TruncateNum(7);
        txtLatLon.text = "LAT: " + Lat.TruncateNum(7) + "\nLON: " + Lon.TruncateNum(7);
        txtAltitude.text = Altitude;
        txtMsn1.text = msn1;
        txtMsn2.text = msn2;
        txtMsn3.text = msn3;
        txtMsn4.text = msn4;
        txtMsn5.text = msn5;
        txtMsn6.text = msn6;

        float v1 = float.Parse(Grav_x);
        float v2 = float.Parse(Grav_z);
        float v3 = float.Parse(Grav_y);

        Maps.lat = Convert.ToDouble(Lat);
        Maps.lon = Convert.ToDouble(Lon);

        Debug.Log("Lat: " + Lat);

        Vector3 coordenadas = new Vector3(v1, v2, v3);

        update_coords(coordenadas);
        //ROCKET.transform.localPosition = new Vector3(0, 0, 0);
    }

    IEnumerator GetRequest(string uri)
    {

        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + uri + " : " + webRequest.error);
            }
            else
            {
                //Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);

                string[] data = webRequest.downloadHandler.text.Split('\n');
                parse_config(data);

            }
        }
    }

    void reload_data()
    {
        StartCoroutine(GetRequest(server));

        CancelInvoke("reload_data");
        Invoke("reload_data", reload_time);
    }

    void update_coords(Vector3 coords)
    {
        PUNTO.transform.localPosition = coords;
    }

    public void TogglePressed()
    {
        if (toggleActive.GetComponent<Toggle>().isOn)
        {
            Debug.Log("Active");
            StartCoroutine(SendActionToRocket("start"));
        }
        else
        {
            Debug.Log("Sleep");
            StartCoroutine(SendActionToRocket("sleep"));
        }
    }


    IEnumerator SendActionToRocket(string action)
    {

        string uri="http://192.168.4.1/main/send_cmd?status=" + action;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log("Error: " + uri + " : " + webRequest.error);
            }
            else
            {
                Debug.Log("Send Ok: " + webRequest.downloadHandler.text);            }
        }
    }

}
