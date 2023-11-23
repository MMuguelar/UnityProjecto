//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.SceneManagement;
//using TMPro;

//public class Main_setup : MonoBehaviour
//{
//    public bool Portrait = false;
//    public GameObject Pantalla0;
//    public GameObject Pantalla1;
//    public GameObject ITEM1;
//    public GameObject BLOCK;
//    public string next_scene = "ipfs";

//    public float A_energy=0;
//    public float A_storage = 0;
//    public float A_processing = 0;
//    public float A_comm = 0;

//    public Slider slider0; //energy no implementado
//    public Slider slider1;
//    public Slider slider2;
//    public Slider slider3;
//    public DataService DATA;
//    public TMP_Text INFO;

//    public TMP_Text INFO_GEO;
//    public TMP_Text INFO_LATLONG;
//    public bool configuradoGPS = false;
//    public Button NEXTBT;


//    public bool isWEB = false;
//    void Start()
//    {
//        NEXTBT.interactable = false;
//        DATA = GameObject.Find("DATA").GetComponent<DataService>();
//        isWEB = DATA.IS_WEB;
//        BLOCK.SetActive(false);

//        Pantalla0.SetActive(false);
//        Pantalla1.SetActive(false);


//        update_values_slides();
//        DATA.USER_PROFILE.name = "Node" + SystemInfo.deviceUniqueIdentifier;

//        INFO.text = DATA.USER_PROFILE.name;

//        DATA.USER_PROFILE.metadata = SystemInfo.deviceName+" "+ SystemInfo.deviceModel;

//        if (DATA.USER_PROFILE.configurado == true)
//        {
//            CancelInvoke("GET_LOCATION_REPEATING");
//            //DATA.GetUserLocation();
//            go_next_scene();
//        }
//        else
//        {
//            if (!isWEB)
//            {
//                CancelInvoke("GET_LOCATION_REPEATING");
//                Invoke("GET_LOCATION_REPEATING", 1);

//                CancelInvoke("enable_next");
//                Invoke("enable_next", 60);
//            }
//            else
//            {
//                enable_next();
//            }

//            Pantalla0.SetActive(true);
//            detect_resolution_and_adjust();
//        }
//    }

//    bool geocodeded = false;
  
//    void GET_LOCATION_REPEATING()
//    {
//        DATA.GetUserLocation();
//        CancelInvoke("GET_LOCATION_REPEATING");
//        Invoke("GET_LOCATION_REPEATING",5);
//    }

//    public void FAIL_GPS()
//    {
//        CancelInvoke("GET_LOCATION_REPEATING");     
//        Invoke("GET_LOCATION_REPEATING", 2);
//    }

//    public void FAIL_GPS2()
//    {
//        FAIL_GPS();
//    }


//    public void enable_next()
//    {
//        CancelInvoke("GET_LOCATION_REPEATING");
//        NEXTBT.interactable = true;
//    }

 
   

//    public void UPDATE_GEO_LATLONG_TEXT(string DATOS)
//    {
//        INFO_LATLONG.text = DATOS;
//        CancelInvoke("GET_LOCATION_REPEATING");


//        if (!geocodeded)
//            DATA.getGeoCodingReverse();
//    }

//    public void UPDATE_GEO_DATA_TEXT(string DATOS)
//    {
//        INFO_GEO.text = DATOS;
//        geocodeded = true;

//        CancelInvoke("GET_LOCATION_REPEATING");
//        enable_next();
//    }

//    public void update_values_slides()
//    {
//        print("update_values_slides");
//        slider1.value = DATA.USER_PROFILE.storage;       
//        slider2.value = DATA.USER_PROFILE.processing;
//        slider3.value = DATA.USER_PROFILE.comm;
//    }


//    public void detect_resolution_and_adjust()
//    {
//        if (Screen.width > Screen.height)
//            Portrait = false;
//        else
//            Portrait = true;

//        if (Portrait==false)
//        {
//            //ITEM1.GetComponent<RectTransform>().anchorMin = new Vector2(0.33f, 0);
//            //TEM1.GetComponent<RectTransform>().anchorMax = new Vector2(0.67f, 1);
//        }
//        else
//        {
//           // ITEM1.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
//           // ITEM1.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
//        }
//    }

//    public void go_datos1()
//    {
//        Pantalla0.SetActive(false);
//        Pantalla1.SetActive(true);

//    }

//    public void go_next()
//    {

//        BLOCK.SetActive(true);

//       A_energy = 50;
//       A_storage = slider1.value;
//       A_processing = slider2.value;
//       A_comm = slider3.value; 

//        //value.ToString("F2");

//        DATA.USER_PROFILE.configurado = true;
//        DATA.USER_PROFILE.energy = A_energy;
//        DATA.USER_PROFILE.storage = A_storage;
//        DATA.USER_PROFILE.processing = A_processing;
//        DATA.USER_PROFILE.comm = A_comm;
//        DATA.nodoACTIVADO = false;
//        DATA.save_user_data();

//        CancelInvoke("go_next_scene");
//         Invoke("go_next_scene",1);
//    }

//    public void go_next_scene()
//    {

//        SceneManager.LoadScene(next_scene);
//    }


//    // Update is called once per frame
//    void Update()
//    {
        
//    }
//}
